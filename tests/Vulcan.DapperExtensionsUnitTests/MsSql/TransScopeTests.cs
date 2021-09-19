using System;
using System.Data;
using Vulcan.DapperExtensions;
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

            //act
            using var scope = new TransScope(_factory, MSSQLConstants.CONNECTION_STRING); //ref +1
            using var connect1 = _factory.GetConnectionManager(MSSQLConstants.CONNECTION_STRING);

            using var connect2 = _factory.GetConnectionManager(MSSQLConstants.CONNECTION_STRING);
            //assert
            Assert.NotNull(connect1.Transaction);

            Assert.Equal(connect1,connect2);
            Assert.Equal(2, connect1.RefCount);
        }

        [Fact]
        public void Create_ShouldBeOk()
        {
            //arrange

            //act
            using var scope = new TransScope(_factory, MSSQLConstants.CONNECTION_STRING); //ref +1
            using var connect1 = _factory.GetConnectionManager(MSSQLConstants.CONNECTION_STRING);

            using var connect2 = _factory.GetConnectionManager(MSSQLConstants.CONNECTION_STRING);
            //assert
            Assert.NotNull(connect1.Transaction);

            Assert.Equal(connect1,connect2);
            Assert.Equal(2, connect1.RefCount);
        }




        public void Dispose()
        {
            _threadLocalStorage.Remove(MSSQLConstants.CONNECTION_STRING);
            _threadLocalStorage = null;
            _factory = null;
            _connectionFactory = null;
        }
    }
}
