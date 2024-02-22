using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopOnlineAPI.Data;
using ShopOnlineAPI.Infrastructure.Dtos;
using ShopOnlineAPI.Models;
using ShopOnlineAPI.Utils;
using ShopOnlineCore.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopOnlineAPI.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {

        public IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public TokenController(IConfiguration config, ApplicationDbContext context)
        {
            _configuration = config;
            _context = context;
        }
        /// <summary>
        /// Create a token access to a register client.
        /// </summary>
        /// <param name="_userData"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> Post(UserInfo _userData)
        {
            if (_userData != null && _userData.email != null && _userData.password != null)
            {
                var user = await GetUser(_userData);

                if (user != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.Id),
                        new Claim("UserName", user.Name),
                        new Claim("Email", user.Email)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(60),
                        signingCredentials: signIn);

                    return ApiResult.Success(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return ApiResult.BadRequest("Invalid credentials");
                }
            }
            else
            {
                return ApiResult.BadRequest();
            }
        }

        private async Task<ClientModel> GetUser(UserInfo data)
        {
            var client = AutoMapperProfile.Map<UserInfo,ClientModel>(data, true);
            return await _context.Client.FirstOrDefaultAsync(u => u.Email == client.Email && u.Password == client.Password);
        }
    }
}
