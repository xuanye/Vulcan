using NpgSql;
using NpgSqlTypes;
using System.Data;
using static Dapper.SqlMapper;

namespace Vulcan.DapperExtensions.NpgSql
{
    public class JsonbParameter : ICustomQueryParameter
    {
        private readonly string _value;

        public JsonbParameter(string value)
        {
            _value = value;
        }

        public void AddParameter(IDbCommand command, string name)
        {
            var parameter = new NpgSqlParameter(name, NpgSqlDbType.Jsonb)
            {
                Value = _value
            };

            command.Parameters.Add(parameter);
        }
    }
}
