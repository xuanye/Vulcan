using System;
using Vulcan.DapperExtensions.ORMapping;
using Vulcan.DapperExtensions.ORMapping.MSSQL;
using Vulcan.DapperExtensions.ORMapping.MySQL;

namespace Vulcan.DapperExtensionsUnitTests.Internal
{
    public class BaseEntityClass : AbstractBaseEntity
    {
        private ISQLBuilder _SQLBuilder;

        protected override ISQLBuilder SQLBuilder
        {
            get
            {
                if (_SQLBuilder != null) return _SQLBuilder;

                _SQLBuilder = TestDataBaseSwitcher.DataBaseType switch
                {
                    DataBaseType.MySQL => new MySQLSQLBuilder(),
                    DataBaseType.MSSQL => new MSSQLSQLBuilder(),
                    _ => throw new NotSupportedException()
                };

                return _SQLBuilder;
            }
        }
    }
}
