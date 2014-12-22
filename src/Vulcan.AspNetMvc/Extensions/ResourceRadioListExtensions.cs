using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Vulcan.AspNetMvc.Common;
using Vulcan.AspNetMvc.Interfaces;

namespace Vulcan.AspNetMvc.Extensions
{
    public static class ResourceRadioListExtensions
    {
        public static MvcHtmlString ResourceRadioList(
           this HtmlHelper html, string name, string resourceName, object selectedValue, object htmlAttributes)
        {
            var resource = ServiceFactory.GetInstance<IResourceService>();
            var rlist = resource.GetResourceByCode(resourceName);

            if (rlist != null)
            {
                StringBuilder sb = new StringBuilder();
                // put a similar foreach here
                foreach (var item in rlist)
                {
                    sb.Append("<label>");
                    sb.Append(html.RadioButton(name, item.Code, item.Value == selectedValue.ToString()));
                    sb.AppendFormat("{0}</label>",item.Name);
                }
                return new MvcHtmlString(sb.ToString());
               
            }
            return new MvcHtmlString("");          
        }
    }
}
