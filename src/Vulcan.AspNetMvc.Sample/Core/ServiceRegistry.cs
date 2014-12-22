using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap.Configuration.DSL;


namespace Vulcan.AspNetMvc.Sample.Core
{
    public class ServiceRegistry : Registry
    {
        public ServiceRegistry()
        {
            For<IHelloService>().Use<HelloService>();
        }
    }
}