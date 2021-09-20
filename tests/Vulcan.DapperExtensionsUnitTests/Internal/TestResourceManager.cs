using System;
using Vulcan.DapperExtensions;
using Vulcan.DapperExtensions.Contract;
using Vulcan.DapperExtensions.ORMapping;
using Vulcan.DapperExtensions.ORMapping.MySQL;
using Vulcan.DapperExtensionsUnitTests.MSSQL;
using Vulcan.DapperExtensionsUnitTests.MySQL;

namespace Vulcan.DapperExtensionsUnitTests.Internal
{
    public static class TestResourceManager
    {
        public static string GetConnectionString()
        {
            return TestDataBaseSwitcher.DataBaseType switch
            {
                DataBaseType.MySQL => Constants.MYSQL_CONNECTION_STRING,
                DataBaseType.MSSQL => Constants.MSSQL_CONNECTION_STRING,
                _ => throw new NotImplementedException()
            };
        }

        public static IConnectionFactory GetConnectionFactory()
        {
            return TestDataBaseSwitcher.DataBaseType switch
            {
                DataBaseType.MySQL => new MySQLConnectionFactory(),
                DataBaseType.MSSQL => new SQLConnectionFactory(),
                _ => throw new NotImplementedException()
            };
        }
    }
}
