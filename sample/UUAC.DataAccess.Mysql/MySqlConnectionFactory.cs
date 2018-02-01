using System.Data;
using MySql.Data.MySqlClient;
using Vulcan.DataAccess;

namespace UUAC.DataAccess.Mysql
{
    public class MySqlConnectionFactory: IConnectionFactory
    {
        public IDbConnection CreateDbConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }
    }
}
