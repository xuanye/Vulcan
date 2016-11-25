using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UUAC.Interface.Service;
using Vulcan.AspNetCoreMvc.Interfaces;

namespace UUAC.WebApp.Libs
{
    public class AppContextService: IAppContextService
    {
        private readonly ISystemService _service;
        public AppContextService(ISystemService service)
        {
            this._service = service;
        }
        public bool HasPrivilege(string identity, string privilegeCode)
        {
            return false;
        }

        public bool IsInRole(string identity, string roleCode)
        {
            return false;
        }

        public async Task<IAppUser> GetUserInfo(string identity)
        {
            var u = await this._service.GetUserInfo(identity);
           
            if (u == null)
            {
                throw new Vulcan.Core.Exceptions.NoAuthorizeExecption("用户信息不存在，请检查后重试!");
            }
            AppUser user = new AppUser();
            user.UserId = identity;
            user.FullName = u.FullName;
            user.GroupCode = u.OrgCode;
            user.GroupName = u.OrgName;
            user.OrgCode = u.OrgCode;
            user.OrgName = u.OrgName;
            user.ViewRootCode = u.ViewRootCode??u.OrgCode ;
            user.ViewRootName = u.ViewRootName??u.OrgName;
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

        public string ViewRootCode { get; set; }

        public string ViewRootName { get; set; }
    }
}
