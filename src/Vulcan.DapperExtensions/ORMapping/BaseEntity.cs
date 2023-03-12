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

        private static readonly object _lockObject = new object();
        private readonly List<string> _propertyChangedList = new List<string>();

        #region Properties

        [Ignore]
        public bool FullUpdate { get; set; }

        #endregion

        protected void Clear()
        {
            lock (_lockObject)
            {
                _propertyChangedList.Clear();
            }
        }

        protected void OnPropertyChanged(string pName)
        {
            lock (_lockObject)
            {
                if (!_propertyChangedList.Contains(pName)) _propertyChangedList.Add(pName);
            }
        }

        #region Public Methods

        public string GetInsertSql(ISqlBuilder SqlBuilder)
        {
            return FullUpdate ? GetInsertFullSql(SqlBuilder) : GetInsertChangeColumnsSql(SqlBuilder);
        }

        public string GetUpdateSql(ISqlBuilder SqlBuilder)
        {
            return FullUpdate ? GetUpdateFullSql(SqlBuilder) : GetUpdateChangeColumnsSql(SqlBuilder);
        }

        public void RemoveUpdateColumn(string ColumnName)
        {
            lock (_lockObject)
            {
                if (_propertyChangedList.Contains(ColumnName)) _propertyChangedList.Remove(ColumnName);
            }
        }

        #endregion

        #region Private Methods

        private string GetInsertFullSql(ISqlBuilder SqlBuilder)
        {
            var t = GetType();
            if (_InsertSqlCache.TryGetValue(t, out var Sql)) return Sql;
            var metaData = EntityReflect.GetDefineInfoFromType(t);
            Sql = SqlBuilder.BuildInsertSql(metaData);

            _InsertSqlCache.TryAdd(t, Sql);

            return _InsertSqlCache[t];
        }

        private string GetUpdateFullSql(ISqlBuilder SqlBuilder)
        {
            var t = GetType();
            if (_UpdateSqlCache.TryGetValue(t, out var Sql)) return Sql;

            var metaData = EntityReflect.GetDefineInfoFromType(t);
            Sql = SqlBuilder.BuildUpdateSql(metaData);
            _UpdateSqlCache.TryAdd(t, Sql);
            return _UpdateSqlCache[t];
        }

        private string GetInsertChangeColumnsSql(ISqlBuilder SqlBuilder)
        {
            var metaData = EntityReflect.GetDefineInfoFromType(GetType());
            lock (_lockObject)
            {
                return SqlBuilder.BuildInsertSql(metaData, _propertyChangedList);
            }
        }

        private string GetUpdateChangeColumnsSql(ISqlBuilder SqlBuilder)
        {
            var metaData = EntityReflect.GetDefineInfoFromType(GetType());
            lock (_lockObject)
            {
                return SqlBuilder.BuildUpdateSql(metaData, _propertyChangedList);
            }
        }

        #endregion
    }
}
