using System;
using System.Data;
using System.Data.SqlClient;

namespace Vulcan.DapperExtensions
{
   

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
