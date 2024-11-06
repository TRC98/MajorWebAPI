using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Core.Entity;

namespace WebAPI.Core.EntitiManagmentService
{
    public interface IUserManagementService
    {
        Task<User> GetUserbyUaerName(string name);
    }
}
