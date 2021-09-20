namespace Vulcan.DapperExtensionsUnitTests.Internal
{
    public static class Constants
    {
        /// <summary>
        /// MSSQL Local TestDb
        /// NOTE:TestDb Should be exists;
        /// </summary>
        public const string MSSQL_CONNECTION_STRING = @"Server=(LocalDB)\MSSQLLocalDB;Initial Catalog=TestDb;Integrated Security=true;";

        /// <summary>
        /// MySQL
        /// NOTE:TestDb Should be exists;
        /// </summary>
        //public const string MYSQL_CONNECTION_STRING = @"Server=192.168.1.30;Port=3306;Database=northwind;Uid=develop;Pwd=M3YvLTd8iUni;";

        public const string MYSQL_CONNECTION_STRING = "Server=192.168.1.30;Port=3306;Database=testdb;Uid=develop;Pwd=M3YvLTd8iUni;charset=utf8mb4;Connection Timeout=18000;SslMode=none;";
    }
}
