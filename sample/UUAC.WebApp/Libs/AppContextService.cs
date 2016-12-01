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
        public Task<bool> HasPrivilege(string identity, string privilegeCode)
        {
            return this._service.HasPrivilege(identity, privilegeCode);
            
        }

        public Task<bool> IsInRole(string identity, string roleCode)
        {
            return this._service.IsInRole(identity, roleCode);
        }

        public async Task<AppUser> GetUserInfo(string identity)
        {
            var u = await this._service.GetUserInfo(identity);
           
            if (u == null)
            {
                throw new Vulcan.Core.Exceptions.NoAuthorizeExecption("用户信息不存在，请检查后重试!");
            }
            MyAppUser user = new MyAppUser();
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

    public class MyAppUser:AppUser
    {
        public string ViewRootCode { get; set; }
        public string ViewRootName { get; set; }
    }
}
