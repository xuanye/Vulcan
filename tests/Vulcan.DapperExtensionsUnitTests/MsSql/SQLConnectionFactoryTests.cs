using System.Data.SqlClient;
using Vulcan.DapperExtensions;
using Vulcan.DapperExtensionsUnitTests.Internal;
using Xunit;

namespace Vulcan.DapperExtensionsUnitTests.MSSql
{
    public class SqlConnectionFactoryTests
    {
        public void CreateDbConnection_ShouldBeOK()
        {
            //arrange

            //act
            var factory = new SqlConnectionFactory();
            var connection = factory.CreateDbConnection(Constants.MSSql_CONNECTION_STRING);

            //act
            Assert.NotNull(connection);
            Assert.IsAssignableFrom<SqlConnection>(connection);
        }
    }
}
