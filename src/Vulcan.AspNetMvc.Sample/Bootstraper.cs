using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using StructureMap;
using StructureMap.Configuration.DSL;
using Vulcan.AspNetMvc.DependencyInjection;
using Vulcan.AspNetMvc.Sample.Core;

namespace Vulcan.AspNetMvc.Sample
{
    public class Bootstraper
    {
        public static void InitContainer()
        {
            List<Registry> rlist = new List<Registry>();
            rlist.Add(new ServiceRegistry());

            IContainer container =  ConfigureDependencies.InitContainer(rlist);

            //Register for MVC
            DependencyResolver.SetResolver(new StructureMapResolver(container));

            //Register for Web API
            //GlobalConfiguration.Configuration.DependencyResolver = new StructureMapResolver(container);

            //Set ConnectManager
        }
    }
}