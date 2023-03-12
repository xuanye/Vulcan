using System.Collections.Generic;
using System.Text;

namespace Vulcan.DapperExtensions.ORMapping.Mysql
{
    public class MysqlBuilder : ISqlBuilder
    {
        public static MysqlBuilder Instance = new MysqlBuilder();

        public string BuildInsertSql(EntityMeta meta)
        {
            if (meta.Columns == null || meta.Columns.Count == 0)
                return string.Empty;

            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("INSERT INTO `{0}` (", meta.TableName);
            for (int i = 0, j = 0; i < meta.Columns.Count; i++)
            {
                if (meta.Columns[i].Identity) continue;
                if (j > 0) sqlBuilder.Append(",");
                sqlBuilder.Append("`" + meta.Columns[i].ColumnName + "`");
                j++;
            }

            sqlBuilder.Append(") VALUES (");
            for (int i = 0, j = 0; i < meta.Columns.Count; i++)
            {
                if (meta.Columns[i].Identity) continue;
                if (j > 0) sqlBuilder.Append(",");
                sqlBuilder.Append("@" + meta.Columns[i].PropertyName + "");
                j++;
            }

            sqlBuilder.Append(");");

            if (meta.Columns.Exists(x => x.Identity))
                sqlBuilder.Append("SELECT CAST(LAST_INSERT_ID() AS SIGNED) Id;");
            else
                sqlBuilder.Append("SELECT CAST(0 AS SIGNED) Id;");

            return sqlBuilder.ToString();
        }

        public string BuildUpdateSql(EntityMeta meta)
        {
            if (meta.Columns == null || meta.Columns.Count == 0)
                return string.Empty;

            var keys = meta.Columns.FindAll(_ => _.PrimaryKey);

            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("UPDATE `{0}` SET ", meta.TableName);

            for (int i = 0, j = 0; i < meta.Columns.Count; i++)
                if (!meta.Columns[i].PrimaryKey)
                {
                    if (j > 0) sqlBuilder.Append(",");
                    j++;
                    sqlBuilder.Append("`" + meta.Columns[i].ColumnName + "`=@" + meta.Columns[i].PropertyName + "");
                }

            sqlBuilder.Append(" WHERE ");
            for (var i = 0; i < keys.Count; i++)
            {
                if (i > 0) sqlBuilder.Append(" AND ");
                sqlBuilder.Append("`" + keys[i].ColumnName + "`=@" + keys[i].PropertyName);
            }

            return sqlBuilder.ToString();
        }

        public string BuildInsertSql(EntityMeta meta, List<string> list)
        {
            if (list == null || list.Count == 0)
                return string.Empty;

            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("INSERT INTO `{0}` (", meta.TableName);
            for (int i = 0, j = 0; i < meta.Columns.Count; i++)
            {
                if (meta.Columns[i].Identity || !list.Contains(meta.Columns[i].ColumnName)) continue;
                if (j > 0) sqlBuilder.Append(",");
                sqlBuilder.Append("`" + meta.Columns[i].ColumnName + "`");
                j++;
            }

            sqlBuilder.Append(") VALUES (");
            for (int i = 0, j = 0; i < meta.Columns.Count; i++)
            {
                if (meta.Columns[i].Identity || !list.Contains(meta.Columns[i].ColumnName)) continue;
                if (j > 0) sqlBuilder.Append(",");
                sqlBuilder.Append("@" + meta.Columns[i].PropertyName + "");
                j++;
            }

            sqlBuilder.Append(");");

            if (meta.Columns.Exists(x => x.Identity))
                sqlBuilder.Append("SELECT CAST(LAST_INSERT_ID() AS SIGNED) Id;");
            else
                sqlBuilder.Append(" SELECT CAST(0 AS SIGNED) Id;");

            return sqlBuilder.ToString();
        }

        public string BuildUpdateSql(EntityMeta meta, List<string> list)
        {
            if (list == null || list.Count == 0)
                return string.Empty;

            var keys = meta.Columns.FindAll(_ => _.PrimaryKey);

            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("UPDATE `{0}` SET ", meta.TableName);

            for (int i = 0, j = 0; i < meta.Columns.Count; i++)
                if (!meta.Columns[i].PrimaryKey && list.Contains(meta.Columns[i].ColumnName))
                {
                    if (j > 0) sqlBuilder.Append(",");
                    j++;
                    sqlBuilder.Append("`" + meta.Columns[i].ColumnName + "`=@" + meta.Columns[i].PropertyName + "");
                }

            sqlBuilder.Append(" WHERE ");
            for (var i = 0; i < keys.Count; i++)
            {
                if (i > 0) sqlBuilder.Append(" AND ");
                sqlBuilder.Append("`" + keys[i].ColumnName + "`=@" + keys[i].PropertyName);
            }

            return sqlBuilder.ToString();
        }

        public string BuildReplaceInsertSql(EntityMeta meta)
        {
            if (meta.Columns == null || meta.Columns.Count == 0)
                return string.Empty;

            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("REPLACE INTO `{0}` (", meta.TableName);
            for (int i = 0, j = 0; i < meta.Columns.Count; i++)
            {
                if (meta.Columns[i].Identity) continue;
                if (j > 0) sqlBuilder.Append(",");
                sqlBuilder.Append("`" + meta.Columns[i].ColumnName + "`");
                j++;
            }

            sqlBuilder.Append(") VALUES (");
            for (int i = 0, j = 0; i < meta.Columns.Count; i++)
            {
                if (meta.Columns[i].Identity) continue;
                if (j > 0) sqlBuilder.Append(",");
                sqlBuilder.Append("@" + meta.Columns[i].PropertyName + "");
                j++;
            }

            sqlBuilder.Append(");");

            return sqlBuilder.ToString();
        }
    }
}
