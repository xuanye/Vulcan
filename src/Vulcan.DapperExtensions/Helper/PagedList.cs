using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Vulcan.DapperExtensions
{
    [ExcludeFromCodeCoverage]
    public class PagedList<T>
    {
        private List<T> _dataList;

        public List<T> DataList
        {
            get => _dataList ?? (_dataList = new List<T>());
            set => _dataList = value;
        }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int Total { get; set; }
    }
}
