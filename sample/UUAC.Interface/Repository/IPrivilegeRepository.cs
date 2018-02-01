using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UUAC.Entity;

namespace UUAC.Interface.Repository
{
    public interface IPrivilegeRepository: IRepository
    {
        Task<List<IPrivilege>> QueryUserPrivilegeList(string appCode,string userId,int type);
        Task<List<IPrivilege>> QueryPrivilegeByParentCode(string appCode, string pCode);
        Task<IPrivilege> GetPrivilege(string code);
        Task<bool> CheckCode(string privilegeCode);
        Task<long> AddPrivilege(IPrivilege entity);
        Task<int> UpdatePrivilege(IPrivilege entity);
        Task<int> RemovRolePrivilegeAsync(string code);
        Task<int> RemovePrivilege(string code);
        Task<List<string>> QueryUserPrivilegeCodeList(string appCode, string identity, int pType);
        Task<List<IPrivilege>> QueryPrivilegeList(string appCode);
    }
}
