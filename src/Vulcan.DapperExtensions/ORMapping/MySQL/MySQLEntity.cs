using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Vulcan.DapperExtensions.ORMapping.MySQL
{
    [ExcludeFromCodeCoverage]
    public class MySQLEntity : AbstractBaseEntity
    {
        private static readonly ConcurrentDictionary<Type, string> ReplaceSqlCache =
            new ConcurrentDictionary<Type, string>();


        private static readonly MySQLSQLBuilder MySqlBuilder = new MySQLSQLBuilder();

        protected override ISQLBuilder SQLBuilder => MySqlBuilder;

        public string GetReplaceInsertSQL()
        {
            var t = GetType();
            if (ReplaceSqlCache.ContainsKey(t)) return ReplaceSqlCache[t];

            var metaData = EntityReflect.GetDefineInfoFromType(t);
            var sql = MySqlBuilder.BuildReplaceInsertSQL(metaData);

            ReplaceSqlCache.TryAdd(t, sql);
            return sql;
        }
    }
}
