using System;
using System.Data;
using Vulcan.DapperExtensions;
using Vulcan.DapperExtensionsUnitTests.Internal;
using Xunit;

namespace Vulcan.DapperExtensionsUnitTests.MSSQL
{
    /// <summary>
    /// share same connection in a scope
    /// </summary>
    public class ConnectionScopeTests:IDisposable
    {

        private ConnectionManagerFactory _factory;
        private ThreadLocalStorage _threadLocalStorage;
        private SQLConnectionFactory _connectionFactory;


        public ConnectionScopeTests()
        {
            //for unit test only ,in asp.net core should use httpContext storage ,
            //or other asynchronous application maybe implement by AsyncLocal<>
            _threadLocalStorage = new ThreadLocalStorage();

            _connectionFactory = new SQLConnectionFactory();

            _factory = new ConnectionManagerFactory(_threadLocalStorage, _connectionFactory);
        }

        [Fact]
        public void TestConnectionScope_IsSameConnection_WithUsingScope()
        {
            using var scope = new ConnectionScope(_factory,TestResourceManager.GetConnectionString());
            //ref +1
            IDbConnection connection;
            using(var connect1 = _factory.GetConnectionManager(TestResourceManager.GetConnectionString()))//ref +1
            {
                connection = connect1.Connection;
                Assert.Equal(2, connect1.RefCount);
            }//ref -1

            using (var connect2 = _factory.GetConnectionManager(TestResourceManager.GetConnectionString()))//ref +1
            {
                Assert.Equal(connection, connect2.Connection);
                Assert.Equal(2, connect2.RefCount);
            }//ref -1

            //do nothing
            scope.Commit();
            scope.Rollback();
        }


        [Fact]
        public void TestConnectionScope_IsNotSameConnection_WithoutUsingScope()
        {

                IDbConnection connection;
                using (var connect1 = _factory.GetConnectionManager(TestResourceManager.GetConnectionString()))
                {
                    connection = connect1.Connection;
                    Assert.Equal(1, connect1.RefCount);
                }

                using (var connect2 = _factory.GetConnectionManager(TestResourceManager.GetConnectionString()))
                {
                    Assert.NotEqual(connection, connect2.Connection);
                    Assert.Equal(1, connect2.RefCount);
                }

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
