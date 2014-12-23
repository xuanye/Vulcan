using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vulcan.AspNetMvc.Interfaces;

namespace Vulcan.AspNetMvc
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
        /// <param name="Identity">The identity.</param>
        /// <returns></returns>
        IUser GetUserInfo(string Identity);
    }
}
