using MySql.Data.MySqlClient;
using System.Data;
using Vulcan.DapperExtensions.Contract;
using Vulcan.DapperExtensions.ORMapping;

namespace Vulcan.DapperExtensionsUnitTests.MySql
{
    public class MySqlConnectionFactory : IConnectionFactory
    {
        public ISqlBuilder SqlBuilder => DapperExtensions.ORMapping.Mysql.MysqlBuilder.Instance;

        public IDbConnection CreateDbConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }
    }
}
