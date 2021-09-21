using Vulcan.DapperExtensionsUnitTests.Internal;
using Xunit;

namespace Vulcan.DapperExtensionsUnitTests
{
    [Collection("Database collection")]
    public class ConnectionManagerTests
    {
        public ConnectionManagerTests(SharedDatabaseFixture fixture)
        {
            SharedDatabaseFixture = fixture;
        }

        public SharedDatabaseFixture SharedDatabaseFixture { get; }

        [Fact]
        public void BeginTransaction_ShouldBeOk()
        {
            //arrange
            using var connectionManager =
                SharedDatabaseFixture.ConnectionManagerFactory.GetConnectionManager(TestResourceManager.GetConnectionString());

            //act
            var trans = connectionManager.BeginTransaction();

            //assert
            Assert.NotNull(trans);
            Assert.True(connectionManager.IsExistDbTransaction());
        }
    }
}
