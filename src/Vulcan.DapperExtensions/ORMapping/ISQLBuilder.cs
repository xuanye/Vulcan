using System.Collections.Generic;

namespace Vulcan.DapperExtensions.ORMapping
{
    public interface ISQLBuilder
    {
        /// <summary>
        ///根据实体的源信息获取insert sql 语句
        /// </summary>
        /// <param name="meta">The meta.</param>
        /// <returns></returns>
        string BuildInsertSql(EntityMeta meta);

        /// <summary>
        /// 根据实体的源信息获取update sql 语句
        /// </summary>
        /// <param name="meta">The meta.</param>
        /// <returns></returns>
        string BuildUpdateSql(EntityMeta meta);

        /// <summary>
        /// 根据实体元信息获取指定列的insert sql 语句
        /// </summary>
        /// <param name="meta">实体元信息.</param>
        /// <param name="list">新增的字段名列表.</param>
        /// <returns></returns>
        string BuildInsertSql(EntityMeta meta, List<string> list);

        /// <summary>
        /// 根据实体元信息获取指定列的update sql 语句
        /// </summary>
        /// <param name="meta">实体元信息</param>
        /// <param name="list">更新的字段名列表.</param>
        /// <returns></returns>
        string BuildUpdateSql(EntityMeta meta, List<string> list);

        //MySql Only
        //string EntityMetaToReplaceInsertSQL(EntityMeta meta);
    }
}
