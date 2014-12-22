using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulcan.AspNetMvc.Interfaces
{
    public interface IResourceService
    {
        List<IResource> GetResourceByCode(string code);

        List<IResource> GetResourceByCode(string code, bool hasAllOption);
        List<IResource> GetResourceByCode(string code, string parentCode, bool hasAllOption);
    }
}
