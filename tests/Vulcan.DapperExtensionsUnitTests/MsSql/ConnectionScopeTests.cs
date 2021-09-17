using System;
using System.Data;
using Vulcan.DapperExtensions;
using Vulcan.DapperExtensionsUnitTests.MsSql;
using Xunit;

namespace Vulcan.DapperExtensionsUnitTests
{
    /// <summary>
    /// share same connection in a scope
    /// </summary>
    public class ConnectionScopeTests:IDisposable
    {

        private ConnectionManagerFactory _factory;
        private ThreadLocalStorage _threadLocalStorage;
        private SqlConnectionFactory _connectionFactory;


        public ConnectionScopeTests()
        {
            //for unit test only ,in asp.net core should use httpContext storage ,
            //or other asynchronous application maybe implement by AsyncLocal<>
            _threadLocalStorage = new ThreadLocalStorage();

            _connectionFactory = new SqlConnectionFactory();

            _factory = new ConnectionManagerFactory(_threadLocalStorage, _connectionFactory);
        }

        [Fact]
        public void TestConnectionScope_IsSameConnection_WithUsingScope()
        {
            using var scope = new ConnectionScope(_factory, MSSQLConstants.CONNECTION_STRING);
            //ref +1
            IDbConnection connection;
            using(var connect1 = _factory.GetConnectionManager(MSSQLConstants.CONNECTION_STRING))//ref +1
            {
                connection = connect1.Connection;
                Assert.Equal(2, connect1.RefCount);
            }//ref -1

            using (var connect2 = _factory.GetConnectionManager(MSSQLConstants.CONNECTION_STRING))//ref +1
            {
                Assert.Equal(connection, connect2.Connection);
                Assert.Equal(2, connect2.RefCount);
            }//ref -1
        }


        [Fact]
        public void TestConnectionScope_IsNotSameConnection_WithoutUsingScope()
        {

                IDbConnection connection;
                using (var connect1 = _factory.GetConnectionManager(MSSQLConstants.CONNECTION_STRING))
                {
                    connection = connect1.Connection;
                    Assert.Equal(1, connect1.RefCount);
                }

                using (var connect2 = _factory.GetConnectionManager(MSSQLConstants.CONNECTION_STRING))
                {
                    Assert.NotEqual(connection, connect2.Connection);
                    Assert.Equal(1, connect2.RefCount);
                }

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
