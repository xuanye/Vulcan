using System.Runtime.InteropServices;

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
        public static DataBaseType DataBaseType = DataBaseType.MySql;
#else
        public static DataBaseType DataBaseType = DataBaseType.MSSql;
#endif

    }

    internal enum DataBaseType
    {
        MSSql = 1,
        MySql = 2
    }
}
