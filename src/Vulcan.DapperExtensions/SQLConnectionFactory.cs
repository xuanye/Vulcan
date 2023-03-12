using System.Data;
using System.Data.SqlClient;
using Vulcan.DapperExtensions.Contract;
using Vulcan.DapperExtensions.ORMapping;

namespace Vulcan.DapperExtensions
{
    public class SqlConnectionFactory : IConnectionFactory
    {
        public ISqlBuilder SqlBuilder => ORMapping.Mssql.MssqlBuilder.Instance;

        public IDbConnection CreateDbConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }


    }
}
