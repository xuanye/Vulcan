using Vulcan.DapperExtensions;
using Vulcan.DapperExtensionsUnitTests.Internal;
using Xunit;

namespace Vulcan.DapperExtensionsUnitTests.MSSQL
{

    /// <summary>
    /// test scope transaction
    /// </summary>
    public class TransScopeTests : SharedDatabaseTest
    {   

        public TransScopeTests(SharedDatabaseFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void CreateTransScope_ShouldBeOk()
        {
            //arrange
            var connectionString = Fixture.ConnectionString;
            //act
            using var scope = new TransScope(Fixture.ConnectionManagerFactory, connectionString); //ref +1
            using var connect1 = Fixture.ConnectionManagerFactory.GetConnectionManager(connectionString);
            using var connect2 = Fixture.ConnectionManagerFactory.GetConnectionManager(connectionString);
            //assert
            Assert.NotNull(connect1.Transaction);

            Assert.Equal(connect1,connect2);
            Assert.Equal(connect1.Transaction,connect2.Transaction);
            Assert.Equal(2, connect1.RefCount);
        }

        [Fact]
        public void CreateTransScope_ShouldBeDifferentTransaction_WithNestedScope()
        {
            //arrange
            var connectionString = Fixture.ConnectionString;

            //act
            using var scope1 = new TransScope(Fixture.ConnectionManagerFactory, connectionString); //ref +1
            using var connect1 = Fixture.ConnectionManagerFactory.GetConnectionManager(connectionString);
            using var scope2 = new TransScope(Fixture.ConnectionManagerFactory, connectionString, TransScopeOption.RequiresNew); //ref +1
            using var connect2 = Fixture.ConnectionManagerFactory.GetConnectionManager(connectionString);

            //assert
            Assert.NotNull(connect1.Transaction);
            Assert.NotNull(connect2.Transaction);

            Assert.Equal(connect1,connect2);
            Assert.NotEqual(connect1.Transaction,connect2.Transaction);
            Assert.Equal(2, connect1.RefCount);
        }


        [Fact]
        public void CreateTransScope_DataExists_CommitScope()
        {
            //arrange

            //act

            //assert


        }


        [Fact]
        public void CreateTransScope_DataExists_RollbackScope()
        {
            //arrange

            //act

            //assert
        }      
    }
}
