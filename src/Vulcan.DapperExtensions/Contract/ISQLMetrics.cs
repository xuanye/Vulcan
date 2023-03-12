using System;

namespace Vulcan.DapperExtensions.Contract
{
    public interface ISqlMetrics : IDisposable
    {
        void AddToMetrics(string Sql, object param);
    }
}
