using System;

namespace Vulcan.DapperExtensions.Contract
{
    public interface ISQLMetrics : IDisposable
    {
        void AddToMetrics(string sql, object param);
    }





}
