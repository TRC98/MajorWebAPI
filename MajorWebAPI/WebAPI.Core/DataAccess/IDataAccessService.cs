using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Core.Model;

namespace WebAPI.Core.DataAccess
{
    public interface IDataAccessService
    {
        Task<IEnumerable<UserModel>> GetUsersAsync();
        Task<bool> AddUserAsync(UserModel user);
    }
}
