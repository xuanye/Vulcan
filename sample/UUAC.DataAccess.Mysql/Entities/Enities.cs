using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UUAC.Entity;

namespace UUAC.DataAccess.Mysql.Entitis
{
    public partial class AppInfo : IAppInfo
    {
        
    }
    public partial class Privilege : IPrivilege 
    {
        public bool HasChild { get; set; }
        public string ParentName { get; set; }
        public string AppName { get; set; }
    }

    public partial class UserInfo : IUserInfo
    {
       
    }
    public partial class RoleInfo : IRoleInfo
    {
        public string ParentName { get; set; }
        public bool HasChild { get; set; }
        public string AppName { get; set; }
    }
    public partial class ApiAuth : IApiAuth
    {
       
    }

    public partial class Organization : IOrganization
    {
        public string ParentName { get; set; }
        public bool HasChild { get; set; }
    }


    public partial class RoleUserRelation: IRoleUser
    {

    }

    public partial class RolePrivilegeRelation: IRolePrivilege
    {

    }

}
