using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UUAC.Common;
using Vulcan.DataAccess;

namespace UUAC.DataAccess.Mysql.Repository
{
    public class BaseRepository:Vulcan.DataAccess.ORMapping.MySql.MySqlRepository
    {
        protected BaseRepository() : this(Constans.MAIN_DB_KEY)
        {
        }

        protected BaseRepository(string key) : base(ConnectionStringManager.GetConnectionString(key))
        {
        }

        protected BaseRepository(IConnectionFactory factory, string key) : base(factory, ConnectionStringManager.GetConnectionString(key))
        {
        }
    }
}
