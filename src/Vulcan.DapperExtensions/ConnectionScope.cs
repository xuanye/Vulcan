using Vulcan.DapperExtensions.Contract;

namespace Vulcan.DapperExtensions
{
    public class ConnectionScope: IScope
    {

        private string _connectionString;
        private ConnectionManager _connectionManager;

        public ConnectionScope(IConnectionManagerFactory mgr, string connectionString, IConnectionFactory factory = null)
        {
            this._connectionString = connectionString;
            _connectionManager = mgr.GetConnectionManager(_connectionString, factory);
        }

        public void Commit()
        {
        }

        public void Dispose()
        {           
            _connectionManager.Dispose();
            _connectionString = null;
            _connectionManager = null;
        }

        public void Rollback()
        {
        }
    }
}
