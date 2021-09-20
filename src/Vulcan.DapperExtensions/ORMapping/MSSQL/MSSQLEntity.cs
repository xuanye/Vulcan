namespace Vulcan.DapperExtensions.ORMapping.MSSQL
{
    public class MSSQLEntity : AbstractBaseEntity
    {
        private static readonly MSSQLSQLBuilder _builder = new MSSQLSQLBuilder();

        protected override ISQLBuilder SQLBuilder => _builder;
    }
}
