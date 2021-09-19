using System.Data;
using Vulcan.DapperExtensions.Contract;

namespace Vulcan.DapperExtensions
{
    /// <summary>
    /// Transaction Management
    /// </summary>
    public class TransScope : IScope
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransScope"/> class.
        /// </summary>
        /// <param name="mgr"></param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="option">The option.</param>
        public TransScope(IConnectionManagerFactory mgr,string connectionString, TransScopeOption option = TransScopeOption.Required)
            :this(mgr, null, connectionString, option)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransScope"/> class.
        /// </summary>
        /// <param name="mgr"></param>
        /// <param name="factory"></param>
        /// <param name="connectionString"></param>
        /// <param name="option">notice: sql server don't support nested transaction </param>
        public TransScope(IConnectionManagerFactory mgr, IConnectionFactory factory,string connectionString, TransScopeOption option = TransScopeOption.Required)
        {
            _connectionManager = mgr.GetConnectionManager(connectionString, factory);

            if (!_connectionManager.IsExistDbTransaction() || option == TransScopeOption.RequiresNew)
            {
                _tran = _connectionManager.BeginTransaction();
                _beginTransactionIsInCurrentTransScope = true;
            }
            else
            {
                _tran = _connectionManager.Transaction;
            }
        }

        private readonly ConnectionManager _connectionManager;
        private readonly IDbTransaction _tran;

        /// <summary>
        /// Whether the transaction is started by this instance
        /// (which instance is responsible for starting and which object is responsible for committing)
        /// </summary>
        private readonly bool _beginTransactionIsInCurrentTransScope;

        private bool _completed;

        /// <summary>
        /// rollback  transaction
        /// </summary>
        public void Rollback()
        {
            if (!_beginTransactionIsInCurrentTransScope) return;

            _tran?.Rollback();
            _completed = true;

        }


        public void Dispose()
        {
            if (!_completed)
            {
                Rollback();
            }

            _connectionManager.Dispose();
        }

        public void Commit()
        {
            if (!_beginTransactionIsInCurrentTransScope || _tran == null) return;

            _tran.Commit();
            _completed = true;
        }

    }
}
