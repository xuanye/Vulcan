using System.Data;
using MySql.Data.MySqlClient;
using Vulcan.DapperExtensions.Contract;
using Vulcan.DapperExtensions.ORMapping;

namespace Vulcan.DapperExtensionsUnitTests.MySQL
{
    public class MySQLConnectionFactory : IConnectionFactory
    {
        public ISQLBuilder SQLBuilder => DapperExtensions.ORMapping.MySQL.MySQLSQLBuilder.Instance;

        public IDbConnection CreateDbConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }
    }
}
