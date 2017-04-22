using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vulcan.DataAccess
{
    public class ConnectionScope:IDisposable
    {

        private string _conStr;
        private ConnectionManager _connectionManager;
        //private IConnectionFactory _dbFactory;

        public ConnectionScope(string constr) : this(constr, null)
        {
        }

        public ConnectionScope(string constr, IConnectionFactory factory )
        {
       
            this._conStr = constr;
            _connectionManager = factory == null ?
               ConnectionManager.GetManager(constr)
               : ConnectionManager.GetManager(factory, constr);
            //this._dbFactory = factory;
        }
  


        public void Dispose()
        {
            _connectionManager.Dispose();
            _conStr = null;
            _connectionManager = null;
            //_dbFactory = null;
        }
    }
}
