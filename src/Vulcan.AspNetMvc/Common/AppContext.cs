using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Vulcan.AspNetMvc
{
    internal class AppContext
    {
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
                    throw new Exception("请先登录！");
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
    }
}
