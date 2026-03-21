namespace Vulcan.DapperExtensionsUnitTests.Internal
{
    public static class Constants
    {
        /// <summary>
        /// MSSQL 连接字符串
        /// 优先使用环境变量 CI_MSSQL_CONNECTION_STRING
        /// </summary>
        public static string MSSQL_CONNECTION_STRING =
            System.Environment.GetEnvironmentVariable("CI_MSSQL_CONNECTION_STRING")
            ?? @"Server=192.168.2.9,1433;Database=TestDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;Encrypt=False;";

        /// <summary>
        /// MySQL 连接字符串
        /// 优先使用环境变量 CI_MYSQL_CONNECTION_STRING
        /// </summary>
        public static string MYSQL_CONNECTION_STRING =
            System.Environment.GetEnvironmentVariable("CI_MYSQL_CONNECTION_STRING")
            ?? "server=192.168.2.9;port=3306;database=testdb;uid=develop;pwd=M3YvLTd8iUni;charset=utf8;Connection Timeout=30;SslMode=None;AllowPublicKeyRetrieval=True;";
    }
}
