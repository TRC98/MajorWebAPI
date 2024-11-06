using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPI.Core;
using WebAPI.Core.Model;
using WebAPI.Core.Service;

namespace MajorWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ILoginService _loginService;
        private readonly IAuthService _authService;
        public LoginController(ILogger<LoginController> logger, ILoginService loginService,IAuthService authService)
        {
            _logger = logger;
            _loginService = loginService;
            _authService = authService;
        }
        
        [HttpPost]
        [Route("UserLogin")]
        public async Task<WebAPICommonResponse> UserLogin([FromBody] LoginModel loginModel)
        {
            //_logger.LogError("{0} || hello error {1}",LoggFilesInformation.LogginController,$"{userName}-{password}");
            //var result = new WebAPICommonResponse
            //{
            //    StatusCode = 200,
            //    Body = ""

            //};
            //return Ok(result);
            var result = await _loginService.UserLogin(loginModel);
            return result;

        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenRequest request)
        {
            var principal = await _authService.GetPrincipalFromExpiredToken(request.AccessToken);
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

         //   var newAccessToken = await _authService.GenerateToken(user);
            var newRefreshToken = await _authService.GenerateRefreshToken();

            var responce = new AuthResponse
            {
                UserId = user.UserId.ToString(),
          //      Token = newAccessToken,
                RefreshToken = newRefreshToken
            };
            //_userService.UpdateUser(user);

            return Ok(responce);
        }

    }
}
