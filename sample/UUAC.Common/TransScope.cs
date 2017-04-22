using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UUAC.Common;
using Vulcan.DataAccess;

namespace UUAC.Common
{
    public class TransScope:Vulcan.DataAccess.TransScope
    {
        public TransScope() : this(Constans.MAIN_DB_KEY)
        {
            
        }

        public TransScope(string key) : base(ConnectionStringManager.GetConnectionString(key))
        {
        }

        public TransScope( IConnectionFactory factory, string key) : base(factory,ConnectionStringManager.GetConnectionString(key))
        {
        }
    }
}
