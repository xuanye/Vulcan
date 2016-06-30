using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vulcan.Core.Enities
{
    public class PagedList<T>
    {
        private List<T> _dataList;

        public List<T> DataList
        {
            get { return _dataList ?? (_dataList = new List<T>()); }
            set { _dataList = value; }
        }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int Total { get; set; }
    }
}