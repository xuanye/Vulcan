using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Vulcan.AspNetMvc.Config;


namespace Vulcan.AspNetMvc.Extensions
{
    public static class HtmlHelperExtension
    {      
        public static MvcHtmlString Css(this HtmlHelper html, params string[] cssfilename)
        {
            if (cssfilename != null)
            {
                string folderpath = "~/static/css";
                string csslink = "<link href=\"{0}\" rel=\"Stylesheet\" type=\"text/css\" />";
                StringBuilder sb = new StringBuilder();
              
                foreach (string filename in cssfilename)
                {
                     sb.AppendFormat(csslink, UrlHelper.GenerateContentUrl(folderpath + "/" + filename + ".css?v=1", html.ViewContext.HttpContext));
                }

                return MvcHtmlString.Create(sb.ToString());
            }
            return MvcHtmlString.Empty;

        }

        public static MvcHtmlString Js(this HtmlHelper html, params string[] jsKeys)
        {
            if (jsKeys != null)
            {
                string jslink = "<script src='{0}' type='text/javascript'></script>";
                StringBuilder sb = new StringBuilder();
                foreach (string key in jsKeys)
                {
                    var jsurlValue = JsConfig.GetJsUrl(key);
                    if (string.IsNullOrEmpty(jsurlValue))
                        continue;
                    string[] arrUrl = jsurlValue.Split(';');
                    foreach (string jsurl in arrUrl)
                    {
                        sb.AppendFormat(jslink, UrlHelper.GenerateContentUrl(jsurl, html.ViewContext.HttpContext));
                    }
                }
                return MvcHtmlString.Create(sb.ToString());
            }
            return MvcHtmlString.Empty;

        }


    }
}
