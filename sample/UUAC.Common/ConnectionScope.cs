using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UUAC.Common;
using Vulcan.DataAccess;

namespace UUAC.Common
{
    public class ConnectionScope: Vulcan.DataAccess.ConnectionScope
    {
        public ConnectionScope() : this(Constans.MAIN_DB_KEY)
        {
            
        }

        public ConnectionScope(string key) : base(ConnectionStringManager.GetConnectionString(key))
        {

        }

        public ConnectionScope(string key, IConnectionFactory factory) : base(ConnectionStringManager.GetConnectionString(key), factory)
        {
        }
    }
}
