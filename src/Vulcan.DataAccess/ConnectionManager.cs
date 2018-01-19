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
        private string _uuid;
        private readonly IRuntimeContextStorage _ctxStorage;

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

        #region 构造

        internal ConnectionManager(IConnectionFactory factory, string connectionString, IRuntimeContextStorage ctxStorage)
        {
            _dbConnectionString = connectionString;
            _ctxStorage = ctxStorage;

            _connection = factory.CreateDbConnection(connectionString);

            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }

            _uuid = new Guid().ToString("N");
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

        internal void AddRef()
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

                    _ctxStorage.Remove(_dbConnectionString);
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
