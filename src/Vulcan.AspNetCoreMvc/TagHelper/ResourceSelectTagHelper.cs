using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Vulcan.AspNetCoreMvc.Interfaces;

namespace Vulcan.AspNetCoreMvc.TagHelper
{
    [HtmlTargetElement("select")]
    public class ResourceSelectTagHelper:SelectTagHelper
    {
        private readonly IResourceService _service;
        public ResourceSelectTagHelper(IHtmlGenerator generator,IResourceService service) : base(generator)
        {
            _service = service;
        }

        public string RsCode { get; set; }
        public string RsParentCode { get; set; }
        public bool HasAllOption { get; set; }
        public string SelectedValue { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //处理数据
            var rlist = this._service.GetResourceByCode(RsCode, RsParentCode, HasAllOption);

            List<SelectListItem> selectList = new List<SelectListItem>();
            foreach(var resource in rlist)
            {
                var item = new SelectListItem
                {
                    Value = resource.Value,
                    Text = resource.Name,
                    Selected = resource.Value == SelectedValue
                };
                selectList.Add(item);
            }
            base.Items = selectList; //设置数据

            base.Process(context, output);
        }
    }
}
