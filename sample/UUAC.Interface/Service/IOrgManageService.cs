using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UUAC.Entity;
using UUAC.Entity.DTOEntities;

namespace UUAC.Interface.Service
{
    public interface IOrgManageService
    {
        Task<List<IOrganization>> QueryAllOrgList();

        Task<List<IOrganization>> QueryOrgListByParentCode(string pcode);
        IOrganization GetOrgInfo(string orgCode);
        Task<bool> CheckOrgCode(string id, string orgCode);
        Task<int> SaveOrgInfo(IOrganization entity, int type);
        Task<int> RemoveOrgInfo(string orgCode);
        Task<List<IOrganization>> QueryOrgTreeByParentCode(string pcode);
    }
}
