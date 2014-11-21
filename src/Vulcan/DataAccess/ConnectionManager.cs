using System;
using System.Data;

namespace Vulcan.DataAccess
{
    /// <summary>
    /// 连接管理
    /// </summary>
    public class ConnectionManager : IDisposable
    {
        #region 属性和字段

        private static object _lock = new object();
        private string _dbConnectionString;
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public IDbConnection Connection
        {
            get
            {
                return _connection;
            }
        }

        public IDbTransaction Transaction
        {
            get
            {
                return _transaction;
            }
        }

        #endregion 属性和字段

        #region 静态方法

        public static ConnectionManager GetManager(string connectionString)
        {
            ConnectionManager mgr = null;
            if (!ApplicationContext.LocalContext.Contains(connectionString))
                lock (_lock)
                {
                    if (!ApplicationContext.LocalContext.Contains(connectionString))
                    {
                        mgr = new ConnectionManager(connectionString);
                        ApplicationContext.LocalContext[connectionString] = mgr;
                    }
                }
            mgr = (ConnectionManager)(ApplicationContext.LocalContext[connectionString]);

            mgr.AddRef();
            return mgr;
        }

        #endregion 静态方法

        #region 构造

        private ConnectionManager(string connectionString)
        {
            _dbConnectionString = connectionString;
            _connection = ConnectionFactory.CreateDbConnection(connectionString);
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        #endregion 构造

        #region public

        public IDbTransaction BeginTransaction()
        {
            _transaction = this.Connection.BeginTransaction();
            return _transaction;
        }

        public bool IsExistDbTransaction()
        {
            return Transaction != null;
        }

        #endregion public

        #region 计数器

        private int _refCount;

        public int RefCount
        {
            get { return _refCount; }
        }

        private void AddRef()
        {
            _refCount += 1;
        }

        private void DeRef()
        {
            lock (_lock)
            {
                _refCount -= 1;
                if (_refCount == 0)
                {
                    if (_connection != null && _connection.State != ConnectionState.Closed)
                        _connection.Close();//Dispose
                    ApplicationContext.LocalContext.Remove(_dbConnectionString);
                }
            }
        }

        #endregion 计数器

        #region IDisposable

        public void Dispose()
        {
            DeRef();
        }

        #endregion IDisposable
    }
}