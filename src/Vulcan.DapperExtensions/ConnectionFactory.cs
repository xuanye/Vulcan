using System;
using System.Data;
using System.Data.SqlClient;

namespace Vulcan.DapperExtensions
{
    public static class ConnectionFactoryHelper
    {
        private static IConnectionFactory _Default;
        public static void Configure(IConnectionFactory factory)
        {
            _Default = factory;
        }
        public static IDbConnection CreateDefaultDbConnection(string connectionString)
        {
            if(_Default != null)
            {
                return _Default.CreateDbConnection(connectionString);
            }

            throw new NullReferenceException("默认的IConnectionFactory没有设置，请在应用程序启动时设置");
        }
    }

    public interface IConnectionFactory
    {
        IDbConnection CreateDbConnection(string connectionString);
    }

    public class SqlConnectionFactory : IConnectionFactory
    {
        public IDbConnection CreateDbConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
