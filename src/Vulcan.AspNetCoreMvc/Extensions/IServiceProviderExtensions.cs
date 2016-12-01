using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vulcan.AspNetCoreMvc.Extensions
{
    public static class IServiceProviderExtensions
    {
        public static T GetService<T>(this IServiceProvider provider)
        {
            return (T)provider.GetService(typeof(T));
        }
    }
}
