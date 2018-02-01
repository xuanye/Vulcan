using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UUAC.Entity;
using Vulcan.DataAccess;

namespace UUAC.Interface.Repository
{
    public interface IRoleRepository: IRepository
    {
        Task<List<IRoleInfo>> QueryRoleByParentCode(string appCode, string pCode);
        Task<bool> CheckCode(string privilegeCode);
        Task<IRoleInfo> GetRole(string code);
        Task<int> RemovRolePrivilegeAsync(string roleCode,string pCode);
        Task<int> RemovRoleUserAsync(string roleCode,string userId);
        Task<int> RemoveRole(string code);
        Task<long> AddRole(IRoleInfo entity);
        Task<int> UpdateRole(IRoleInfo entity);
        Task<List<string>> GetUserRoleCodeList(string appCode, string identity);
        Task<List<IRoleInfo>> QueryUserRoles(string appCode, string userId);
        Task<int> GetMaxRolePoint(string appCode);
        Task UpdateRolePoint(string appCode, int point);
        Task<bool> CheckChildRole(string appCode,string roleCode);
        Task MinusRolePoint(string appCode, int right);
        Task<PagedList<IUserInfo>> QueryRoleUsers(string roleCode, string queryText, PageView page);

        Task<List<IRoleUser>> GetRoleUsers(string roleCode);
        Task<int> AddRoleUsers(List<IRoleUser> newList);
        Task<List<string>> QueryRolePrivilegeList(string appCode, string roleCode);

        Task<int> SaveRolePrivileges(List<IRolePrivilege> plist);
    }
}
