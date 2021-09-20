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
        public static DataBaseType DataBaseType = RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                                                  || RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
            ? DataBaseType.MySQL
            : DataBaseType.MySQL;
    }

    internal enum DataBaseType
    {
        MSSQL = 1,
        MySQL = 2
    }
}
