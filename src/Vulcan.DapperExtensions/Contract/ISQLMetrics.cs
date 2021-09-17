using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace Vulcan.DapperExtensions.Contract
{
    public interface ISQLMetrics : IDisposable
    {
        void AddToMetrics(string sql, object param);
    }

   

   

}
