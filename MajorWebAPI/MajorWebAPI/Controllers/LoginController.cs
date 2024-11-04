using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

            // Return the token
            return Ok(new AuthResponse { Token = token });
        }

    }
}
