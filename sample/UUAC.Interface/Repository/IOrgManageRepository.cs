using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UUAC.Entity;

namespace UUAC.Interface.Repository
{
    public interface IOrgManageRepository
    {
        Task<List<IOrganization>> QueryAllOrgList();
        Task<List<IOrganization>> QueryRootOrgList();
        Task<List<IOrganization>> QueryOrgListByParentCode(string pcode);
        IOrganization GetOrgInfo(string orgCode);
        Task<bool> CheckOrgCode(string orgCode);
        Task<long> AddOrg(IOrganization entity);
        Task<int> UpdateOrg(IOrganization entity);
        Task<IOrganization> GetOrgInfoAsync(string orgCode);
        Task<int> RemoveOrgInfo(string orgCode);
        Task<bool> CheckChildOrg(string parentCode);
        Task<bool> CheckOrgUser(string orgCode);
        Task<List<IOrganization>> QueryOrgTreeByParentCode(string pcode);
        Task<List<IOrganization>> QueryRootOrgTree();
        Task<bool> CheckOrgCodeInView(string parentCode, int point);
        Task<int> GetMaxOrgPoint();
        Task<int> UpdateOrgPoint(int point);
    }
}
