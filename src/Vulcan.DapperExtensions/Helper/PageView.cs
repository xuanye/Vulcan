using System.Diagnostics.CodeAnalysis;

namespace Vulcan.DapperExtensions
{
    [ExcludeFromCodeCoverage]
    public class PageView
    {
        public PageView()
        {
        }

        public PageView(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex - 1;
            PageSize = pageSize;
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
        public string SortName { get; set; }
        public string SortOrder { get; set; }

        public string GetOrderSql()
        {
            if (!string.IsNullOrEmpty(SortName) && !string.IsNullOrEmpty(SortOrder))
                return $"ORDER BY {SortName} {SortOrder} ";
            return string.Empty;
        }
    }
}
