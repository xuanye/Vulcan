using Vulcan.DapperExtensions.Contract;

namespace Vulcan.DapperExtensions
{
    public class ConnectionScope : IScope
    {
        private ConnectionManager _connectionManager;

        private string _connectionString;

        public ConnectionScope(IConnectionManagerFactory mgr, string connectionString,
            IConnectionFactory factory = null)
        {
            _connectionString = connectionString;
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
