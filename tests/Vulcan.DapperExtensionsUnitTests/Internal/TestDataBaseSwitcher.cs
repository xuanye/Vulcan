namespace Vulcan.DapperExtensionsUnitTests.Internal
{
    internal static class TestDataBaseSwitcher
    {
        public static DataBaseType DataBaseType = DataBaseType.MSSQL;
    }

    internal enum DataBaseType
    {
        MSSQL = 1,
        MySQL = 2
    }
}
