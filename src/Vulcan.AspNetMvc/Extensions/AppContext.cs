using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using Vulcan.AspNetMvc.Common;
using Vulcan.AspNetMvc.Exceptions;
using Vulcan.AspNetMvc.Interfaces;
using Vulcan.AspNetMvc.Utils;

namespace Vulcan.AspNetMvc.Extensions
{
    public class AppContext
    {
        private static IAppContextService _AppContextService;
        private static IAppContextService AppContextService
        {
            get
            {
                if (_AppContextService == null)
                {
                    _AppContextService = ServiceFactory.GetInstance<IAppContextService>();
                }
                return _AppContextService;
            }
        }


        /// <summary>
        /// 当前登录账户的标识
        /// </summary>
        /// <value>The identity.</value>
        public static string Identity
        {
            get
            {
                if (AppConfig.IsDebugMode)
                {
                    return AppConfig.Get("DebuggerUserId");
                }
                string uid = HttpContext.Current.User.Identity.Name;
                if (string.IsNullOrEmpty(uid))
                {
                    throw new NoAuthorizeExecption();
                }
                return uid;
            }
        }
        public static bool IsIsAuthenticated
        {
            get
            {
                if (AppConfig.IsDebugMode)
                {
                    string userid = AppConfig.Get("DebuggerUserId");
                    if (!HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        FormsAuthentication.SetAuthCookie(userid, false);
                    }
                    return true;
                }
                else
                {
                    return HttpContext.Current.User.Identity.IsAuthenticated;
                }
            }
        }
        /// <summary>
        /// 当前登录账户的完整信息
        /// </summary>
        /// <value>The current user.</value>
        public static IUser CurrentUser
        {
            get
            {
                IUser user = null;
                if (HttpContext.Current.Session["UseInfo"] != null)
                {
                    user = HttpContext.Current.Session["UseInfo"] as IUser;

                    if (user == null)
                    {
                        throw new NoAuthorizeExecption();
                    }
                  
                }
                else
                {
                    user = AppContextService.GetUserInfo(Identity);                
                    if (user == null)
                    {
                        throw new NoAuthorizeExecption();
                    }
                    HttpContext.Current.Session["UseInfo"] = user;                    
                }
                return user;
            }
        }

        /// <summary>
        /// 获取用户的IP
        /// </summary>
        /// <value>The user IP.</value>
        public static string UserIP
        {
            get
            {
                return IPUtils.GetWebClientIP();
            }
        }

        //获取本机IP
        public static string ServerIp
        {
            get
            {
                return IPUtils.GetLocalIP();
            }
        }

        public static string ServerMAC
        {
            get
            {
                return IPUtils.GetLocalMAC();
            }
        }
        /// <summary>
        /// 判断当前用户是否在某个角色内
        /// </summary>
        /// <param name="roleCode">角色代码</param>
        /// <returns>
        /// 	<c>true</c> if [is in role] [the specified role code]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInRole(string roleCode)
        {
            return false;
        }
        /// <summary>
        /// 判断是否有权限
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="privilegeCode">The privilege code.</param>
        /// <returns>
        ///   <c>true</c> if the specified privilege code has right; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasPrivilege(string userId, string privilegeCode)
        {         
            if (AppConfig.IsDebugMode)
            {
                return true;
            }
            else
            {
                return AppContextService.HasPrivilege(userId, privilegeCode);
            }

        }

        /// <summary>
        /// 将object类型转换成指定类型，吞掉转换异常等情况
        /// </summary>
        /// <typeparam name="T">需要转换成的类型</typeparam>
        /// <param name="o">需要转换的对象</param>
        /// <returns></returns>
        private static T ObjectToStrongType<T>(object o) where T : class, new()
        {
            T t = null;
            if (o != null)
            {
                t = o as T;
            }
            return t;
        }
    }
}
