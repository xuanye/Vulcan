using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _contextAccessor;
        public AppContextService(ISystemService service, IHttpContextAccessor contextAccessor)
        {
            this._service = service;
            this._contextAccessor = contextAccessor;
        }
        public Task<bool> HasPrivilege(string identity, string privilegeCode)
        {
            return this._service.HasPrivilege(identity, privilegeCode);
            
        }

        public async Task<bool> IsInRole(string identity, string roleCode)
        {
            string roleKey = $"HAS_{roleCode}";
            object hasValue;
            if (_contextAccessor.HttpContext.Items.TryGetValue(roleKey, out hasValue))
            {
                if(hasValue != null){
                    return (bool)hasValue;
                }
            }
            
            var hasRole = await this._service.IsInRole(identity, roleCode);

            _contextAccessor.HttpContext.Items[roleKey] = hasRole;

            return hasRole;
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
