using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using blogApi.Entities;
using blogApi.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace blogApi.Controllers
{
    [Route("api/likes")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LikesController : ControllerBase
    {
        private IRepositoryUnitOfWork uow;

        public LikesController(IRepositoryUnitOfWork unitOfWork)
        {
            uow = unitOfWork;
        }


        [HttpGet]
        [Route("get-likes")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostsAsync()
        {
            try
            {
                var Likes = await uow.Likes.GetLikesAsync();

                return Ok(Likes);
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }

        }

        [HttpPost]
        [Route("like-post/{postId}/{userId}")]
        public async Task<IActionResult> LikePostAsync([FromRoute] int postId, [FromRoute] int userId)
        {
            try
            {
                var checkIfLikeExists = await uow.Likes.checkIfLikeExist(postId, userId);

                if(checkIfLikeExists == null)
                {
                    var newLike = new Likes();
                    newLike.post_Id = postId;
                    newLike.user_Id = userId;
                    newLike.liked = true;

                    uow.Likes.Create(newLike);
                    await uow.save();

                    return Ok(new { success = true });
                }
                else
                {
                   if(checkIfLikeExists.liked == true)
                    {
                        checkIfLikeExists.liked = false;
                        uow.Likes.Update(checkIfLikeExists);
                        await uow.save();

                        return Ok(new { success = false });
                    }
                   else
                    { 
                        checkIfLikeExists.liked = true;
                        uow.Likes.Update(checkIfLikeExists);
                        await uow.save();
                        return Ok(new { success = true });
                    }
                }   
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }

        }
    }
}