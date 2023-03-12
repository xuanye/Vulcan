using System;
using Vulcan.DapperExtensions;
using Vulcan.DapperExtensions.Contract;
using Vulcan.DapperExtensionsUnitTests.MySql;

namespace Vulcan.DapperExtensionsUnitTests.Internal
{
    public static class TestResourceManager
    {
        public static string GetConnectionString()
        {
            return TestDataBaseSwitcher.DataBaseType switch
            {
                DataBaseType.Mysql => Constants.MYSql_CONNECTION_STRING,
                DataBaseType.Mssql => Constants.MSSql_CONNECTION_STRING,
                _ => throw new NotImplementedException()
            };
        }

        public static IConnectionFactory GetConnectionFactory()
        {
            return TestDataBaseSwitcher.DataBaseType switch
            {
                DataBaseType.Mysql => new MySqlConnectionFactory(),
                DataBaseType.Mssql => new SqlConnectionFactory(),
                _ => throw new NotImplementedException()
            };
        }
    }
}
