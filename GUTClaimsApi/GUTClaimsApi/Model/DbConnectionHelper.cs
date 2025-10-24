using Microsoft.Data.SqlClient;

namespace GUTClaimsApi.Models
{
    public class DbConnectionHelper
    {
        private readonly IConfiguration _config;
        public DbConnectionHelper(IConfiguration config) => _config = config;

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        }
    }
}