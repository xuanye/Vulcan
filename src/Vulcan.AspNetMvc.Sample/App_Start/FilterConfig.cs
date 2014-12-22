using System.Web;
using System.Web.Mvc;

namespace Vulcan.AspNetMvc.Sample
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}