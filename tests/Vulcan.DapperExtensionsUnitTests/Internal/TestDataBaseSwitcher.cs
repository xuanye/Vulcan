namespace Vulcan.DapperExtensionsUnitTests
{
    internal static class TestDataBaseSwitcher
    {
        public static DataBaseType DataBaseType = DataBaseType.MySQL;
    }

    internal enum DataBaseType
    {
        MSSQL = 1,
        MySQL = 2
    }
}
