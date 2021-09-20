using System.Threading.Tasks;
using AutoFixture;
using Vulcan.DapperExtensions;
using Vulcan.DapperExtensionsUnitTests.Internal;
using Xunit;

namespace Vulcan.DapperExtensionsUnitTests.MSSQL
{
    /// <summary>
    ///     test scope transaction
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


        [Fact]
        public async Task CreateTransScope_ShouldBeSuccess_Commit()
        {
            //arrange
            var repository = SharedDatabaseFixture.Repository ;
            long newId;
            var testItem = AutoFixture.Create<TestItem>();
            //act

            using (var scope = repository.CreateScope())
            {

                newId = await repository.InsertAsync(testItem);
                scope.Commit();
            }
            var dbItem= await repository.GetTestItemAsync((int)newId);
            //assert
            Assert.True(newId>0);
            Assert.NotNull(dbItem);

            Assert.Equal(testItem.Name,dbItem.Name);
            Assert.Equal(testItem.Address,dbItem.Address);

        }


        [Fact]
        public async Task CreateTransScope_ShouldBeOk_Rollback()
        {
            //arrange
            long newId;
            var repository = SharedDatabaseFixture.Repository ;
            var testItem = AutoFixture.Create<TestItem>();
            //act


            using (repository.CreateScope())
            {

                newId = await repository.InsertAsync(testItem);
                //don't commit
                //scope.Commit();
            }
            var dbItem= await repository.GetTestItemAsync((int)newId);
            //assert
            Assert.True(newId>0);
            Assert.Null(dbItem);
        }
    }
}
