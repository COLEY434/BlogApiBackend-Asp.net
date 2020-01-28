using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blogApi.DAL;
using blogApi.DTOS.ReadDTO;
using blogApi.DTOS.WriteDTO;
using blogApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace blogApi.Controllers
{
    [Route("api/authenticate")]
    [ApiController]
    
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var authResponse = await _authService.AuthenticateUserAsync(request.Email, request.Password);

                if (!authResponse.Success)
                {
                    return Ok(new AuthFailedResponse
                    {
                        Success = authResponse.Success,
                        ErrorMessage = authResponse.ErrorMesage
                    });
                }

                return Ok(new AuthSuccessResponse
                {
                    Success = authResponse.Success,
                    UserId = authResponse.UserId,
                    ExpiresIn = authResponse.ExpiresIn,
                    Token = authResponse.Token,
                    Username = authResponse.Username
                });
            }
            catch(Exception ex)
            {//add logging to database later
                return Ok(ex);
            }
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] AuthenticationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var CheckIfUserExist = await _authService.FindUserAsync(request.Email, request.Password);

                if (!CheckIfUserExist)
                {
                    var authResponse = await _authService.RegisterAsync(request.Email, request.Password);

                    return Ok(authResponse);
                }

                return Ok(new AuthFailedResponse
                {
                    Success = false,
                    ErrorMessage = "User with this email already exist"
                });
            }
            catch(Exception ex)
            {
                //add logging to the database later
                return Ok(ex);
            }

        }
    }
}