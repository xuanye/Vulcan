using Vulcan.DapperExtensions;
using Xunit;
using System.Data;
using System.Data.SqlClient;
using Vulcan.DapperExtensionsUnitTests.Internal;

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
