using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Vulcan.DataAccess
{
    public interface ISQLMetrics : IDisposable
    {
        void AddToMetrics(string sql, object param);
    }

    public class NullSQLMetrics : ISQLMetrics
    {
        public void AddToMetrics(string sql, object param)
        {
          
        }

        public void Dispose()
        {
           
        }
    }

}
