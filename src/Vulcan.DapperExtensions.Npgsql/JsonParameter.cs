using NpgSql;
using NpgSqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using static Dapper.SqlMapper;

namespace Vulcan.DapperExtensions.NpgSql
{
    public class JsonParameter : ICustomQueryParameter
    {
        private readonly string _value;

        public JsonParameter(string value)
        {
            _value = value;
        }

        public void AddParameter(IDbCommand command, string name)
        {
            var parameter = new NpgSqlParameter(name, NpgSqlDbType.Json)
            {
                Value = _value
            };

            command.Parameters.Add(parameter);
        }
    }
}
