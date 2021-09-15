using System;
using System.Collections.Generic;

namespace Vulcan.DapperExtensions.ORMapping
{
    public abstract class AbstractBaseEntity
    {
        private static Dictionary<Type, string> _InsertSqlCache = new Dictionary<Type, string>();
        private static Dictionary<Type, string> _UpdateSqlCache = new Dictionary<Type, string>();

        private static object lockobject = new object();
        private List<string> _PropertyChangedList = new List<string>();

        #region 属性

        [Ignore]
        public bool FullUpdate { get; set; }

        #endregion 属性

        #region 公开方法

        public string GetInsertSQL()
        {
            if (FullUpdate)
            {
                return GetInsertFullSql();
            }
            return GetInsertChangeColumnsSql();
        }

        public string GetUpdateSQL()
        {
            if (FullUpdate)
            {
                return GetUpdateFullSql();
            }
            return GetUpdateChangeColumnsSql();
        }

        public void RemoveUpdateColumn(string ColumnName)
        {
            lock (lockobject)
            {
                if (_PropertyChangedList.Contains(ColumnName))
                {
                    _PropertyChangedList.Remove(ColumnName);
                }
            }
        }

        #endregion 公开方法

        protected abstract ISQLBuilder SQLBuilder
        {
            get;
        }

        protected void Clear()
        {
            lock (lockobject)
            {
                _PropertyChangedList.Clear();
            }
        }

        protected void OnPropertyChanged(string pName)
        {
            lock (lockobject)
            {
                if (!_PropertyChangedList.Contains(pName))
                {
                    _PropertyChangedList.Add(pName);
                }
            }
        }

        #region 私有方法

        private string GetInsertFullSql()
        {
            var t = this.GetType();
            if (!_InsertSqlCache.ContainsKey(t))
            {
                var metadeta = EntityReflect.GetDefineInfoFromType(t);
                var sql = SQLBuilder.BuildInsertSql(metadeta);
                lock (lockobject)
                {
                    if (!_InsertSqlCache.ContainsKey(t))
                    {
                        _InsertSqlCache.Add(t, sql);
                    }
                }
            }
            return _InsertSqlCache[t];
        }

        private string GetUpdateFullSql()
        {
            var t = this.GetType();
            if (!_UpdateSqlCache.ContainsKey(t))
            {
                var metadeta = EntityReflect.GetDefineInfoFromType(t);
                var sql = SQLBuilder.BuildUpdateSql(metadeta);
                lock (lockobject)
                {
                    if (!_UpdateSqlCache.ContainsKey(t))
                    {
                        _UpdateSqlCache.Add(t, sql);
                    }
                }
            }
            return _UpdateSqlCache[t];
        }

        private string GetInsertChangeColumnsSql()
        {
            var metadeta = EntityReflect.GetDefineInfoFromType(this.GetType());
            return SQLBuilder.BuildInsertSql(metadeta, this._PropertyChangedList);
        }

        private string GetUpdateChangeColumnsSql()
        {
            var metadeta = EntityReflect.GetDefineInfoFromType(this.GetType());
            return SQLBuilder.BuildUpdateSql(metadeta, this._PropertyChangedList);
        }

        #endregion 私有方法
    }
}
