using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blogApi.DTOS.WriteDTO;
using blogApi.Entities;
using blogApi.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace blogApi.Controllers
{
    [Route("api/post")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostController : ControllerBase
    {
        private IRepositoryUnitOfWork uow;
        public PostController(IRepositoryUnitOfWork unitOfWork)
        {
            uow = unitOfWork;
        } 

        // GET: api/Post
        [HttpGet]
        [Route("get-posts")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostsAsync()
      {
            try
            {
                var AllPost = await uow.Post.GetPostsAsync();

                return Ok(AllPost);
            }
            catch(Exception ex)
            {

                return Ok(ex.Message);
            }
           
        }

        [HttpGet]
        [Route("user/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserPostsAsync([FromRoute] int userId)
        {
            try
            {
                var AllPost = await uow.Post.GetUsersPostsAsync(userId);

                return Ok(AllPost);
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }

        }

        [HttpGet]
        [Route("get-comments/{postId}")]
        public async Task<IActionResult> GetCommentssAsync([FromRoute] int postId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var comments = await uow.Post.GetCommentsAsync(postId);

                if(comments == null)
                {
                    return Ok(new { success = false, message = "No comment for this post"});
                }
                return Ok(new { success = true, comments = comments });
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        // GET: api/Post/5
        [HttpPut("update")]
        public async Task<IActionResult> updatePostAsync([FromBody] PostWriteUpdateDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var postInfo = await uow.Post.GetPostById(model.post_Id);
                if (postInfo == null)
                {
                    return Ok(new { success = true, message = "Failed to update post" });
                }

                postInfo.message = model.message;
                postInfo.updated_at = DateTime.Now;
                uow.Post.Update(postInfo);
                await uow.save();

                var newPostInfo = await uow.Post.GetPostsByIdSingle(model.post_Id);

                return Ok(new { success = true, message = "Post Updated successfully", userInfo = newPostInfo });

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
            

           

        }

        [HttpPost]
        [Route("reply/create")]
        public async Task<IActionResult> createPostRepliesAsync([FromBody] PostRepliesWriteDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var ReplyData = new Replies();
                ReplyData.user_Id = model.user_Id;
                ReplyData.post_Id = model.post_Id;
                ReplyData.reply_message = model.message;
                ReplyData.created_at = DateTime.Now;
                ReplyData.updated_at = null;

                uow.Replies.Create(ReplyData);
                await uow.save();

                return Ok(new { success = true, message = "Reply sent successfully"});
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        // POST: api/Post/create

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> createPostAsync(PostWriteDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var postData = new posts();
                    postData.user_id = model.user_Id;
                    postData.message = model.message;
                    postData.created_at = DateTime.Now;
                    postData.updated_at = null;

                uow.Post.Create(postData);
                await uow.save();

                return Ok(new { success = true });
            }
            catch(Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        //DELETE: 
        [HttpDelete("delete/{postId}")]
        public async Task<IActionResult> DeletePostAsync([FromRoute] int postId)
        {
            try
            {
                var post = await uow.Post.GetPostById(postId);

                if (post == null)
                {
                    return Ok(new { success = false, message="Post deleted"});
                }

                var PostComments = await uow.Replies.GetPostComments(postId);

                foreach(var comment in PostComments)
                {
                    uow.Replies.Delete(comment);
                }
                 
                uow.Post.Delete(post);
                await uow.save();

                return Ok(new { success = true, message = "Post deleted successfully" });
            }
            catch (Exception ex)
            {
                //log to database first
                return Ok(new { success = false, message = "oops!!! something is wrong with the server, please contact the admin" });
            }
           

        }
    }
}
