namespace Vulcan.DapperExtensionsUnitTests.Internal
{
    public static class Constants
    {
        /// <summary>
        ///     MSSql Local TestDb
        ///     NOTE:TestDb Should be exists;
        /// </summary>
        public const string MSSql_CONNECTION_STRING =
            @"Server=(LocalDB)\MSSqlLocalDB;Initial Catalog=TestDb;Integrated Security=true;";

        /// <summary>
        ///     MySql
        ///     NOTE:TestDb Should be exists;
        /// </summary>
        //public const string MYSql_CONNECTION_STRING = @"Server=192.168.1.30;Port=3306;Database=northwind;Uid=develop;Pwd=M3YvLTd8iUni;";
        public const string MYSql_CONNECTION_STRING =
            "server=192.168.1.30;port=3306;database=testdb;uid=develop;pwd=M3YvLTd8iUni;charset=utf8mb4;Connection Timeout=18000;SslMode=none;";
    }
}
