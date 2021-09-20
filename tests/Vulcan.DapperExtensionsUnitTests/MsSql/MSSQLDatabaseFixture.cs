using System;

namespace Vulcan.DapperExtensionsUnitTests.MSSQL
{
    public class MSSQLDatabaseFixture : IDisposable
    {
        public void Dispose()
        {
            // ... clean up test data from the database ...
        }

        //public SqlConnection Db { get; private set; }
    }
}
