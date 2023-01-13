using Vulcan.DapperExtensions;
using Vulcan.DapperExtensionsUnitTests.Internal;
using Xunit;

namespace Vulcan.DapperExtensionsUnitTests
{
    /// <summary>
    ///     test scope transaction
    /// </summary>
    [Collection("Database collection")]
    public class TransScopeTests 
    {
        public TransScopeTests(SharedDatabaseFixture fixture) 
        {
            SharedDatabaseFixture = fixture;
        }

        public SharedDatabaseFixture SharedDatabaseFixture { get; }

        [Fact]
        public void CreateTransScope_ShouldBeOk()
        {
            //arrange
            var connectionString = SharedDatabaseFixture.ConnectionString;
            //act
            using var scope = new TransScope(SharedDatabaseFixture.ConnectionManagerFactory, connectionString); //ref +1
            using var connect1 = SharedDatabaseFixture.ConnectionManagerFactory.GetConnectionManager(connectionString);
            using var connect2 = SharedDatabaseFixture.ConnectionManagerFactory.GetConnectionManager(connectionString);
            //assert
            Assert.NotNull(connect1.Transaction);

            Assert.Equal(connect1, connect2);
            Assert.Equal(connect1.Transaction, connect2.Transaction);
            Assert.Equal(3, connect1.RefCount);
        }

        [Fact]
        public void CreateTransScope_ShouldBeSameTransaction_WithNestedScope()
        {
            //arrange
            var connectionString = SharedDatabaseFixture.ConnectionString;

            //act

            using var scope1 = new TransScope(SharedDatabaseFixture.ConnectionManagerFactory, connectionString); //ref +1
            using var connect1 = SharedDatabaseFixture.ConnectionManagerFactory.GetConnectionManager(connectionString); //ref +1
            using var scope2 = new TransScope(SharedDatabaseFixture.ConnectionManagerFactory, connectionString); //ref +1
            using var connect2 = SharedDatabaseFixture.ConnectionManagerFactory.GetConnectionManager(connectionString); //ref +1


            //assert
            Assert.NotNull(connect1.Transaction);
            Assert.NotNull(connect2.Transaction);

            Assert.Equal(connect1, connect2);
            Assert.Equal(connect1.Transaction, connect2.Transaction);
            Assert.Equal(4, connect1.RefCount);
        }


    }
}
