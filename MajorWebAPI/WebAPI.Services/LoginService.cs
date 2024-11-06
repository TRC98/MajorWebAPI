using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Core;
using WebAPI.Core.DataAccess;
using WebAPI.Core.EntitiManagmentService;
using WebAPI.Core.Entity;
using WebAPI.Core.Model;
using WebAPI.Core.Service;

namespace WebAPI.Services
{
    public class LoginService : ILoginService
    {
        private readonly IWebApiResponceService _apiresponce;
        private readonly IDataAccessLayer _dataAccessLayer;
        private readonly IAuthService _authService;
        private readonly IUserManagementService _userManagementService;
        public LoginService(IDataAccessLayer dataAccessLayer,IWebApiResponceService webApiResponceService, IAuthService authService, IUserManagementService userManagementService)
        {
            _apiresponce = webApiResponceService;
            _dataAccessLayer = dataAccessLayer;
            _authService = authService;
            _userManagementService = userManagementService;
        }
        public async Task<WebAPICommonResponse> UserLogin(LoginModel loginModel)
        {
            try
            {

                User user = await _userManagementService.GetUserbyUaerName(loginModel.UserName);

                if (user.UserName != loginModel.UserName)
                {
                    return await _apiresponce.GenerateResponseMessage((int)WebResponseCode.Unathorized, "Invalid credentials", null);
                    //return Unauthorized("Invalid credentials 1");
                }

                //var saltedPassword = loginModel.Password + user.Salt;

                //var result = _passwordHasher.VerifyHashedPassword(user, user.Password, saltedPassword);

                if (loginModel.Password != user.Password)
                {
                    return await _apiresponce.GenerateResponseMessage((int)WebResponseCode.Unathorized, "Invalid credentials", null);
                }

                // Generate token
                var token = await _authService.GenerateToken(user);
                var newRefreshToken = await _authService.GenerateRefreshToken();

                // Return the token
                var responce = new AuthResponse
                {
                    UserId = user.Id.ToString(),
                    Token = token,
                    RefreshToken = newRefreshToken
                };
                return await _apiresponce.GenerateResponseMessage((int)WebResponseCode.Success, "Successfully Loged", responce);
            }
            catch (Exception ex)
            {
                return await _apiresponce.GenerateResponseMessage((int)WebResponseCode.Exception, "Internal Error", null);
                throw ex;
            }
        }
    }
}
