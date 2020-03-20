using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using blogApi.Entities;
using blogApi.DTOS.WriteDTO;
using blogApi.DAL.Login;
using blogApi.Interfaces;
using Microsoft.AspNetCore.Cors;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Azure.Storage.Blobs;
using blogApi.DTOS.ReadDTO;

namespace blogApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class usersController : ControllerBase
    {
        private IRepositoryUnitOfWork uow;
        public usersController(IRepositoryUnitOfWork unitOfWork)
        {
            uow = unitOfWork;
        }
       

        // Gets all users from the database
        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            try
            {
                var users = await uow.User.GetAllUsersAsync();
                if (users == null)
                {
                    return Ok(new {ErrorMessage = "Users not found" });
                }

                return Ok(users);
            }
            catch(Exception ex)
            {
                return Ok(ex.Message);
            }
           
        }


        // Gets users by id
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute] int id)
        {
            try
            {
                var user = await uow.User.GetUserById(id);
                if (user == null)
                {
                    return Ok(new { ErrorMessage = "User not found" });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        
    [HttpPut]
    [Route("change-profile/{UserId}")]
    public async Task<IActionResult> ChangeUserPasswordAsync([FromRoute] int UserId, [FromBody] ChangePassword passwordInfo)
    {
        try
        {
            var user = await uow.User.GetUserByIdT(UserId);
            if (Convert.ToInt32(user.password) != passwordInfo.OldPassword)
            {
                return Ok(new { success = false });
            }

            user.password = passwordInfo.NewPassword.ToString();
            uow.User.Update(user);
            await uow.save();

            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            return Ok(ex.Message);
        }
    }
    // updates the user record
    [HttpPost("edit-profile/{userId}")]
     public async Task<IActionResult> UpdateUsersAsync([FromRoute] int userId, [FromForm] UserUpdateDTO UserModel)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = await uow.User.GetUserByIdT(userId);
                if (user != null && UserModel.Photo != null)
                {
                    var file = UserModel.Photo;
                    var maxSize = 3145728;
                    var fileNameAndExtension = file.FileName;
                    var fileExtension = Path.GetExtension(fileNameAndExtension);
                    string[] allowedExtensions = { ".jpg", ".png", ".jpeg" };

                    if (file.Length <= maxSize)
                    {
                        if (allowedExtensions.Contains(fileExtension))
                        {

                            var NewFileNameForAzure = userId + "-" + Guid.NewGuid() + fileExtension;
                            var connectionString = "DefaultEndpointsProtocol=https;AccountName=collinsazurestorage434;AccountKey=dlvvVNb0SC5AzAwHADZTy/+2h1xLSCnXDvu0huySWH56zVQBXnuISg6Q7LRTD9AZRtqW0R9yzGqUJFYhUtoAbA==;EndpointSuffix=core.windows.net";
                            //connect to azure storage
                            BlobContainerClient containerClient = new BlobContainerClient(connectionString, "img");
                            // Get a reference to a blob
                            BlobClient blobClient = containerClient.GetBlobClient(NewFileNameForAzure);
                            
                            
                                var uri = "";
                                using (Stream filetoupload = file.OpenReadStream())
                                {
                                    await blobClient.UploadAsync(filetoupload);
                                    uri = blobClient.Uri.AbsoluteUri;
                                }

                                user.img_url = uri;
                                user.updated_at = DateTime.Now;
                                user.firstname = UserModel.Firstname;
                                user.surname = UserModel.Surname;
                                user.email = UserModel.Email;
                                user.username = UserModel.Username;
                                user.country = UserModel.Country;

                                uow.User.Update(user);
                                await uow.save();

                            var UpdatedUserInfo = new UserReadDTO
                            {
                                userId = user.userId,
                                surname = user.surname,
                                firstname = user.firstname,
                                username = user.username,
                                age = user.age,
                                state = user.state,
                                date_joined = user.created_at,
                                country = user.country,
                                email = user.email,
                                gender = user.gender,
                                img_url = user.img_url
                            };

                            return Ok(new { success = true, userData = UpdatedUserInfo });                              
                        }
                        return Ok(new { success = false, message = "Please only images with png and Jpg extensions are allowed" });
                    }
                    else
                    {
                        return Ok(new { success = false, message = "Please select an image less than 3 mb" });
                    }
                }
                else
                {
                    user.updated_at = DateTime.Now;
                    user.firstname = UserModel.Firstname;
                    user.surname = UserModel.Surname;
                    user.email = UserModel.Email;
                    user.username = UserModel.Username;
                    user.country = UserModel.Country;

                    uow.User.Update(user);
                    await uow.save();

                    var UpdatedUserInfo = new UserReadDTO
                    {
                        userId = user.userId,
                        surname = user.surname,
                        firstname = user.firstname,
                        username = user.username,
                        age = user.age,
                        state = user.state,
                        date_joined = user.created_at,
                        country = user.country,
                        email = user.email,
                        gender = user.gender,
                        img_url = user.img_url
                    };

                    return Ok(new { success = true, userData = UpdatedUserInfo });
                }  
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

    

    
        //DELETE: 
    [HttpDelete("delete/{id}")]
     public async Task<ActionResult<users>> DeleteUserAsync([FromRoute] int id)
        {
            try
            {
                var users = await uow.User.GetUserByIdT(id);
                if (users != null)
                {
                    uow.User.Delete(users);
                    await uow.save();

                    return Ok(new { success = true });
                }

                return Ok(new { success = false, Message = "Failed to delete"});
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
