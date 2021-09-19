using System;
using Vulcan.DapperExtensions;
using Vulcan.DapperExtensions.Contract;
using Vulcan.DapperExtensionsUnitTests.Internal;
using Xunit;

namespace Vulcan.DapperExtensionsUnitTests
{
    /// <summary>
    /// ConnectionManagerTests
    /// </summary>
    public class ConnectionManagerFactoryTests: IDisposable
    {
        private ConnectionManagerFactory _factory;
        private ThreadLocalStorage _threadLocalStorage;
        private IConnectionFactory _connectionFactory;


        public ConnectionManagerFactoryTests()
        {
            //for unit test only ,in asp.net core should use httpContext storage ,
            //or other asynchronous application maybe implement by AsyncLocal<>
            _threadLocalStorage = new ThreadLocalStorage();

            _connectionFactory = TestResourceManager.GetConnectionFactory();

            _factory = new ConnectionManagerFactory(_threadLocalStorage, _connectionFactory);
        }

        [Fact]
        public void Constructor_ShouldThrowException_IfPassNull()
        {
            Assert.Throws<ArgumentNullException>(() => { var _ = new ConnectionManagerFactory(_threadLocalStorage, null); });
            Assert.Throws<ArgumentNullException>(() => { var _ = new ConnectionManagerFactory(null, _connectionFactory); });
            Assert.Throws<ArgumentNullException>(() => { var _ = new ConnectionManagerFactory(null, null); });
        }

        [Fact]
        public void GetConnectionManager_IsNotNull_WithSimpleCall()
        {
            //arrange


            //act
            using var connectionManager = _factory.GetConnectionManager(TestResourceManager.GetConnectionString());


            //assert
            Assert.NotNull(connectionManager);
            Assert.NotNull(connectionManager.Connection);

            Assert.Equal(TestResourceManager.GetConnectionString(), connectionManager.Connection.ConnectionString);

            Assert.Equal(1, connectionManager.RefCount);

            Assert.Equal(System.Data.ConnectionState.Open, connectionManager.Connection.State);

        }

        [Fact]
        public void GetConnectionManager_CheckRefCountEqualToTwo_GetTowice()
        {
            //arrange


            //act
            using var connectionManager = _factory.GetConnectionManager(TestResourceManager.GetConnectionString());


            //assert
            Assert.NotNull(connectionManager);
            Assert.NotNull(connectionManager.Connection);

            Assert.Equal(TestResourceManager.GetConnectionString(), connectionManager.Connection.ConnectionString);

            Assert.Equal(1, connectionManager.RefCount);

            Assert.Equal(System.Data.ConnectionState.Open, connectionManager.Connection.State);

            using var connectionManager2 = _factory.GetConnectionManager(TestResourceManager.GetConnectionString());

            Assert.NotNull(connectionManager2);
            Assert.NotNull(connectionManager2.Connection);

            Assert.Equal(connectionManager, connectionManager2);

            Assert.Equal(2, connectionManager.RefCount);

        }

        [Fact]
        public void GetConnectionManager_CheckRefCount0_AfterDispose()
        {
            //arrange


            //act
            var connectionManager = _factory.GetConnectionManager(TestResourceManager.GetConnectionString());


            //assert
            Assert.NotNull(connectionManager);
            Assert.NotNull(connectionManager.Connection);

            Assert.Equal(TestResourceManager.GetConnectionString(), connectionManager.Connection.ConnectionString);

            Assert.Equal(1, connectionManager.RefCount);

            Assert.Equal(System.Data.ConnectionState.Open, connectionManager.Connection.State);

            connectionManager.Dispose();

            Assert.Equal(0, connectionManager.RefCount);
            Assert.Equal(System.Data.ConnectionState.Closed, connectionManager.Connection.State);

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
