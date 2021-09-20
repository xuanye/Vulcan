using System;
using System.Threading;
using System.Threading.Tasks;
using Vulcan.DapperExtensions;
using Vulcan.DapperExtensions.Contract;

namespace Vulcan.DapperExtensionsUnitTests.Internal
{
    public class SharedDatabaseFixture: IDisposable
    {
        private static readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1,1);
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
                DataBaseType.MySQL => new MySQL.UnitTestRepository(ConnectionManagerFactory, ConnectionString,
                    ConnectionFactory),
                DataBaseType.MSSQL => new MSSQL.UnitTestRepository(ConnectionManagerFactory, ConnectionString,
                    ConnectionFactory),
                _ => throw new NotSupportedException()
            };

            SeedAsync().GetAwaiter().GetResult();
        }

        public IConnectionFactory ConnectionFactory { get; private set; }

        public  string ConnectionString { get; private set;}

        public  ConnectionManagerFactory ConnectionManagerFactory { get;private set; }


        public IRepository Repository { get; }

        private async Task SeedAsync()
        {
            await semaphoreSlim.WaitAsync();
            try
            {

                if (_databaseInitialized)
                {
                    return;
                }

                //Initial database
                await Repository.InitialTestDb();

                _databaseInitialized = true;
            }
            finally
            {
                //When the task is ready, release the semaphore. It is vital to ALWAYS release the semaphore when we are ready, or else we will end up with a Semaphore that is forever locked.
                //This is why it is important to do the Release within a try...finally clause; program execution may crash or take a different path, this way you are guaranteed execution
                semaphoreSlim.Release();
            }
        }


        public void Dispose()
        {
            _threadLocalStorage.Remove(ConnectionString);
            _threadLocalStorage = null;
            ConnectionManagerFactory = null;
            ConnectionFactory = null;
            ConnectionString = null;
        }
    }
}
