using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vulcan.AspNetCoreMvc.Interfaces
{
    public interface IAppContextService
    {
        /// <summary>
        /// Determines whether the specified identity has privilege.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="privilegeCode">The privilege code.</param>
        /// <returns></returns>
        bool HasPrivilege(string identity, string privilegeCode);

        /// <summary>
        /// Determines whether [is in role] [the specified identity].
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="roleCode">The role code.</param>
        /// <returns></returns>
        bool IsInRole(string identity, string roleCode);


        /// <summary>
        /// Gets the user information.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <returns></returns>
        Task<IAppUser> GetUserInfo(string identity);
    }

    public interface IAppUser
    {
        string UserId { get; }
        string FullName { get; }
        string EmployID { get; }

        string GroupCode { get;  }
        string GroupName { get;  }

        string DeptCode { get;  }
        string DeptName { get;  }

        string OrgCode { get;  }
        string OrgName { get;  }

        string ViewRootCode { get;  }

        string ViewRootName { get;  }
    }
}
