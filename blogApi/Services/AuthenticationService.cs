       using blogApi.DAL;
using blogApi.DTOS.ReadDTO;
using blogApi.Entities;
using blogApi.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace blogApi.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly RepositoryContext _dataContext;
        public AuthenticationService(IOptions<JwtSettings> jwtSettings, RepositoryContext dataContext)
        {
            _jwtSettings = jwtSettings.Value;
            _dataContext = dataContext;
        }


        public async Task<AuthenticationResult> AuthenticateUserAsync(string email, string password)
        {
            var user = await _dataContext.users.AsNoTracking().Where(x => x.email == email && x.password == password).SingleOrDefaultAsync();

            if(user == null)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    ErrorMesage = "Invalid username or password"
                };
            }

            return GenerateTokenAsync(user);
        }

        public async Task<bool> FindUserAsync(string email, string password)
        {
            var exists = await _dataContext.users.Where(x => x.email == email && x.password == password).SingleOrDefaultAsync();

            if(exists != null)
            {
                return true;
            }

            return false;
        }

        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
        {
                var userInfo = new users();
                userInfo.firstname = null;
                userInfo.surname = null;
                userInfo.state = null;
                userInfo.gender = null;
                userInfo.age = null;
                userInfo.created_at = DateTime.Now;
                userInfo.updated_at = null;
                userInfo.email = email;
                userInfo.password = password;
                userInfo.img_url = null;
                userInfo.last_logged_In = null;
                userInfo.country = null;
                userInfo.username = null;

                _dataContext.users.Add(userInfo);
                await _dataContext.SaveChangesAsync();

                var newUser = await _dataContext.users.Where(x => x.email == email && x.password == password).SingleOrDefaultAsync();

                return GenerateTokenAsync(newUser);
            
        }
        private AuthenticationResult GenerateTokenAsync(users user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Email, user.email),
                    new Claim("id", user.userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                UserId = user.userId,
                ExpiresIn = tokenDescriptor.Expires,
                Username = user.username
            };

            
        }
    }
}
