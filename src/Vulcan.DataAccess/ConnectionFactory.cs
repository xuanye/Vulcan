using System;
using System.Data;
using System.Data.SqlClient;

namespace Vulcan.DataAccess
{
    public abstract class ConnectionFactory
    {
        public static ConnectionFactory Default;

        public static IDbConnection CreateDbConnection(string connectionString)
        {
            if (Default != null)
            {
                return Default.CreateDefaultDbConnection(connectionString);
            }
            else
            {
                throw new ArgumentNullException("没有配置默认的DbConnectionFactory");
            }
        }

        protected abstract IDbConnection CreateDefaultDbConnection(string connectionString);
    }



    public class SqlConnectionFactory : ConnectionFactory
    {
        protected override IDbConnection CreateDefaultDbConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}