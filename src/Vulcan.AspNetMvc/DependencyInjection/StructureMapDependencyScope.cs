using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.Dependencies;
using StructureMap;

namespace Vulcan.AspNetMvc.DependencyInjection
{
    public class StructureMapDependencyScope : IDependencyScope
    {
        private IContainer container;

        public StructureMapDependencyScope(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            if (container == null)
                throw new ObjectDisposedException("this", "This scope has already been disposed.");

            if (serviceType.IsAbstract || serviceType.IsInterface)
            {
                return container.TryGetInstance(serviceType);
            }
            return container.GetInstance(serviceType);        
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (container == null)
                throw new ObjectDisposedException("this", "This scope has already been disposed.");

            return container.GetAllInstances(serviceType).Cast<object>();         
        }

        public void Dispose()
        {
            if (container != null)
                container.Dispose();

            container = null;
        }
    }
}
