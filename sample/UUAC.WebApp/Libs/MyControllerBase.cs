using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UUAC.Common;
using Vulcan.AspNetCoreMvc.Interfaces;
using UUAC.Interface.Service;
using Vulcan.Core.Exceptions;
using Vulcan.AspNetCoreMvc.Extensions;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace UUAC.WebApp.Libs
{
    public class MyControllerBase : Controller
    {
       
        
        protected string UserId => this.User.Identity.IsAuthenticated ? this.User.Identity.Name : "";

        
        protected async Task<MyAppUser> GetSignedUser()
        {
            
            string userId = this.UserId;
            if (string.IsNullOrEmpty(userId))
            {
                throw new NoAuthorizeExecption("当前没有用户登录或登录状态已过期，请刷新后重试");
            }
            var user = HttpContext.Session.GetObjectFromByteArray<MyAppUser>(Constans.AppUserSessionKey);
            if (user == null)
            {
                var service = HttpContext.RequestServices.GetService<IAppContextService>();

                user = await service.GetUserInfo(userId) as MyAppUser;
                if(user == null)
                {
                    throw new NoAuthorizeExecption("意外错误，用户信息加载异常");
                }
                HttpContext.Session.SetObjectAsByteArray<MyAppUser>(Constans.AppUserSessionKey, user);
            }
            return user;
        }
    }
}
