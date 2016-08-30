using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using UUAC.Common;
using UUAC.DataAccess.Mysql.Repository;
using UUAC.Interface.Repository;

namespace UUAC.DataAccess.Mysql
{
    public static class RepositoryRegistry
    {

        public static void Registry(IServiceCollection services)
        {
            services.AddSingleton<IPrivilegeRepository, PrivilegeRepository>();
            services.AddSingleton<IAppManageRepository, AppManageRepository>();
            services.AddSingleton<IOrgManageRepository, OrgManageRepository>();

            services.AddSingleton<IUserManageRepository, UserManageRepository>();


            services.AddSingleton<IRoleRepository, RoleRepository>();

        }
    }
}
