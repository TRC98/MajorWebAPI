using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Core.DataAccess
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetAsync(string id);
        IEnumerable<TEntity> GetEntitiesBySP(string storedProcedureName, Dictionary<string, Tuple<string, DbType, ParameterDirection>> parameters);
        Task<IEnumerable<TEntity>> GetUsersAsync();
    }
}
