using System;
using Vulcan.DapperExtensions;
using Vulcan.DapperExtensions.Contract;
using Vulcan.DapperExtensions.ORMapping;
using Vulcan.DapperExtensions.ORMapping.MySql;
using Vulcan.DapperExtensionsUnitTests.MSSql;
using Vulcan.DapperExtensionsUnitTests.MySql;

namespace Vulcan.DapperExtensionsUnitTests.Internal
{
    public static class TestResourceManager
    {
        public static string GetConnectionString()
        {
            return TestDataBaseSwitcher.DataBaseType switch
            {
                DataBaseType.MySql => Constants.MYSql_CONNECTION_STRING,
                DataBaseType.MSSql => Constants.MSSql_CONNECTION_STRING,
                _ => throw new NotImplementedException()
            };
        }

        public static IConnectionFactory GetConnectionFactory()
        {
            return TestDataBaseSwitcher.DataBaseType switch
            {
                DataBaseType.MySql => new MySqlConnectionFactory(),
                DataBaseType.MSSql => new SqlConnectionFactory(),
                _ => throw new NotImplementedException()
            };
        }
    }
}
