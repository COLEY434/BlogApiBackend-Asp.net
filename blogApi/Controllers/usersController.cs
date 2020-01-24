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
       

        // Gets all users 
        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            try
            {
                var users = await uow.User.GetAllUsersAsync();
                if (users == null)
                {
                    return NotFound();
                }

                return Ok(users);
            }
            catch(Exception ex)
            {
                return Ok(ex.Message);
            }
           
        }


        //Get male users 
        [HttpGet]
        [Route("male")]
        public async Task<IActionResult> GetMalesUsersAsync()
        {
            try
            {
                var users = await uow.User.GetMaleUsers();
                if (users == null)
                {
                    return NotFound();
                }

                return Ok(users);
            }
            catch (Exception ex)
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
                var users = await uow.User.GetUserById(id);
                if (users == null)
                {
                    return NotFound();
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        // updates the user record
        [HttpPut("edit-profile/{id}")]
        public async Task<IActionResult> UpdateUsersAsync([FromRoute] int id, [FromBody] UserUpdateDTO UserModel)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var users = await uow.User.GetUserByIdT(id);
                if (users != null)
                {

                    users.firstname = UserModel.Firstname;
                    users.surname = UserModel.Surname;
                    users.state = UserModel.State;
                    users.gender = UserModel.Gender;
                    users.age = UserModel.Age;
                    users.updated_at = DateTime.Now;
                    users.email = UserModel.Email;
                    users.username = UserModel.Username;
                    users.country = UserModel.Country;

                    uow.User.Update(users);
                    await uow.save();

                    return Ok(new { success = true, message = "Profile Data Updated Successfully" });
                }

                return Ok(new { success = false, Message = "Failed to update" });
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpPost]
        [Route("upload-image/{userId}")]
        public async Task<IActionResult> UploadUserImageAsync([FromRoute] int userId, [FromForm] FileUpload model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var User = await uow.User.GetUserByIdT(userId);

            var file = model.Photo;
            var maxSize = 5000000;
            var fileNameAndExtension = file.FileName;
            var fileExtension = Path.GetExtension(fileNameAndExtension);
            string[] allowedExtensions = { ".jpg", ".png", ".jpeg" };

            //bool isUploaded = false;

            if (file.Length < maxSize)
            {
                if (allowedExtensions.Contains(fileExtension))
                {
                    
                    //var destinationPath = Path.Combine(@"C:\Users\user\Pictures\Images\ProfilePictures", NewFileName);                    
                    // var img_url = $"http://127.0.0.1:8887/ProfilePictures/{NewFileName}";

                    var NewFileNameForAzure = userId + "-" + Guid.NewGuid() + fileExtension;
                    var connectionString = "DefaultEndpointsProtocol=https;AccountName=collinsazurestorage434;AccountKey=dlvvVNb0SC5AzAwHADZTy/+2h1xLSCnXDvu0huySWH56zVQBXnuISg6Q7LRTD9AZRtqW0R9yzGqUJFYhUtoAbA==;EndpointSuffix=core.windows.net";
                    //connect to azure storage
                    BlobContainerClient containerClient = new BlobContainerClient(connectionString, "img");
                    // Get a reference to a blob
                    BlobClient blobClient = containerClient.GetBlobClient(NewFileNameForAzure);
                    try
                    {
                        var uri = "";
                        using (Stream filetoupload = file.OpenReadStream())
                        {
                            await blobClient.UploadAsync(filetoupload);
                             uri = blobClient.Uri.AbsoluteUri;
                        }

                        User.img_url = uri;
                        User.updated_at = DateTime.Now;

                        uow.User.Update(User);
                        await uow.save();

                        return Ok(new { success = true, message = "Image Uploaded Successfully", pic = uri });

                    }
                    catch(Exception e)
                    {

                    }


                    //using (var fileStream = new FileStream(destinationPath, FileMode.Create))
                    //{
                    //    await file.CopyToAsync(fileStream);

                    //    isUploaded = true;

                    //    if(isUploaded)
                    //    {
                    //        try
                    //        {
                    //            User.img_url = img_url;
                    //            User.updated_at = DateTime.Now;

                    //            uow.User.Update(User);
                    //            await uow.save();

                    //            return Ok(new { success = true, message = "Image Uploaded Successfully", pic = img_url });
                    //        }
                    //        catch(Exception ex)
                    //        {
                    //            return Ok(ex.Message);
                    //        }

                    //    }


                    //}
                }
                return Ok(new { success = false, message = "Please only images with png and Jpg extensions are allowed" });
            }
            else
            {
                return Ok(new { success = false, message = "Please select an image less than 5 mb" });
            }

           


        }

        // Creates the user record
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateUserAsync(UserWriteDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                try
                {
                var checkIfUserExist = await uow.User.ValidateUser(user.password, user.email);

                if(checkIfUserExist == null)
                {
                    var userInfo = new users();

                    userInfo.firstname = null;
                    userInfo.surname = null;
                    userInfo.state = null;
                    userInfo.gender = null;
                    userInfo.age = null;
                    userInfo.created_at = DateTime.Now;
                    userInfo.updated_at = null;
                    userInfo.email = user.email;
                    userInfo.password = user.password;
                    userInfo.img_url = null;
                    userInfo.last_logged_In = null;
                    userInfo.country = null;
                    userInfo.username = null;

                    uow.User.Create(userInfo);
                    await uow.save();


                    var userExist = await uow.User.ValidateUser(user.password, user.email);

                    if (userExist != null)
                    {
                        return Ok(new { success = true, userId = userExist.Id });
                    }
                    // return Ok(new { success = true, message = "Logged in successful" });
                }
                return Ok(new { success = false, message = "User account exist" });
            }
                catch (Exception ex)
                {
                    return Ok(ex.Message);
                }

        }

        //Route for authenticating the user
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> AuthUserAsync(LoginWriteDTO model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new { error = true });
            }
            try
            {
                var userExist = await uow.User.ValidateUser(model.password, model.email);

                if (userExist != null)
                {
                    return Ok(new { success = true, userId = userExist.Id });
                }
                else
                {
                    return Ok(new { success = false });
                }

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }




        //DELETE: 
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<users>> Deleteusers(int id)
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
