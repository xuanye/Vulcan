namespace Vulcan.DapperExtensionsUnitTests.Internal
{
    internal static class TestDataBaseSwitcher
    {
        /// <summary>
        ///     Linux/Mac prefers to use MySql as a test database，
        ///     Windows use SqlSERVER
        ///     Maybe use Compiler Switch？
        /// </summary>

#if MySqlDebug
        public static DataBaseType DataBaseType = DataBaseType.Mysql;
#else
        public static DataBaseType DataBaseType = DataBaseType.Mssql;
#endif

    }

    internal enum DataBaseType
    {
        Mssql = 1,
        Mysql = 2
    }
}
