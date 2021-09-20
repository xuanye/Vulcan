namespace Vulcan.DapperExtensions.ORMapping.MSSql
{
    public class MSSqlEntity : AbstractBaseEntity
    {
        private static readonly MSSqlSQLBuilder _builder = new MSSqlSQLBuilder();

        protected override ISQLBuilder SQLBuilder => _builder;
    }
}
