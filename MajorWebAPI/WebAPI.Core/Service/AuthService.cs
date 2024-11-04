using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Core.Model;

namespace WebAPI.Core.Service
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _Configuration;
        public AuthService(IConfiguration cnfiguration)
        {
            _Configuration = cnfiguration;
        }
        public async Task<string> GenerateToken(UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSettings = _Configuration.GetSection("JWT");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                            new Claim(ClaimTypes.Role, user.Role.ToString()),
                            // Add more claims as needed
                        },
                expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings["ExpiryMinutes"])),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return tokenHandler.WriteToken(token);

        }

        public Task<IActionResult> Login(LoginModel loginModel)
        {
            throw new NotImplementedException();
        }
    }
}
