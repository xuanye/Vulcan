using System;
using System.Collections.Generic;
using System.Text;

namespace Vulcan.DataAccess
{
    public interface ISQLMetricsFactory
    {
        ISQLMetrics Create();
    }
}
