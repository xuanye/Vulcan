using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UUAC.Entity;
using Vulcan.DataAccess;

namespace UUAC.Interface.Repository
{
    public interface IRoleRepository
    {
        Task<List<IRoleInfo>> QueryRoleByParentCode(string appCode, string pCode);
        Task<bool> CheckCode(string privilegeCode);
        Task<IRoleInfo> GetRole(string code);
        Task<int> RemovRolePrivilegeAsync(string code);
        Task<int> RemovRoleUserAsync(string code);
        Task<int> RemoveRole(string code);
        Task<long> AddRole(IRoleInfo entity);
        Task<int> UpdateRole(IRoleInfo entity);
        Task<List<string>> GetUserRoleCodeList(string appCode, string identity);
        Task<List<IRoleInfo>> QueryUserRoles(string appCode, string userId);
        Task<int> GetMaxOrgPoint(string appCode);
        Task UpdateRolePoint(string appCode, int point);
        Task<bool> CheckChildRole(string appCode,string roleCode);
        Task MinusRolePoint(string appCode, int right);
        Task<PagedList<IUserInfo>> QueryRoleUsers(string roleCode, string queryText, PageView page);
    }
}
