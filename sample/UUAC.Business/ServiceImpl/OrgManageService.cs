using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UUAC.Common;
using UUAC.Entity;
using UUAC.Entity.DTOEntities;
using UUAC.Interface.Repository;
using UUAC.Interface.Service;
using Vulcan.Core;
using Vulcan.Core.Exceptions;

namespace UUAC.Business.ServiceImpl
{
    public class OrgManageService: IOrgManageService
    {
        private readonly IOrgManageRepository _repo;
        public OrgManageService(IOrgManageRepository repo)
        {
            _repo = repo;
        }
        public Task<List<IOrganization>> QueryAllOrgList()
        {
            return _repo.QueryAllOrgList();
        }
        public Task<List<IOrganization>> QueryOrgTreeByParentCode(string pcode)
        {
            pcode = Utility.ClearSafeStringParma(pcode);
            if (string.IsNullOrEmpty(pcode))
            {
                return _repo.QueryRootOrgTree();
            }
            else
            {
                return _repo.QueryOrgTreeByParentCode(pcode);
            }
        }

        public Task<List<IOrganization>> QueryOrgListByParentCode(string pcode)
        {
            pcode = Utility.ClearSafeStringParma(pcode);
            if (string.IsNullOrEmpty(pcode))
            {
                return _repo.QueryRootOrgList();
            }
            else
            {
                return _repo.QueryOrgListByParentCode(pcode);
            }
        }

        public IOrganization GetOrgInfo(string orgCode)
        {
            orgCode = Utility.ClearSafeStringParma(orgCode);
            return _repo.GetOrgInfo(orgCode);
        }

        public Task<bool> CheckOrgCode(string id, string orgCode)
        {
            if(id == orgCode)
            {
                return Task.FromResult<bool>(true);
            }
            return _repo.CheckOrgCode(orgCode);
        }

        public async Task<int> SaveOrgInfo(IOrganization entity, int type)
        {
            using (ConnectionScope scope = new ConnectionScope())
            {
                if (type == 1) // 新增
                {
                    // 校验
                    bool result = await this._repo.CheckOrgCode(entity.OrgCode);
                    if (!result)
                    {
                        return -1;
                    }

                    if (string.IsNullOrEmpty(entity.ParentCode) || entity.OrgType == (sbyte)OrgType.Division || entity.OrgType == (sbyte)OrgType.Company) // 根组织
                    {
                        entity.UnitCode = entity.OrgCode;
                        entity.UnitName = entity.OrgName;
                    }
                    else
                    {
                        IOrganization pOrg = await _repo.GetOrgInfoAsync(entity.ParentCode);
                        if(pOrg == null)
                        {
                            throw new BizException("父组织不存在，请检查后重新保存");
                        }

                        entity.UnitCode = pOrg.UnitCode;
                        entity.UnitName = pOrg.UnitName;
                    }

                    await this._repo.AddOrg(entity);
                    return 1;
                   
                }
                else
                {
                    return await this._repo.UpdateOrg(entity);
                }
            }
        }

        public async Task<int> RemoveOrgInfo(string orgCode)
        {
            using (ConnectionScope scope = new ConnectionScope())
            {
                orgCode = Utility.ClearSafeStringParma(orgCode);

                bool checkChild = await _repo.CheckChildOrg(orgCode);
                if (!checkChild)
                {
                    throw new BizException("该组织下存在下属组织，不能删除");
                }

                bool checkUser = await _repo.CheckOrgUser(orgCode);
                if (!checkUser)
                {
                    throw new BizException("该组织下存在用户，不能删除");
                }

                return await _repo.RemoveOrgInfo(orgCode);
            }
        }

       
    }
}
