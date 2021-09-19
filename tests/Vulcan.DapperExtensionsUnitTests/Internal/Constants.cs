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
        public const string MYSQL_CONNECTION_STRING = @"server=192.168.102.108;port=3306;database=testdb;uid=develop;pwd=K@^23O12NAGb;charset=utf8mb4;Connection Timeout=18000;SslMode=none;";
    }
}
