using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Vulcan.AspNetMvc.Interfaces
{

    public interface IUser
    {
        string UserId { get; }
        string FullName { get; }
        string EmployID { get; }

        string GroupCode { get; set; }
        string GroupName { get; set; }

        string DeptCode { get; set; }
        string DeptName { get; set; }

        string OrgCode { get; set; }
        string OrgName { get; set; }

    }
}
