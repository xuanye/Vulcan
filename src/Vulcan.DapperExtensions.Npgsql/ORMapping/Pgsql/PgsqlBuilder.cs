using System.Collections.Generic;
using System.Text;

namespace Vulcan.DapperExtensions.ORMapping.PgSql
{
    public class PgsqlBuilder : ISqlBuilder
    {
        public static PgsqlBuilder Instance = new PgsqlBuilder();

        public string BuildInsertSql(EntityMeta meta)
        {
            if (meta.Columns == null || meta.Columns.Count == 0)
                return string.Empty;

            var Sqlbuilder = new StringBuilder();
            Sqlbuilder.AppendFormat("INSERT INTO '{0}' (", meta.TableName);
            for (int i = 0, j = 0; i < meta.Columns.Count; i++)
            {
                if (meta.Columns[i].Identity) continue;
                if (j > 0) Sqlbuilder.Append(",");
                Sqlbuilder.Append("'" + meta.Columns[i].ColumnName + "'");
                j++;
            }

            Sqlbuilder.Append(") VALUES (");
            for (int i = 0, j = 0; i < meta.Columns.Count; i++)
            {
                if (meta.Columns[i].Identity) continue;
                if (j > 0) Sqlbuilder.Append(",");
                Sqlbuilder.Append("@" + meta.Columns[i].PropertyName + "");
                j++;
            }

            Sqlbuilder.Append(")");

            var identityCol = meta.Columns.Find(x => x.Identity);
            if (identityCol != null)
                Sqlbuilder.AppendFormat(" RETURNING {0};", identityCol.ColumnName);
            else
                Sqlbuilder.Append(";SELECT CAST(0 AS BIGINT);");

            return Sqlbuilder.ToString();
        }

        public string BuildUpdateSql(EntityMeta meta)
        {
            if (meta.Columns == null || meta.Columns.Count == 0)
                return string.Empty;

            var keys = meta.Columns.FindAll(_ => _.PrimaryKey);

            var Sqlbuilder = new StringBuilder();
            Sqlbuilder.AppendFormat("UPDATE '{0}' SET ", meta.TableName);

            for (int i = 0, j = 0; i < meta.Columns.Count; i++)
                if (!meta.Columns[i].PrimaryKey)
                {
                    if (j > 0) Sqlbuilder.Append(",");
                    j++;
                    Sqlbuilder.Append("'" + meta.Columns[i].ColumnName + "'=@" + meta.Columns[i].PropertyName + "");
                }

            Sqlbuilder.Append(" WHERE ");
            for (var i = 0; i < keys.Count; i++)
            {
                if (i > 0) Sqlbuilder.Append(" AND ");
                Sqlbuilder.Append("'" + keys[i].ColumnName + "'=@" + keys[i].PropertyName);
            }

            return Sqlbuilder.ToString();
        }

        public string BuildInsertSql(EntityMeta meta, List<string> list)
        {
            if (list == null || list.Count == 0)
                return string.Empty;

            var Sqlbuilder = new StringBuilder();
            Sqlbuilder.AppendFormat("INSERT INTO '{0}' (", meta.TableName);
            for (int i = 0, j = 0; i < meta.Columns.Count; i++)
            {
                if (meta.Columns[i].Identity || !list.Contains(meta.Columns[i].ColumnName)) continue;
                if (j > 0) Sqlbuilder.Append(",");
                Sqlbuilder.Append("'" + meta.Columns[i].ColumnName + "'");
                j++;
            }

            Sqlbuilder.Append(") VALUES (");
            for (int i = 0, j = 0; i < meta.Columns.Count; i++)
            {
                if (meta.Columns[i].Identity || !list.Contains(meta.Columns[i].ColumnName)) continue;
                if (j > 0) Sqlbuilder.Append(",");
                Sqlbuilder.Append("@" + meta.Columns[i].PropertyName + "");
                j++;
            }

            Sqlbuilder.Append(")");

            var identityCol = meta.Columns.Find(x => x.Identity);
            if (identityCol != null)
                Sqlbuilder.AppendFormat(" RETURNING {0};", identityCol.ColumnName);
            else
                Sqlbuilder.Append(";SELECT CAST(0 AS BIGINT);");

            return Sqlbuilder.ToString();
        }

        public string BuildUpdateSql(EntityMeta meta, List<string> list)
        {
            if (list == null || list.Count == 0)
                return string.Empty;

            var keys = meta.Columns.FindAll(_ => _.PrimaryKey);

            var Sqlbuilder = new StringBuilder();
            Sqlbuilder.AppendFormat("UPDATE '{0}' SET ", meta.TableName);

            for (int i = 0, j = 0; i < meta.Columns.Count; i++)
                if (!meta.Columns[i].PrimaryKey && list.Contains(meta.Columns[i].ColumnName))
                {
                    if (j > 0) Sqlbuilder.Append(",");
                    j++;
                    Sqlbuilder.Append("'" + meta.Columns[i].ColumnName + "'=@" + meta.Columns[i].PropertyName + "");
                }

            Sqlbuilder.Append(" WHERE ");
            for (var i = 0; i < keys.Count; i++)
            {
                if (i > 0) Sqlbuilder.Append(" AND ");
                Sqlbuilder.Append("'" + keys[i].ColumnName + "'=@" + keys[i].PropertyName);
            }

            return Sqlbuilder.ToString();
        }


    }
}
