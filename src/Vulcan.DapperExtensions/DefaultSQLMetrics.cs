using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Vulcan.DapperExtensions.Contract;

namespace Vulcan.DapperExtensions
{
    public class DefaultSQLMetrics : ISQLMetrics
    {
        private readonly ILogger<DefaultSQLMetrics> _logger;
        private object _param;
        private string _sql;
        private Stopwatch _sw;

        public DefaultSQLMetrics(ILogger<DefaultSQLMetrics> logger)
        {
            _sw = Stopwatch.StartNew();
            _logger = logger;
        }

        public void AddToMetrics(string sql, object param)
        {
            _sql = sql;
            _param = param;
        }

        public void Dispose()
        {
            _sw.Stop();
            var es = _sw.ElapsedMilliseconds;
            _logger.LogDebug("SQL EXECUTE Finished in {0} ms,SQL={1},params = {_param}", es, _sql, _param ?? "");
            if (es > 500)
                _logger.LogWarning("SQL EXECUTE Finished in {0} ms,SQL={1},params = {_param}", es, _sql, _param ?? "");
            _sw = null;
            _sql = null;
            _param = null;
        }
    }
}
