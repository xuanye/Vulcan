using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vulcan.DapperExtensions
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
                return $"ORDER BY {SortName} {SortOrder} ";
            return string.Empty;
        }
    }
}
