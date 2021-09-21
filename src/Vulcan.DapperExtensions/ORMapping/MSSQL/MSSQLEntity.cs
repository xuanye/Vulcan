using System.Diagnostics.CodeAnalysis;

namespace Vulcan.DapperExtensions.ORMapping.MSSQL
{
    [ExcludeFromCodeCoverage]
    public class MSSQLEntity : AbstractBaseEntity
    {
        private static readonly MSSQLSQLBuilder _builder = new MSSQLSQLBuilder();

        protected override ISQLBuilder SQLBuilder => _builder;
    }
}
