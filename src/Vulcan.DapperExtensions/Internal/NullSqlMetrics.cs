using System.Diagnostics.CodeAnalysis;
using Vulcan.DapperExtensions.Contract;

namespace Vulcan.DapperExtensions.Internal
{
    [ExcludeFromCodeCoverage]
    internal class NoopSqlMetrics : ISqlMetrics
    {
        public void AddToMetrics(string sql, object param)
        {
        }

        public void Dispose()
        {
        }
    }
}
