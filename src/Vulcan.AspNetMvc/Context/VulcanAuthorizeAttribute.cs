using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Vulcan.AspNetMvc.Extensions;

namespace Vulcan.AspNetMvc.Context
{
    public class VulcanAuthorizeAttribute : AuthorizeAttribute
    {
        string[] _usersSplit;
        protected string[] UsersSplit
        {
            get
            {
                if (_usersSplit == null)
                {
                    _usersSplit = SplitString(base.Users);
                }
                return _usersSplit;
            }
        }
        string[] _rolesSplit;
        protected string[] RolesSplit
        {
            get
            {
                if (_rolesSplit == null)
                {
                    _rolesSplit = SplitString(base.Roles);
                }
                return _rolesSplit;
            }
        }
        private string _privilegeCode;
        public string PagePrivilegeCode
        {
            get
            {
                return _privilegeCode;
            }
            set
            {
                _privilegeCode = value;
            }
        }

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {

            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }


            if (!AppContext.IsIsAuthenticated)
            {
                httpContext.Response.StatusCode = 401;//无权限状态码
                return false;
            }

            if (UsersSplit.Length > 0 && !UsersSplit.Contains(AppContext.Identity, StringComparer.OrdinalIgnoreCase))
            {
                return false;
            }

            if (RolesSplit.Length > 0 && !RolesSplit.Any(AppContext.IsInRole))
            {
                return false;
            }
           
            //验证页面权限
            if (!string.IsNullOrEmpty(_privilegeCode) && !AppContext.HasPrivilege(AppContext.Identity, _privilegeCode))
            {
                return false;
            }
            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            UrlHelper url = null;
            switch (context.HttpContext.Response.StatusCode)
            {
                case 401:
                    url = new UrlHelper(context.RequestContext);
                    context.Result = new RedirectResult(url.Action("Login", "Home"));
                    break;
                //case 403://Forbidden
                //    break;
                default:
                    url = new UrlHelper(context.RequestContext);
                    context.Result = new RedirectResult(url.Action("NoRight", "Home"));
                    break;

            }

        }

        internal static string[] SplitString(string original)
        {
            if (String.IsNullOrEmpty(original))
            {
                return new string[0];
            }

            var split = from piece in original.Split(',')
                        let trimmed = piece.Trim()
                        where !String.IsNullOrEmpty(trimmed)
                        select trimmed;
            return split.ToArray();
        }
    }
}
