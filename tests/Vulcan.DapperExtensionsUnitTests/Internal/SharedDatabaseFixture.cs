using System;
using System.Threading;
using System.Threading.Tasks;
using Vulcan.DapperExtensions;
using Vulcan.DapperExtensions.Contract;
using Vulcan.DapperExtensionsUnitTests.MSSQL;
using Vulcan.DapperExtensionsUnitTests.MySQL;

namespace Vulcan.DapperExtensionsUnitTests.Internal
{
    public class SharedDatabaseFixture : IDisposable
    {
        private static readonly object LockObject = new object();
        private static bool _databaseInitialized;


        private ThreadLocalStorage _threadLocalStorage;


        public SharedDatabaseFixture()
        {
            //for unit test only ,in asp.net core should use httpContext storage ,
            //or other asynchronous application maybe implement by AsyncLocal<>
            _threadLocalStorage = new ThreadLocalStorage();


            ConnectionString = TestResourceManager.GetConnectionString();
            ConnectionFactory = TestResourceManager.GetConnectionFactory();
            ConnectionManagerFactory = new ConnectionManagerFactory(_threadLocalStorage, ConnectionFactory);

            Repository = TestDataBaseSwitcher.DataBaseType switch
            {
                DataBaseType.MySQL => new MySQLUnitTestRepository(ConnectionManagerFactory, ConnectionString,
                    ConnectionFactory),
                DataBaseType.MSSQL => new MSSQLUnitTestRepository(ConnectionManagerFactory, ConnectionString,
                    ConnectionFactory),
                _ => throw new NotSupportedException()
            };

            //user_id -> UserId
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            Seed();
        }

        public IConnectionFactory ConnectionFactory { get; private set; }

        public string ConnectionString { get; private set; }

        public ConnectionManagerFactory ConnectionManagerFactory { get; private set; }


        public TestRepository Repository { get; }


        public void Dispose()
        {
            _threadLocalStorage.Remove(ConnectionString);
            _threadLocalStorage = null;
            ConnectionManagerFactory = null;
            ConnectionFactory = null;
            ConnectionString = null;
        }

        private void Seed()
        {
            lock (LockObject)
            {

                if (_databaseInitialized) return;
                //Initial database
                Repository.InitialTestDb();
                _databaseInitialized = true;
            }
        }

        /// <summary>
        /// Reset test data - clears and reinitializes the test_item table.
        /// Call this before tests that require a clean database state.
        /// </summary>
        public void ResetTestData()
        {
            lock (LockObject)
            {
                Repository.InitialTestDb();
            }
        }
    }
}
