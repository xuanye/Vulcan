using System.Data;
using MySql.Data.MySqlClient;
using Vulcan.DapperExtensions.Contract;
using Vulcan.DapperExtensions.ORMapping;

namespace Vulcan.DapperExtensionsUnitTests.MySql
{
    public class MySqlConnectionFactory : IConnectionFactory
    {
        public ISqlBuilder SqlBuilder => DapperExtensions.ORMapping.MySql.MySqlBuilder.Instance;

        public IDbConnection CreateDbConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }
    }
}
