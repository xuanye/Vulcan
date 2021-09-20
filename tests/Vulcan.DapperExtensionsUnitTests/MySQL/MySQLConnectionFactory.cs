using System.Data;
using MySql.Data.MySqlClient;
using Vulcan.DapperExtensions.Contract;

namespace Vulcan.DapperExtensionsUnitTests.MySQL
{
    public class MySQLConnectionFactory:IConnectionFactory
    {
        public IDbConnection CreateDbConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }
    }
}
