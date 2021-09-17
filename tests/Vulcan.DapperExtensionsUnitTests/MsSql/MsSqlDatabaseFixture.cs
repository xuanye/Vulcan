using System;
using System.Collections.Generic;
using System.Text;

namespace Vulcan.DapperExtensionsUnitTests.MSSQL
{
    public class MSSQLDatabaseFixture : IDisposable
    {
        public MSSQLDatabaseFixture()
        {
            //Db = new SqlConnection("MyConnectionString");

            // ... initialize data in the test database ...
        }

        public void Dispose()
        {
            // ... clean up test data from the database ...
        }

        //public SqlConnection Db { get; private set; }
    }
}
