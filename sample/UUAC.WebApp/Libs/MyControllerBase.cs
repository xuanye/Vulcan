using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UUAC.Common;
using Vulcan.AspNetCoreMvc.Interfaces;

namespace UUAC.WebApp.Libs
{
    public class MyControllerBase:Controller
    {
      
        protected string UserId => this.User.Identity.IsAuthenticated ? this.User.Identity.Name : "";

        protected string FullName
        {
            get
            {
                if(this.User.HasClaim(x=>x.Type == MyClaimTypes.FullName))
                {
                    return this.User.FindFirst(MyClaimTypes.FullName).Value;
                }
                return null;
            }
        }
        protected string GroupCode
        {
            get
            {
                if (this.User.HasClaim(x => x.Type == MyClaimTypes.GroupCode))
                {
                    return this.User.FindFirst(MyClaimTypes.GroupCode).Value;
                }
                return null;
            }
        }
        protected string OrgCode
        {
            get
            {
                if (this.User.HasClaim(x => x.Type == MyClaimTypes.OrgCode))
                {
                    return this.User.FindFirst(MyClaimTypes.OrgCode).Value;
                }
                return null;
            }
        }
        protected string ViewRootCode
        {
            get
            {
                if (this.User.HasClaim(x => x.Type == MyClaimTypes.ViewRootCode))
                {
                    return this.User.FindFirst(MyClaimTypes.ViewRootCode).Value;
                }
                return null;
            }
        }
        protected string ViewRootName
        {
            get
            {
                if (this.User.HasClaim(x => x.Type == MyClaimTypes.ViewRootName))
                {
                    return this.User.FindFirst(MyClaimTypes.ViewRootName).Value;
                }
                return null;
            }
        }
    }
}
