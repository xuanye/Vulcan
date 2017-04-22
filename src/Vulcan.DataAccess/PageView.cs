using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vulcan.DataAccess
{
    public class PageView
    {
        public PageView()
        {
        }

        public PageView(int pageIndex,int pageSize)
        {
            this.PageIndex = pageIndex - 1;
            this.PageSize = pageSize;


        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
        public string SortName { get; set; }
        public string SortOrder { get; set; }

        public string GetSqlOrder()
        {
            if (!string.IsNullOrEmpty(SortName) && !string.IsNullOrEmpty(SortOrder))
                return string.Format("ORDER BY {0} {1} ", SortName, SortOrder);
            return string.Empty;
        }
    }
}
