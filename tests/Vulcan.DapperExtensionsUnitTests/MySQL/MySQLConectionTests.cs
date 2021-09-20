using MySql.Data.MySqlClient;
using Vulcan.DapperExtensionsUnitTests.Internal;
using Xunit;

namespace Vulcan.DapperExtensionsUnitTests.MySQL
{
    public class MySQLConectionTests
    {
        [Fact]
        public void TestConnection()
        {
            var connection = new MySqlConnection(Constants.MYSQL_CONNECTION_STRING);
            connection.Open();
            Assert.NotNull(connection);
            connection.Close();
        }
    }
}
