using System.Data;
using System.Data.SqlClient;
using Vulcan.DapperExtensions.Contract;
using Vulcan.DapperExtensions.ORMapping;

namespace Vulcan.DapperExtensions
{
    public class SQLConnectionFactory : IConnectionFactory
    {
        public ISQLBuilder SQLBuilder => ORMapping.MSSQL.MSSQLSQLBuilder.Instance;

        public IDbConnection CreateDbConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        
    }
}
