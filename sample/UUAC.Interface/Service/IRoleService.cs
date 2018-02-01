using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UUAC.Entity;
using Vulcan.DataAccess;

namespace UUAC.Interface.Service
{
    public interface IRoleService
    {
        Task<List<IRoleInfo>> QueryRoleByParentCode(string appCode, string pCode);
        Task<bool> CheckCode(string id, string appCode, string privilegeCode);
        Task<IRoleInfo> GetRole(string id);
        Task<int> SaveRole(IRoleInfo entity, int type);
        Task<int> RemoveRole(string id);
        Task<List<IRoleInfo>> QueryUserTopRole(string appCode, string userId);
        Task<List<string>> GetUserRoleCodeList(string appCode, string userId);
        Task<bool> IsInRole(string identity, string roleCode);
        Task<PagedList<IUserInfo>> QueryRoleUsers(string roleCode, string queryText, PageView page);

        Task<int> AddRoleUserBatch(string roleCode, string userIds);

        Task<int> RomveRoleUser(string roleCode, string useId);

        Task<List<string>> QueryRolePrivilegeList(string appCode, string roleCode);
        Task<int> SaveRolePrivileges(string roleCode, List<IRolePrivilege> plist);
    }
}
