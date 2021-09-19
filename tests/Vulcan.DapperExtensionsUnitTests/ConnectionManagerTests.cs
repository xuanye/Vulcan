using System;
using Vulcan.DapperExtensions;
using Xunit;

namespace Vulcan.DapperExtensionsUnitTests.MSSQL
{
    public class ConnectionManagerTests : IDisposable
    {
        private ConnectionManagerFactory _factory;
        private ThreadLocalStorage _threadLocalStorage;
        private SQLConnectionFactory _connectionFactory;

        public ConnectionManagerTests()
        {
            //for unit test only ,in asp.net core should use httpContext storage ,
            //or other asynchronous application maybe implement by AsyncLocal<>
            _threadLocalStorage = new ThreadLocalStorage();

            _connectionFactory = new SQLConnectionFactory();

            _factory = new ConnectionManagerFactory(_threadLocalStorage, _connectionFactory);
        }

        [Fact]
        public void BeginTransaction_ShouldBeOk()
        {
            //arrange
            using var connectionManager = _factory.GetConnectionManager(MSSQLConstants.CONNECTION_STRING);

            //act
            var trans = connectionManager.BeginTransaction();

            //assert
            Assert.NotNull(trans);
            Assert.True(connectionManager.IsExistDbTransaction());


        }
        public void Dispose()
        {
            _threadLocalStorage.Remove(MSSQLConstants.CONNECTION_STRING);
            _threadLocalStorage = null;
            _factory = null;
            _connectionFactory = null;
            //throw new NotImplementedException();
        }
    }
}
