using System.Diagnostics.CodeAnalysis;
using Vulcan.DapperExtensions.Contract;

namespace Vulcan.DapperExtensions.Internal
{
    [ExcludeFromCodeCoverage]
    internal class NoopSQLMetrics : ISQLMetrics
    {
        public void AddToMetrics(string sql, object param)
        {

        }

        public void Dispose()
        {

        }
    }
}
