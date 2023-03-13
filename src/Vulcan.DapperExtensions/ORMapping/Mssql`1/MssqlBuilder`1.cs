using System.Collections.Generic;
using System.Text;

namespace Vulcan.DapperExtensions.ORMapping.Mssql
{
    public class MssqlBuilder : ISqlBuilder
    {
        public static MssqlBuilder Instance = new MssqlBuilder();

        public string BuildInsertSql(EntityMeta meta)
        {
            if (meta.Columns == null || meta.Columns.Count == 0)
                return string.Empty;

            var Sqlbuilder = new StringBuilder();
            Sqlbuilder.AppendFormat("INSERT INTO [{0}] (", meta.TableName);
            for (int i = 0, j = 0; i < meta.Columns.Count; i++)
            {
                if (meta.Columns[i].Identity) continue;
                if (j > 0) Sqlbuilder.Append(",");
                Sqlbuilder.Append("[" + meta.Columns[i].ColumnName + "]");
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

            Sqlbuilder.Append(");");
            var existsIdentity = meta.Columns.Exists(x => x.Identity);

            Sqlbuilder.Append(existsIdentity
                ? " SELECT CAST(SCOPE_IDENTITY() as bigint) as Id;"
                : " SELECT CAST(0 as bigint) as Id;");

            return Sqlbuilder.ToString();
        }

        public string BuildUpdateSql(EntityMeta meta)
        {
            if (meta.Columns == null || meta.Columns.Count == 0)
                return string.Empty;

            var keys = meta.Columns.FindAll(_ => _.PrimaryKey);

            var Sqlbuilder = new StringBuilder();
            Sqlbuilder.AppendFormat("UPDATE [{0}] SET ", meta.TableName);

            for (int i = 0, j = 0; i < meta.Columns.Count; i++)
            {
                if (meta.Columns[i].PrimaryKey) continue;

                if (j > 0) Sqlbuilder.Append(",");
                j++;
                Sqlbuilder.Append("[" + meta.Columns[i].ColumnName + "]=@" + meta.Columns[i].PropertyName + "");
            }

            Sqlbuilder.Append(" WHERE ");
            for (var i = 0; i < keys.Count; i++)
            {
                if (i > 0) Sqlbuilder.Append(" AND ");
                Sqlbuilder.Append("[" + keys[i].ColumnName + "]=@" + keys[i].PropertyName);
            }

            return Sqlbuilder.ToString();
        }

        public string BuildInsertSql(EntityMeta meta, List<string> list)
        {
            if (list == null || list.Count == 0)
                return string.Empty;

            var sqlbuilder = new StringBuilder();
            sqlbuilder.AppendFormat("INSERT INTO [{0}] (", meta.TableName);
            for (int i = 0, j = 0; i < meta.Columns.Count; i++)
            {
                if (meta.Columns[i].Identity || !list.Contains(meta.Columns[i].ColumnName)) continue;
                if (j > 0) sqlbuilder.Append(",");
                sqlbuilder.Append("[" + meta.Columns[i].ColumnName + "]");
                j++;
            }

            sqlbuilder.Append(") VALUES (");
            for (int i = 0, j = 0; i < meta.Columns.Count; i++)
            {
                if (meta.Columns[i].Identity || !list.Contains(meta.Columns[i].ColumnName)) continue;
                if (j > 0) sqlbuilder.Append(",");
                sqlbuilder.Append("@" + meta.Columns[i].PropertyName + "");
                j++;
            }

            sqlbuilder.Append(");");

            var exists = meta.Columns.Exists(x => x.Identity);
            sqlbuilder.Append(exists
                ? " SELECT CAST(SCOPE_IDENTITY() as bigint) as Id;"
                : " SELECT CAST(0 as bigint) as Id;");

            return sqlbuilder.ToString();
        }

        public string BuildUpdateSql(EntityMeta meta, List<string> list)
        {
            if (list == null || list.Count == 0)
                return string.Empty;

            var keys = meta.Columns.FindAll(_ => _.PrimaryKey);

            var sqlbuilder = new StringBuilder();
            sqlbuilder.AppendFormat("UPDATE [{0}] SET ", meta.TableName);

            for (int i = 0, j = 0; i < meta.Columns.Count; i++)
            {
                if (meta.Columns[i].PrimaryKey || !list.Contains(meta.Columns[i].ColumnName)) continue;

                if (j > 0) sqlbuilder.Append(",");
                j++;
                sqlbuilder.Append("[" + meta.Columns[i].ColumnName + "]=@" + meta.Columns[i].PropertyName + "");
            }

            sqlbuilder.Append(" WHERE ");
            for (var i = 0; i < keys.Count; i++)
            {
                if (i > 0) sqlbuilder.Append(" AND ");
                sqlbuilder.Append("[" + keys[i].ColumnName + "]=@" + keys[i].PropertyName);
            }

            return sqlbuilder.ToString();
        }
    }
}
