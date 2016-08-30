using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vulcan.AspNetCoreMvc.Interfaces;

namespace UUAC.WebApp.Libs
{
    public class AppContextService: IAppContextService
    {
        public bool HasPrivilege(string identity, string privilegeCode)
        {
            return false;
        }

        public bool IsInRole(string identity, string roleCode)
        {
            return false;
        }

        public IAppUser GetUserInfo(string identity)
        {
            AppUser user = new AppUser();
            user.UserId = identity;
            user.FullName = "管理员";
            return user;
        }
    }

    public class AppUser : IAppUser
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string EmployID { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
    }
}
