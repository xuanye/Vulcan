using System.Data;
using MySql.Data.MySqlClient;
using Vulcan.DataAccess;

namespace UUAC.WebApp.Libs
{
    public class MySqlConnectionFactory: IConnectionFactory
    {
        public IDbConnection CreateDbConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }
    }
}
