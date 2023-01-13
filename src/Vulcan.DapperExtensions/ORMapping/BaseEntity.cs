using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Vulcan.DapperExtensions.ORMapping
{
    public abstract class BaseEntity
    {
        private static readonly ConcurrentDictionary<Type, string> _InsertSqlCache =
            new ConcurrentDictionary<Type, string>();

        private static readonly ConcurrentDictionary<Type, string> _UpdateSqlCache =
            new ConcurrentDictionary<Type, string>();

        private static readonly object _LockObject = new object();
        private readonly List<string> _PropertyChangedList = new List<string>();

        #region Properties

        [Ignore] public bool FullUpdate { get; set; }

        #endregion

        protected void Clear()
        {
            lock (_LockObject)
            {
                _PropertyChangedList.Clear();
            }
        }

        protected void OnPropertyChanged(string pName)
        {
            lock (_LockObject)
            {
                if (!_PropertyChangedList.Contains(pName)) _PropertyChangedList.Add(pName);
            }
        }

        #region Public Methods

        public string GetInsertSQL(ISQLBuilder sqlBuilder)
        {
            return FullUpdate ? GetInsertFullSql(sqlBuilder) : GetInsertChangeColumnsSql(sqlBuilder);
        }

        public string GetUpdateSQL(ISQLBuilder sqlBuilder)
        {
            return FullUpdate ? GetUpdateFullSql(sqlBuilder) : GetUpdateChangeColumnsSql(sqlBuilder);
        }

        public void RemoveUpdateColumn(string ColumnName)
        {
            lock (_LockObject)
            {
                if (_PropertyChangedList.Contains(ColumnName)) _PropertyChangedList.Remove(ColumnName);
            }
        }

        #endregion

        #region Private Methods

        private string GetInsertFullSql(ISQLBuilder sqlBuilder)
        {
            var t = GetType();
            if (_InsertSqlCache.TryGetValue(t, out var sql)) return sql;
            var metaData = EntityReflect.GetDefineInfoFromType(t);
            sql = sqlBuilder.BuildInsertSql(metaData);

            _InsertSqlCache.TryAdd(t, sql);

            return _InsertSqlCache[t];
        }

        private string GetUpdateFullSql(ISQLBuilder sqlBuilder)
        {
            var t = GetType();
            if (_UpdateSqlCache.TryGetValue(t, out var sql)) return sql;

            var metaData = EntityReflect.GetDefineInfoFromType(t);
            sql = sqlBuilder.BuildUpdateSql(metaData);
            _UpdateSqlCache.TryAdd(t, sql);
            return _UpdateSqlCache[t];
        }

        private string GetInsertChangeColumnsSql(ISQLBuilder sqlBuilder)
        {
            var metaData = EntityReflect.GetDefineInfoFromType(GetType());
            lock (_LockObject)
            {
                return sqlBuilder.BuildInsertSql(metaData, _PropertyChangedList);
            }
        }

        private string GetUpdateChangeColumnsSql(ISQLBuilder sqlBuilder)
        {
            var metaData = EntityReflect.GetDefineInfoFromType(GetType());
            lock (_LockObject)
            {
                return sqlBuilder.BuildUpdateSql(metaData, _PropertyChangedList);
            }
        }

        #endregion
    }
}
