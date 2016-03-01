using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Vulcan.DataAccess;

namespace Vulcan.Test.MockObject
{
    public class MySqlConnectionFactory : ConnectionFactory
    {
        protected override IDbConnection CreateDefaultDbConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }
    }
}
