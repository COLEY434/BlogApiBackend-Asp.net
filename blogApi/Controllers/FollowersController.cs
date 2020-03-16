using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blogApi.DAL;
using blogApi.Entities;
using blogApi.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace blogApi.Controllers
{
    [Route("api/follow")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FollowersController : ControllerBase
    {
        private readonly IRepositoryUnitOfWork _uow;
        public FollowersController(IRepositoryUnitOfWork uow)
        {
            _uow = uow;
        }
        [HttpPost()]
        [Route("{userToFollowOrUnfollowId}/{userThatWantToFollowORUnfollowId}")]
        public async Task<IActionResult> FollowUserAsync([FromRoute] int userToFollowOrUnfollowId, [FromRoute] int userThatWantToFollowORUnfollowId)
        {
             try
             {
                var CheckIfFollowingIsTrue = await _uow.Followers.CheckIfFollowerExistAsync(userToFollowOrUnfollowId, userThatWantToFollowORUnfollowId);

                if (CheckIfFollowingIsTrue == null)
                {
                    var Follower = new Followers
                    {
                        user_Id = userToFollowOrUnfollowId,
                        follower_Id = userThatWantToFollowORUnfollowId,
                        isFollowing = true
                    };

                    _uow.Followers.Create(Follower);
                    await _uow.save();
                    return Ok(new { following = true });
                }
                else
                {
                    if (CheckIfFollowingIsTrue.isFollowing == true)
                    {
                        CheckIfFollowingIsTrue.isFollowing = false;
                        _uow.Followers.Update(CheckIfFollowingIsTrue);
                        await _uow.save();

                        return Ok(new { following = false });
                    }
                    else
                    {
                        CheckIfFollowingIsTrue.isFollowing = true;
                        _uow.Followers.Update(CheckIfFollowingIsTrue);
                        await _uow.save();

                        return Ok(new { following = true });
                    }
                }

               
            }
            catch(Exception ex)
            {
                return Ok(ex.Message);

            }
           
        }
  
        [HttpGet]
        [Route("get-Following-status/{userId}/{followersUserId}")]
        public async Task<IActionResult> GetFollowingStatusAsync([FromRoute] int userId, [FromRoute] int followersUserId)
        {       
            try
            {
                var FollowingStatus = await _uow.Followers.GetFollowingStatusAsync(userId, followersUserId);

                return Ok(new { following = FollowingStatus });
            }
            catch(Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet]
        [Route("get-followers/{userId}")]
        public async Task<IActionResult> GetFollowersAsync([FromRoute] int userId)
        {
            try
            {
                var Followers = await _uow.Followers.GetFollowersAsync(userId);

                if(Followers.Count() == 0)
                {
                    return Ok(new { success = true, message = "No followers" });
                }
                return Ok(new { success = true, followers = Followers });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = "Sorry, internal server error, please contact customer support" });
            }
        }

        [HttpGet]
        [Route("get-followings/{userId}")]
        public async Task<IActionResult> GetFollowingsAsync([FromRoute] int userId)
        {
            try
            {
                var Followings = await _uow.Followers.GetFollowingsAsync(userId);

                if (Followings.Count() == 0)
                {
                    return Ok(new { success = true, message = "No followings" });
                }
                return Ok(new { success = true, followings = Followings });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = "Sorry, internal server error, please contact customer support" });
            }
        }
    }
}