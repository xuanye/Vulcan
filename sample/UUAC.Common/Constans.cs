using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UUAC.Common
{
    public class Constans
    {
        public const string EVERYONE_ROLE_POSTFIX = "everyone";
        public const string ADMIN_ROLE_POSTFIX = "admin";

        public const string SUPPER_ADMIN_ROLE = "UUAC_admin";

        public const string APP_CODE = "UUAC";

        public const string MAIN_DB_KEY = "master";

        public const string AuthenticationScheme = "UUAC_COOKIE";

        public const string AppUserSessionKey = "AppUserSessionKey";

     
    }
    public class MyClaimTypes
    {
        public const string FullName = "UUAC.AppUser.FullName";
        public const string OrgCode = "UUAC.AppUser.OrgCode";
        public const string GroupCode = "UUAC.AppUser.GroupCode";
        public const string ViewRootCode = "UUAC.AppUser.ViewRootCode";
        public const string ViewRootName = "UUAC.AppUser.ViewRootName";
        
    }
}
