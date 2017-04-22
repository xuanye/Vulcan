using System;
using System.Data;
using Vulcan.DataAccess.Context;

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
        private string _uuid;
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
            return GetManager(null, connectionString);
        }
        public static ConnectionManager GetManager(IConnectionFactory factory,string connectionString)
        {
            ConnectionManager mgr;
            if (!AppRuntimeContext.Contains(connectionString))
            {
                lock (_lock)
                {
                    if (!AppRuntimeContext.Contains(connectionString))
                    {
                        mgr = new ConnectionManager(factory, connectionString);
                        AppRuntimeContext.SetItem(connectionString, mgr);
                    }
                }
            }
            mgr = (ConnectionManager)AppRuntimeContext.GetItem(connectionString);

            mgr.AddRef();
            return mgr;
        }
        #endregion 静态方法

        #region 构造

        private ConnectionManager(IConnectionFactory factory, string connectionString)
        {
            _dbConnectionString = connectionString;

            _connection = factory == null ? 
                ConnectionFactoryHelper.CreateDefaultDbConnection(connectionString) 
                : factory.CreateDbConnection(connectionString);
           
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }

            _uuid = new Guid().ToString("D");
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
                    AppRuntimeContext.RemoveItem(_dbConnectionString);
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