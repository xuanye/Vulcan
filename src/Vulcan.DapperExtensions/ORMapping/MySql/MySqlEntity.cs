using System;
using System.Collections.Concurrent;

namespace Vulcan.DataAccess.ORMapping.MySql
{
    public class MySqlEntity : AbstractBaseEntity
    {

        private static readonly ConcurrentDictionary<Type, string> ReplaceSqlCache = new ConcurrentDictionary<Type, string>();


        private static readonly MySqlSQLBuilder MySqlBuilder = new MySqlSQLBuilder();

        protected override ISQLBuilder SQLBuilder => MySqlBuilder;

        public string GetReplaceInsertSQL()
        {
            var t = this.GetType();
            if (ReplaceSqlCache.ContainsKey(t)) return ReplaceSqlCache[t];

            var metaData = EntityReflect.GetDefineInfoFromType(t);
            var sql = MySqlBuilder.BuildReplaceInsertSQL(metaData);

            ReplaceSqlCache.TryAdd(t, sql);
            return sql;

        }
    }
}
