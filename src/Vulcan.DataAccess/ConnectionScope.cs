using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vulcan.DataAccess
{
    public class ConnectionScope: IScope
    {

        private string _conStr;
        private ConnectionManager _connectionManager;
        //private IConnectionFactory _dbFactory;

        public ConnectionScope(IConnectionManagerFactory mgr, string constr) : this(mgr,constr, null)
        {
        }

        public ConnectionScope(IConnectionManagerFactory mgr, string constr, IConnectionFactory factory )
        {
       
            this._conStr = constr;
            _connectionManager = factory == null ?
               mgr.GetConnectionManager(constr)
               : mgr.GetConnectionManager(factory, constr);
            //this._dbFactory = factory;
        }

        public void Commit()
        {
            //throw new NotImplementedException();
        }

        public void Dispose()
        {
            _connectionManager.Dispose();
            _conStr = null;
            _connectionManager = null;
            //_dbFactory = null;
        }

        public void Rollback()
        {
            //throw new NotImplementedException();
        }
    }
}
