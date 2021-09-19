using System.Data;
using System.Data.SqlClient;
using Vulcan.DapperExtensions.Contract;

namespace Vulcan.DapperExtensions
{
    public class SQLConnectionFactory : IConnectionFactory
    {
        public IDbConnection CreateDbConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
