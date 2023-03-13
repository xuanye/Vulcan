using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Vulcan.DapperExtensions.ORMapping.Mysql
{
    [ExcludeFromCodeCoverage]
    public class MysqlEntity : BaseEntity
    {
        private static readonly ConcurrentDictionary<Type, string> _replaceSqlCache =
            new ConcurrentDictionary<Type, string>();



        public string GetReplaceInsertSql()
        {
            var t = GetType();
            if (_replaceSqlCache.ContainsKey(t)) return _replaceSqlCache[t];

            var metaData = EntityReflect.GetDefineInfoFromType(t);
            var sql = MysqlBuilder.Instance.BuildReplaceInsertSql(metaData);

            _replaceSqlCache.TryAdd(t, sql);
            return sql;
        }
    }
}
