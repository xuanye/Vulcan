using System.Data;
using Vulcan.DapperExtensions;
using Vulcan.DapperExtensionsUnitTests.Internal;
using Xunit;

namespace Vulcan.DapperExtensionsUnitTests.MSSQL
{
    /// <summary>
    ///     share same connection in a scope
    /// </summary>
    public class ConnectionScopeTests : SharedDatabaseTest
    {
        public ConnectionScopeTests(SharedDatabaseFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void TestConnectionScope_IsSameConnection_WithUsingScope()
        {
            //arrange
            var connectionString = TestResourceManager.GetConnectionString();
            //act

            using var scope = new ConnectionScope(SharedDatabaseFixture.ConnectionManagerFactory, connectionString);
            //ref +1
            IDbConnection connection;
            using (var connect1 = SharedDatabaseFixture.ConnectionManagerFactory.GetConnectionManager(connectionString)) //ref +1
            {
                connection = connect1.Connection;
                //assert
                Assert.Equal(2, connect1.RefCount);
            } //ref -1

            using (var connect2 = SharedDatabaseFixture.ConnectionManagerFactory.GetConnectionManager(connectionString)) //ref +1
            {
                //assert
                Assert.Equal(connection, connect2.Connection);
                Assert.Equal(2, connect2.RefCount);
            } //ref -1

            //do nothing
            scope.Commit();
            scope.Rollback();
        }


        [Fact]
        public void TestConnectionScope_IsNotSameConnection_WithoutUsingScope()
        {
            //arrange
            var connectionString = TestResourceManager.GetConnectionString();
            IDbConnection connection;
            //act

            using (var connect1 = SharedDatabaseFixture.ConnectionManagerFactory.GetConnectionManager(connectionString))
            {
                connection = connect1.Connection;
                //assert
                Assert.Equal(1, connect1.RefCount);
            }

            using (var connect2 = SharedDatabaseFixture.ConnectionManagerFactory.GetConnectionManager(connectionString))
            {
                //assert
                Assert.NotEqual(connection, connect2.Connection);
                Assert.Equal(1, connect2.RefCount);
            }
        }
    }
}
