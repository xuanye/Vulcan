using System;
using System.Data;
using Vulcan.DapperExtensions;
using Vulcan.DapperExtensions.Contract;
using Vulcan.DapperExtensionsUnitTests.Internal;
using Xunit;

namespace Vulcan.DapperExtensionsUnitTests
{
    /// <summary>
    ///     ConnectionManagerTests
    /// </summary>
    public class ConnectionManagerFactoryTests : IDisposable
    {
        private IConnectionFactory _connectionFactory;
        private ThreadLocalStorage _threadLocalStorage;


        public ConnectionManagerFactoryTests()
        {
            //for unit test only ,in asp.net core should use httpContext storage ,
            //or other asynchronous application maybe implement by AsyncLocal<>
            _threadLocalStorage = new ThreadLocalStorage();

            _connectionFactory = TestResourceManager.GetConnectionFactory();
        }


        public void Dispose()
        {
            _threadLocalStorage = null;
            _connectionFactory = null;
        }

        [Fact]
        public void Constructor_ShouldThrowException_IfPassNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = new ConnectionManagerFactory(_threadLocalStorage, null);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = new ConnectionManagerFactory(null, _connectionFactory);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = new ConnectionManagerFactory(null, null);
            });
        }

        [Fact]
        public void GetConnectionManager_IsNotNull_WithSimpleCall()
        {
            //arrange
            var factory = new ConnectionManagerFactory(_threadLocalStorage, _connectionFactory);
            var connectString = TestResourceManager.GetConnectionString();

            //act
            using var connectionManager = factory.GetConnectionManager(connectString);


            //assert
            Assert.NotNull(connectionManager);
            Assert.NotNull(connectionManager.Connection);

            Assert.Equal(1, connectionManager.RefCount);

            Assert.Equal(ConnectionState.Open, connectionManager.Connection.State);
        }

        [Fact]
        public void GetConnectionManager_CheckRefCountEqualToTwo_GetTwice()
        {
            //arrange
            var factory = new ConnectionManagerFactory(_threadLocalStorage, _connectionFactory);
            var connectString = TestResourceManager.GetConnectionString();

            //act
            using var connectionManager = factory.GetConnectionManager(connectString);


            //assert
            Assert.NotNull(connectionManager);
            Assert.NotNull(connectionManager.Connection);

            Assert.Equal(1, connectionManager.RefCount);

            Assert.Equal(ConnectionState.Open, connectionManager.Connection.State);

            using var connectionManager2 = factory.GetConnectionManager(connectString);

            Assert.NotNull(connectionManager2);
            Assert.NotNull(connectionManager2.Connection);

            Assert.Equal(connectionManager, connectionManager2);

            Assert.Equal(2, connectionManager.RefCount);
        }

        [Fact]
        public void GetConnectionManager_CheckRefCount0_AfterDispose()
        {
            //arrange
            var factory = new ConnectionManagerFactory(_threadLocalStorage, _connectionFactory);
            var connectString = TestResourceManager.GetConnectionString();

            //act
            var connectionManager = factory.GetConnectionManager(connectString);


            //assert
            Assert.NotNull(connectionManager);
            Assert.NotNull(connectionManager.Connection);

            Assert.Equal(1, connectionManager.RefCount);

            Assert.Equal(ConnectionState.Open, connectionManager.Connection.State);

            connectionManager.Dispose();

            Assert.Equal(0, connectionManager.RefCount);
            Assert.Equal(ConnectionState.Closed, connectionManager.Connection.State);
        }
    }
}
