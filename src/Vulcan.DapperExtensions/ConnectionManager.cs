using System;
using System.Data;

namespace Vulcan.DapperExtensions
{
    /// <summary>
    /// wrapper connection manager
    /// </summary>
    public class ConnectionManager : IDisposable
    {
        internal ConnectionManager(IConnectionFactory factory, string connectionString, IRuntimeContextStorage ctxStorage)
        {
            _dbConnectionString = connectionString;
            _ctxStorage = ctxStorage;

            Connection = factory.CreateDbConnection(connectionString);

            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
        }

        #region Fields

        private static readonly object LockObject = new object();
        private readonly string _dbConnectionString;

        private readonly IRuntimeContextStorage _ctxStorage;
        #endregion

        #region Properties
        public IDbConnection Connection { get; }

        public IDbTransaction Transaction { get; private set; }


        #endregion


        #region Public Method

        public IDbTransaction BeginTransaction()
        {
            Transaction = this.Connection.BeginTransaction();
            return Transaction;
        }

        public bool IsExistDbTransaction()
        {
            return Transaction != null;
        }

        #endregion

        #region Counter

        public int RefCount { get; private set; }

        internal void AddRef()
        {
            RefCount += 1;
        }

        private void DeRef()
        {
            lock (LockObject)
            {
                RefCount -= 1;
                if (RefCount != 0) return;

                if (Connection != null && Connection.State != ConnectionState.Closed)
                    Connection.Close();//Dispose

                _ctxStorage.Remove(_dbConnectionString);
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            DeRef();
        }

        #endregion IDisposable
    }
}
