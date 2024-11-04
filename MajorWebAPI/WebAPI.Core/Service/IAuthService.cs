using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Core.Model;

namespace WebAPI.Core.Service
{
    public interface IAuthService
    {
        Task<IActionResult> Login(LoginModel loginModel);
        Task<string> GenerateToken(UserModel user);
    }
}
