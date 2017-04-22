using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using UUAC.Business.ServiceImpl;
using UUAC.Common;

using UUAC.Interface.Service;

namespace UUAC.Business
{
    public static class ServiceRegistry
    {

        public static void Registry(IServiceCollection services)
        {
            services.AddSingleton<IPrivilegeService, PrivilegeService>();
            services.AddSingleton<IAppManageService, AppManageService>();
            services.AddSingleton<IOrgManageService, OrgManageService>();
            services.AddSingleton<IUserManageService, UserManageService>();

            services.AddSingleton<IRoleService, RoleService>();
            services.AddSingleton<ISystemService, SystemService>();
        }
    }
}
