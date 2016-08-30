using System;
using System.Collections.Generic;

namespace Vulcan.DataAccess.ORMapping.MySql
{
    public class MySqlEntity : AbstractBaseEntity
    {
        private static Dictionary<Type, string> _ReplaceSqlCache = new Dictionary<Type, string>();
        private static object lockobject = new object();

        private static MySqlSQLBuilder _builder = new MySqlSQLBuilder();

        protected override ISQLBuilder SQLBuilder
        {
            get
            {
                return _builder;
            }
        }

        public string GetReplaceInsertSQL()
        {
            Type t = this.GetType();
            if (!_ReplaceSqlCache.ContainsKey(t))
            {
                EntityMeta metadeta = EntityReflect.GetDefineInfoFromType(t);
                string sql = _builder.BuildReplaceInsertSQL(metadeta);
                lock (lockobject)
                {
                    if (!_ReplaceSqlCache.ContainsKey(t))
                    {
                        _ReplaceSqlCache.Add(t, sql);
                    }
                }
            }
            return _ReplaceSqlCache[t];
        }
    }
}