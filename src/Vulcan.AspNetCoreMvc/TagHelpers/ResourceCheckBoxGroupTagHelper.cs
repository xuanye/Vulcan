using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Vulcan.AspNetCoreMvc.Interfaces;
using System.Text;

namespace Vulcan.AspNetCoreMvc.TagHelpers
{
    [HtmlTargetElement("rs-checkbox")]
    public class ResourceCheckBoxGroupTagHelper:TagHelper
    {
        private const string ForAttributeName = "asp-for";


        private readonly IResourceService _service;
        public ResourceCheckBoxGroupTagHelper(IResourceService service)
        {
            _service = service;

        }

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }



        public string RsCode { get; set; }
        public string RsParentCode { get; set; }

        public string ItemClass { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string checkValue = For.Model?.ToString();
            var rlist = this._service.GetResourceByCode(RsCode, RsParentCode, false);

            StringBuilder sb = new StringBuilder();

            foreach (var resource in rlist)
            {
                sb.AppendFormat(@"<div class='checkbox'><label><input type='checkbox' class='{4}' value='{0}' name='{1}' {2}/><span class='lbl'>{3}</span></label></div>",
                    resource.Value,
                    For.Name,
                    resource.Value == checkValue ? "checked" : "",
                    resource.Name,
                    ItemClass
                );
            }

            output.TagName = "div";


            output.Content.SetHtmlContent(sb.ToString());
            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}

