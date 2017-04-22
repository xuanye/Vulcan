using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UUAC.WebApp.ViewModels
{
    public class SearchAppInfoModel: SearchModel
    {
        public string AppCode { get; set; }
        public string AppName { get; set; }
    }

    public class SearchModel
    {
        public int page { get; set; }
        public int rp { get; set; }
        public string sortname { get; set; }
        public string sortorder { get; set; }
        public string colkey { get; set; }
        public string colsinfo { get; set; }

        public string[] colsArray
        {
            get
            {
                if (string.IsNullOrEmpty(colsinfo))
                {
                    return new string[] { };
                }else
                {
                    return colsinfo.Split(',');
                }
            }
        }

        public int checkall { get; set; }
    }
}
