using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vulcan.AspNetMvc.Interfaces;


namespace Vulcan.AspNetMvc.Extensions
{
    public static class ResourceExtensions
    {
        public static string GetResourceNameByValue(
          this IList<IResource> rList, string value)
        {
            var res = rList.Where(o => o.Value == value);
            if (res.Count() == 0)
                return null;
            return res.First().Name;
        }
    }
}
