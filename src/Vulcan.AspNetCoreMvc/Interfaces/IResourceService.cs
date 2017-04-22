using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vulcan.AspNetCoreMvc.Interfaces
{
    public interface IResourceService
    {
        List<IResource> GetResourceByCode(string code);

        List<IResource> GetResourceByCode(string code, bool hasAllOption);
        List<IResource> GetResourceByCode(string code, string parentCode, bool hasAllOption);
    }
    public interface IResource
    {
        string Code { get; set; }
        string Name { get; set; }
        string Value { get; set; }
    }
}
