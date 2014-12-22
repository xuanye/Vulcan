using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Vulcan.AspNetMvc.Common;
using Vulcan.AspNetMvc.Interfaces;


namespace Vulcan.AspNetMvc.Extensions
{
    public static class ResourceDisplayTextExtensions
    {
        public static MvcHtmlString ResourceDisplayText(
        this HtmlHelper html,string resourceName, object selectedValue)
        {
            var resource = ServiceFactory.GetInstance<IResourceService>();
            var rlist = resource.GetResourceByCode(resourceName);
            return new MvcHtmlString(rlist.GetResourceNameByValue(selectedValue.ToString()));
        }
    }
}
