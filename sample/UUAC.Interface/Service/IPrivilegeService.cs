using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UUAC.Entity;
using UUAC.Entity.DTOEntities;

namespace UUAC.Interface.Service
{
    public interface IPrivilegeService
    {
        List<IPrivilege> QueryUserPrivilegeList(string appCode, string userId, int type);
        Task<List<IPrivilege>> QueryPrivilegeByParentCode(string appCode, string pCode);
        Task<IPrivilege> GetPrivilege(string code);
        Task<int> SavePrivilege(IPrivilege entity, int type);
        Task<int> RemovePrivilege(string code);
        Task<bool> CheckCode(string id, string appCode,string privilegeCode);
    }
}
