using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPI.Core.Model;
using WebAPI.Core.Service;

namespace MajorWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IAuthService _authservice;
        public LoginController(ILogger<LoginController> logger, IAuthService authService)
        {
            _logger = logger;
            _authservice = authService;
        }
        
        [HttpGet]
        [Route("UserLogin")]
        public async Task<IActionResult> UserLogin([FromBody] LoginModel loginModel)
        {
            //_logger.LogError("{0} || hello error {1}",LoggFilesInformation.LogginController,$"{userName}-{password}");
            //var result = new WebAPICommonResponse
            //{
            //    StatusCode = 200,
            //    Body = ""

            //};
            //return Ok(result);

            UserModel user = new UserModel()
            {
                UserId = 1,
                UserName = "aaa",
                Role = "User",
                Password = "1111"
            };

            if (user.UserName != loginModel.UserName)
            {
                return Unauthorized("Invalid credentials 1");
            }

            //var saltedPassword = loginModel.Password + user.Salt;

            //var result = _passwordHasher.VerifyHashedPassword(user, user.Password, saltedPassword);

            if (loginModel.Password != user.Password)
            {
                return Unauthorized("Invalid credentials 2");
            }

            // Generate token
            var token = await _authservice.GenerateToken(user);
            var newRefreshToken = await _authservice.GenerateRefreshToken();

            // Return the token
            var responce = new AuthResponse
            {
                UserId = user.UserId.ToString(),
                Token = token,
                RefreshToken = newRefreshToken
            };
            return Ok(responce);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenRequest request)
        {
            var principal = await _authservice.GetPrincipalFromExpiredToken(request.AccessToken);
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = new UserModel
            {
                UserId=1,
                UserName="aaa",
                Password="1111",
                Role="User",
                RefreshToken = "s",
                RefreshTokenExpiryTime = DateTime.Now
            };
            //var user = await _authservice.GetUserById(userId);
            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return Unauthorized();
            }

            var newAccessToken = await _authservice.GenerateToken(user);
            var newRefreshToken = await _authservice.GenerateRefreshToken();

            var responce = new AuthResponse
            {
                UserId = user.UserId.ToString(),
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            };
            //_userService.UpdateUser(user);

            return Ok(responce);
        }

    }
}
