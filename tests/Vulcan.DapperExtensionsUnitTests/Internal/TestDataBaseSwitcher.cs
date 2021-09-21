using System.Runtime.InteropServices;

namespace Vulcan.DapperExtensionsUnitTests.Internal
{
    internal static class TestDataBaseSwitcher
    {
        /// <summary>
        ///     Linux/Mac prefers to use MySQL as a test database，
        ///     Windows use SQLSERVER
        ///     Maybe use Compiler Switch？
        /// </summary>

#if MySQLDebug
        public static DataBaseType DataBaseType = DataBaseType.MySQL;
#else
        public static DataBaseType DataBaseType = DataBaseType.MSSQL;
#endif

    }

    internal enum DataBaseType
    {
        MSSQL = 1,
        MySQL = 2
    }
}
