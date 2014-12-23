using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vulcan.AspNetMvc.Interfaces;

namespace Vulcan.AspNetMvc.Sample.Core.Impl
{
    public class ResourceService : IResourceService
    {
        public List<IResource> GetResourceByCode(string code)
        {
            return GetResourceByCode(code, false);
        }

        public List<IResource> GetResourceByCode(string code, bool hasAllOption)
        {
            return GetResourceByCode(code,"" , false);
        }

        public List<IResource> GetResourceByCode(string code, string parentCode, bool hasAllOption)
        {
            return new List<IResource>();
        }
    }
}