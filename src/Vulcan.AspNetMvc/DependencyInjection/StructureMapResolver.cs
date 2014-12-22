using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using System.Web.Http.Dispatcher;
using StructureMap;

namespace Vulcan.AspNetMvc.DependencyInjection
{
    public class StructureMapResolver : StructureMapDependencyScope, IDependencyResolver, IHttpControllerActivator
    {
        private readonly IContainer container;

        public StructureMapResolver(IContainer container)
            : base(container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            this.container = container;

            this.container.Inject(typeof(IHttpControllerActivator), this);
            //this.container.Inject(typeof(System.Web.Mvc.IController), this);
        }

        public IDependencyScope BeginScope()
        {
            return new StructureMapDependencyScope(container.GetNestedContainer());
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            return container.GetNestedContainer().GetInstance(controllerType) as IHttpController;
        }
    }
}
