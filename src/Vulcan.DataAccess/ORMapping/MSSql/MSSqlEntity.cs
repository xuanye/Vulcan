namespace Vulcan.DataAccess.ORMapping.MSSql
{
    public class MSSqlEntity : BaseEntity
    {
        private static MSSqlSQLBuilder _builder = new MSSqlSQLBuilder();

        protected override ISQLBuilder SQLBuilder
        {
            get { return _builder; }
        }
    }
}