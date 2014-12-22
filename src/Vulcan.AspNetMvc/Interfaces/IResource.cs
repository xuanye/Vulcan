using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulcan.AspNetMvc.Interfaces
{
    public interface IResource
    {
        string Code { get; set; }
        string Name { get; set; }
        string Value { get; set; }
    }
}
