using System;
using System.Data;
using Vulcan.DapperExtensions;
using Vulcan.DapperExtensionsUnitTests.Internal;
using Xunit;

namespace Vulcan.DapperExtensionsUnitTests.MSSQL
{

    /// <summary>
    /// test scope transaction
    /// </summary>
    public class TransScopeTests : IDisposable
    {

        private ConnectionManagerFactory _factory;
        private ThreadLocalStorage _threadLocalStorage;
        private SQLConnectionFactory _connectionFactory;


        public TransScopeTests()
        {
            //for unit test only ,in asp.net core should use httpContext storage ,
            //or other asynchronous application maybe implement by AsyncLocal<>
            _threadLocalStorage = new ThreadLocalStorage();

            _connectionFactory = new SQLConnectionFactory();

            _factory = new ConnectionManagerFactory(_threadLocalStorage, _connectionFactory);
        }

        [Fact]
        public void CreateTransScope_ShouldBeOk()
        {
            //arrange
            var connectionString = TestResourceManager.GetConnectionString();
            //act
            using var scope = new TransScope(_factory, connectionString); //ref +1
            using var connect1 = _factory.GetConnectionManager(connectionString);
            using var connect2 = _factory.GetConnectionManager(connectionString);
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
            var connectionString = TestResourceManager.GetConnectionString();

            //act
            using var scope1 = new TransScope(_factory, connectionString); //ref +1
            using var connect1 = _factory.GetConnectionManager(connectionString);
            using var scope2 = new TransScope(_factory, connectionString, TransScopeOption.RequiresNew); //ref +1
            using var connect2 = _factory.GetConnectionManager(connectionString);

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


        public void Dispose()
        {
            _threadLocalStorage.Remove(TestResourceManager.GetConnectionString());
            _threadLocalStorage = null;
            _factory = null;
            _connectionFactory = null;
        }
    }
}
