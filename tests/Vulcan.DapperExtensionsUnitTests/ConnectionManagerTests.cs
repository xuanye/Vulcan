using Vulcan.DapperExtensionsUnitTests.Internal;
using Xunit;

namespace Vulcan.DapperExtensionsUnitTests
{
    public class ConnectionManagerTests : SharedDatabaseTest
    {
        public ConnectionManagerTests(SharedDatabaseFixture fixture) : base(fixture)
        {
        }

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
