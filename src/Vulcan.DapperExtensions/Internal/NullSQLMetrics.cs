using Vulcan.DapperExtensions.Contract;

namespace Vulcan.DapperExtensions.Internal
{
    internal class NullSQLMetrics : ISQLMetrics
    {
        public void AddToMetrics(string sql, object param)
        {

        }

        public void Dispose()
        {

        }
    }
}
