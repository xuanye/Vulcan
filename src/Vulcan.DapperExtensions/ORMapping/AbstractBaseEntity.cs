using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Vulcan.DapperExtensions.ORMapping
{
    public abstract class AbstractBaseEntity
    {
        private static readonly ConcurrentDictionary<Type, string> _InsertSqlCache =
            new ConcurrentDictionary<Type, string>();

        private static readonly ConcurrentDictionary<Type, string> _UpdateSqlCache =
            new ConcurrentDictionary<Type, string>();

        private static readonly object _LockObject = new object();
        private readonly List<string> _PropertyChangedList = new List<string>();

        #region Properties

        [Ignore]
        public bool FullUpdate { get; set; }

        #endregion

        protected abstract ISQLBuilder SQLBuilder { get; }

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

        public string GetInsertSQL()
        {
            return FullUpdate ? GetInsertFullSql() : GetInsertChangeColumnsSql();
        }

        public string GetUpdateSQL()
        {
            return FullUpdate ? GetUpdateFullSql() : GetUpdateChangeColumnsSql();
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

        private string GetInsertFullSql()
        {
            var t = GetType();
            if (_InsertSqlCache.TryGetValue(t, out var sql)) return sql;
            var metaData = EntityReflect.GetDefineInfoFromType(t);
            sql = SQLBuilder.BuildInsertSql(metaData);

            _InsertSqlCache.TryAdd(t, sql);

            return _InsertSqlCache[t];
        }

        private string GetUpdateFullSql()
        {
            var t = GetType();
            if (_UpdateSqlCache.TryGetValue(t, out var sql)) return sql;

            var metaData = EntityReflect.GetDefineInfoFromType(t);
            sql = SQLBuilder.BuildUpdateSql(metaData);
            _UpdateSqlCache.TryAdd(t, sql);
            return _UpdateSqlCache[t];
        }

        private string GetInsertChangeColumnsSql()
        {
            var metaData = EntityReflect.GetDefineInfoFromType(GetType());
            lock (_LockObject)
            {
                return SQLBuilder.BuildInsertSql(metaData, _PropertyChangedList);
            }
        }

        private string GetUpdateChangeColumnsSql()
        {
            var metaData = EntityReflect.GetDefineInfoFromType(GetType());
            lock (_LockObject)
            {
                return SQLBuilder.BuildUpdateSql(metaData, _PropertyChangedList);
            }
        }

        #endregion
    }
}
