using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Vulcan.AspNetMvc.Common;
using Vulcan.AspNetMvc.Interfaces;
using Vulcan.AspNetMvc.JsonEntities;


namespace Vulcan.AspNetMvc.Extensions
{
    public static class ResourceDropDownListExtensions
    {
        /// <summary>
        /// Resources the drop down list.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="name">The name.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="selectedValue">The selected value.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString ResourceDropDownList(
          this HtmlHelper html, string name, string resourceName,object selectedValue, object htmlAttributes)
        {
            return ResourceDropDownList(html, name, resourceName, selectedValue, htmlAttributes, false);
        }

        /// <summary>
        /// Resources the drop down list.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="name">The name.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="selectedValue">The selected value.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="hasAllOption"></param>
        /// <returns></returns>
        public static MvcHtmlString ResourceDropDownList(
          this HtmlHelper html, string name, string resourceName, object selectedValue, object htmlAttributes, bool hasAllOption)
        {
            var resource = ServiceFactory.GetInstance<IResourceService>();
            var rlist = resource.GetResourceByCode(resourceName, hasAllOption);
            SelectList list = new SelectList(rlist, "Value", "Name", selectedValue);
            
            return html.DropDownList(name, list, htmlAttributes);
        }
        public static MvcHtmlString ResourceDropDownList(
      this HtmlHelper html, string name, string resourceName, object selectedValue, object htmlAttributes, string parentCode, bool hasAllOption)
        {
            var resource = ServiceFactory.GetInstance<IResourceService>();
            var rlist = resource.GetResourceByCode(resourceName, parentCode, hasAllOption);
            SelectList list = new SelectList(rlist, "Value", "Name", selectedValue);
            return html.DropDownList(name, list, htmlAttributes);
        }

        public static MvcHtmlString DropDownListEx(
          this HtmlHelper html, string name, List<CodeValue> rlist, object selectedValue, object htmlAttributes)
        {
            SelectList list = new SelectList(rlist, "Code", "Value", selectedValue);
            return html.DropDownList(name, list, htmlAttributes);
        }

    }
}
