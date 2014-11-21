using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Vulcan.DataAccess.ORMapping
{
    public class PagedList<T> //where T : class
    {
        private List<T> _dataList;

        public List<T> DataList
        {
            get
            {
                if (_dataList == null) _dataList = new List<T>();
                return _dataList;
            }
            set
            {
                _dataList = value;
            }
        }

        public int PageSize
        {
            get;
            set;
        }

        public int PageIndex
        {
            get;
            set;
        }

        public int Total
        { get; set; }
    }

    public class PageView
    {
        public PageView()
        {
        }

        public PageView(NameValueCollection form)
        {
            this.PageIndex = Convert.ToInt32(form["page"]) - 1;
            this.PageSize = Convert.ToInt32(form["rp"]);
            SortName = form["sortname"];
            SortOrder = form["sortorder"];
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