﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blogApi.DTOS.WriteDTO;
using blogApi.Entities;
using blogApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace blogApi.Controllers
{
    [Route("api/post")]
    [ApiController]
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

        // GET: api/Post/5
        [HttpPut("update")]
        public async Task<IActionResult> updatePostAsync([FromBody] PostWriteUpdateDTO model)
        {
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

        // PUT: api/Post/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
