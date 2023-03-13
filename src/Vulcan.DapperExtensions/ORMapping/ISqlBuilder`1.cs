using System.Collections.Generic;

namespace Vulcan.DapperExtensions.ORMapping
{
    public interface ISqlBuilder
    {
        /// <summary>
        ///     Get the INSERT Sql statement based on the meta-information of the entity
        /// </summary>
        /// <param name="meta">The meta.</param>
        /// <returns></returns>
        string BuildInsertSql(EntityMeta meta);

        /// <summary>
        ///     Get the UPDATE Sql statement based on the meta-information of the entity
        /// </summary>
        /// <param name="meta">The meta.</param>
        /// <returns></returns>
        string BuildUpdateSql(EntityMeta meta);

        /// <summary>
        ///     Get an INSERT Sql statement for the specified column list based on entity meta-information
        /// </summary>
        /// <param name="meta">meta.</param>
        /// <param name="list">column list.</param>
        /// <returns></returns>
        string BuildInsertSql(EntityMeta meta, List<string> list);

        /// <summary>
        ///     Get an UPDATE Sql statement for the specified column list based on entity meta-information
        /// </summary>
        /// <param name="meta">meta</param>
        /// <param name="list">column list.</param>
        /// <returns></returns>
        string BuildUpdateSql(EntityMeta meta, List<string> list);

        //MySql Only
        //string EntityMetaToReplaceInsertSql(EntityMeta meta);
    }
}
