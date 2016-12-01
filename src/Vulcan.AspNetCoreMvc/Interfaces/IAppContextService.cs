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
        Task<bool> HasPrivilege(string identity, string privilegeCode);

        /// <summary>
        /// Determines whether [is in role] [the specified identity].
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="roleCode">The role code.</param>
        /// <returns></returns>
        Task<bool> IsInRole(string identity, string roleCode);


        /// <summary>
        /// Gets the user information.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <returns></returns>
        Task<AppUser> GetUserInfo(string identity);
    }

    public class AppUser
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string EmployID { get; set; }

        public string GroupCode { get; set; }
        public string GroupName { get; set; }

        public string DeptCode { get; set; }
        public string DeptName { get; set; }

        public string OrgCode { get; set; }
        public string OrgName { get; set; }            
           
    }
}
