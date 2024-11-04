using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Core.DataAccess;
using WebAPI.Core.Model;

namespace WebAPI.DataAccess
{
    public class DataAccessService : IDataAccessService
    {
        private readonly string _connectionstring;
        public DataAccessService(string connectionstring)
        {
            _connectionstring = connectionstring;
        }
        private IDbConnection DbConnection => new SqlConnection(_connectionstring);

        public async Task<bool> AddUserAsync(UserModel user)
        {
            try
            {
                using (var connection = DbConnection)
                {
                    long id = await connection.InsertAsync(user);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync()
        {
            try
            {
                using (var connection = DbConnection)
                {
                    string sql = "SELECT * FROM [dbo].[User]";
                    return await connection.QueryAsync<UserModel>(sql);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
