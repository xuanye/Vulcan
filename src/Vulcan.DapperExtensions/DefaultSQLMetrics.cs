using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Vulcan.DapperExtensions.Contract;

namespace Vulcan.DapperExtensions
{
    public class DefaultSqlMetrics : ISqlMetrics
    {
        private readonly ILogger<DefaultSqlMetrics> _logger;
        private object _param;
        private string _Sql;
        private Stopwatch _sw;

        public DefaultSqlMetrics(ILogger<DefaultSqlMetrics> logger)
        {
            _sw = Stopwatch.StartNew();
            _logger = logger;
        }

        public void AddToMetrics(string Sql, object param)
        {
            _Sql = Sql;
            _param = param;
        }

        public void Dispose()
        {
            _sw.Stop();
            var es = _sw.ElapsedMilliseconds;
            _logger.LogDebug("Sql EXECUTE Finished in {0} ms,Sql={1},params = {_param}", es, _Sql, _param ?? "");
            if (es > 500)
                _logger.LogWarning("Sql EXECUTE Finished in {0} ms,Sql={1},params = {_param}", es, _Sql, _param ?? "");
            _sw = null;
            _Sql = null;
            _param = null;
        }
    }
}
