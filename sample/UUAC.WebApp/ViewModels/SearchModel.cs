using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UUAC.WebApp.ViewModels
{
    public class SearchOrgModel: SearchModel
    {
        public string pcode { get; set; }
    }

    public class SearchUserModel : SearchModel
    {
        public string orgCode { get; set; }

        public string qText { get; set; }
    }

    public class SearchRoleModel : SearchModel
    {
        public string roleCode { get; set; }

        public string queryText { get; set; }
    }

    public class SearchPrivilegeModel: SearchModel
    {
        public string pCode { get; set; }
        public string appCode { get; set; }
    }


}
