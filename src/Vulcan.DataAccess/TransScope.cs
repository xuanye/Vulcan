using System;
using System.Data;

namespace Vulcan.DataAccess
{
    /// <summary>
    /// 事务管理
    /// </summary>
    public class TransScope : IScope
    {
        private readonly TransScopeOption _option;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransScope"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="option">The option.</param>
        public TransScope(IConnectionManagerFactory mgr,string connectionString, TransScopeOption option = TransScopeOption.Required)
            :this(mgr, null, connectionString, option)
        {
            _option = option;
        }

        public TransScope(IConnectionManagerFactory mgr, IConnectionFactory factory,string connectionString, TransScopeOption option = TransScopeOption.Required)
        {
            _connectionManager = factory == null ?
                mgr.GetConnectionManager(connectionString) 
                : mgr.GetConnectionManager(factory, connectionString);

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
        /// 是否是该对象开启的事务(哪个对象负责开启则哪个对象负责提交)
        /// </summary>
        private readonly bool _beginTransactionIsInCurrentTransScope = false;

        private bool _completed = false;

        /// <summary>
        /// Commit Transaction
        /// </summary>
        public void Complete()
        {
            if (_beginTransactionIsInCurrentTransScope && _tran != null)
            {
                _tran.Commit();
                _completed = true;
            }
        }

        /// <summary>
        /// rollback  transaction
        /// </summary>
        public void Rollback()
        {
            if (_beginTransactionIsInCurrentTransScope)
            {
                _tran?.Rollback();
                _completed = true;
            }

        }

        /// <summary>
        /// close connection and rollback transaction
        /// </summary>
        public void Close()
        {
            if (!_completed)
            {
                Rollback();
            }

            _connectionManager.Dispose();
        }

      

        public void Dispose()
        {
            Close();
        }

        public void Commit()
        {
            this.Complete();
        }

    }
}
