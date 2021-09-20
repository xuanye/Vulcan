using System.Data.SqlClient;
using Vulcan.DapperExtensions;
using Vulcan.DapperExtensionsUnitTests.Internal;
using Xunit;

namespace Vulcan.DapperExtensionsUnitTests.MSSQL
{
    public class SQLConnectionFactoryTests
    {
        public void CreateDbConnection_ShouldBeOK()
        {
            //arrange

            //act
            var factory = new SQLConnectionFactory();
            var connection = factory.CreateDbConnection(Constants.MSSQL_CONNECTION_STRING);

            //act
            Assert.NotNull(connection);
            Assert.IsAssignableFrom<SqlConnection>(connection);
        }
    }
}
