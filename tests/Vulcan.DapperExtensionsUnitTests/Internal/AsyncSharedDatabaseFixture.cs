using System;
using System.Threading;
using System.Threading.Tasks;
using Vulcan.DapperExtensions;
using Vulcan.DapperExtensions.Contract;
using Vulcan.DapperExtensionsUnitTests.MSSql;
using Vulcan.DapperExtensionsUnitTests.MySql;

namespace Vulcan.DapperExtensionsUnitTests.Internal
{
    public class AsyncSharedDatabaseFixture : IDisposable
    {
        private static readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        private static bool _databaseInitialized;


        private AsyncLocalStorage _localStorage;


        public AsyncSharedDatabaseFixture()
        {
            //for unit test only ,in asp.net core should use httpContext storage ,
            //or other asynchronous application maybe implement by AsyncLocal<>
            _localStorage = new AsyncLocalStorage();

            ConnectionString = TestResourceManager.GetConnectionString();
            ConnectionFactory = TestResourceManager.GetConnectionFactory();
            ConnectionManagerFactory = new ConnectionManagerFactory(_localStorage, ConnectionFactory);

            Repository = TestDataBaseSwitcher.DataBaseType switch
            {
                DataBaseType.MySql => new MySqlUnitTestRepository(ConnectionManagerFactory, ConnectionString,
                    ConnectionFactory),
                DataBaseType.MSSql => new MSSqlUnitTestRepository(ConnectionManagerFactory, ConnectionString,
                    ConnectionFactory),
                _ => throw new NotSupportedException()
            };

            //user_id -> UserId
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            SeedAsync().GetAwaiter().GetResult();
        }

        public IConnectionFactory ConnectionFactory { get; private set; }

        public string ConnectionString { get; private set; }

        public ConnectionManagerFactory ConnectionManagerFactory { get; private set; }


        public TestRepository Repository { get; }


        public void Dispose()
        {
            _localStorage = null;
            ConnectionManagerFactory = null;
            ConnectionFactory = null;
            ConnectionString = null;
        }

        public async Task SeedAsync()
        {
            AsyncLocalStorage.LocalValue = new System.Collections.Generic.Dictionary<string, object>();
            await semaphoreSlim.WaitAsync();
            try
            {
                if (_databaseInitialized) return;
                //Initial database
                await Repository.InitialTestDbAsync();
                _databaseInitialized = true;
            }
            finally
            {
                AsyncLocalStorage.LocalValue = null;
                //When the task is ready, release the semaphore. It is vital to ALWAYS release the semaphore when we are ready, or else we will end up with a Semaphore that is forever locked.
                //This is why it is important to do the Release within a try...finally clause; program execution may crash or take a different path, this way you are guaranteed execution
                semaphoreSlim.Release();
            }
        }
    }
}
