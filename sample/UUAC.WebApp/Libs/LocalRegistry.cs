using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using UUAC.DataAccess.Mysql.Repository;
using UUAC.Interface.Repository;
using Vulcan.AspNetCoreMvc.Interfaces;

namespace UUAC.WebApp.Libs
{
    public static class LocalRegistry
    {
        public static void Registry(IServiceCollection services)
        {
            services.AddSingleton<IAppContextService, AppContextService>();
            services.AddSingleton<IResourceService, ResourceService>();
        }
    }
}
