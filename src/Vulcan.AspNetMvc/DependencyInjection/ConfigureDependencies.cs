using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Vulcan.AspNetMvc.DependencyInjection
{
    public class ConfigureDependencies
    {
        private static IContainer _container;

        private static object _lockObject = new object();

        public static IContainer GetContainer()
        {
            if (_container == null)
            {
                throw new Exception("请先初始化容器");
            }
            return _container;
        }
        private static void CreateContainer()
        {
            if (_container == null)
            {
                lock (_lockObject)
                {
                    if (_container == null)
                    {
                        _container = new Container();
                    }
                }
            }
        }
        public static IContainer InitContainer(List<Registry> rlist)
        {
            CreateContainer();
            _container.Configure(
                x => {
                    foreach (var registry in rlist)
                    {
                        x.AddRegistry(registry);
                    }
                }
            );

            return _container;
        }
    }
}
