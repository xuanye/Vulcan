using Vulcan.DapperExtensions;
using Xunit;
using System.Data;
using System.Data.SqlClient;

namespace Vulcan.DapperExtensionsUnitTests.MSSQL
{
    public class SQLConnectionFactoryTests
    {
        public void CreateDbConnection_ShouldBeOK()
        {
            //arrange
            //act
            var factory = new SQLConnectionFactory();
            var connection = factory.CreateDbConnection(MSSQLConstants.CONNECTION_STRING);

            //act
            Assert.NotNull(connection);
            Assert.IsAssignableFrom<SqlConnection>(connection);

        }
    }
}
