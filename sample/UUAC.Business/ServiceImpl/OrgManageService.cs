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

        public Task<IOrganization> GetOrgInfo(string orgCode)
        {
            orgCode = Utility.ClearSafeStringParma(orgCode);
            return _repo.GetOrgInfoAsync(orgCode);
        }

        public Task<bool> CheckOrgCode(string id, string orgCode)
        {
            if(id == orgCode)
            {
                return Task.FromResult<bool>(true);
            }
            return _repo.CheckOrgCode(orgCode);
        }

        public async Task<bool> CheckOrgCodeInView(string orgCode, string viewCode)
        {
            if (string.IsNullOrEmpty(orgCode)
                || string.IsNullOrEmpty(viewCode)) // 校验用户是否拥有编辑组织的权限
            {
                return true;
            }

            if(viewCode == "000000")
            {
                return true;
            }

            var rootOrg = await _repo.GetOrgInfoAsync(viewCode);
            if(rootOrg == null)
            {
                throw new BizException("可视范围组织机构不存在，请联系管理员");
            }

            return await _repo.CheckOrgCodeInView(orgCode, rootOrg.Left);
        }


        public async Task<int> SaveOrgInfo(IOrganization entity, int type,string viewRootCode)
        {
            int ret = -1;
            using (var scope = this._repo.BeginTransScope())
            {
                bool inView = await CheckOrgCodeInView(entity.ParentCode, viewRootCode);
                if (!inView)
                {
                    throw new BizException("没有相应的权限");
                }
                if (type == 1) // 新增
                {
                    // 校验
                    bool result = await this._repo.CheckOrgCode(entity.OrgCode);
                    if (!result)
                    {
                       return - 1;
                    }
                    if (string.IsNullOrEmpty(entity.ParentCode) ) // 根组织
                    {
                        entity.UnitCode = entity.OrgCode;
                        entity.UnitName = entity.OrgName;

                        int maxPos = await _repo.GetMaxOrgPoint();
                        entity.Left = maxPos + 1;
                        entity.Right = entity.Left + 1;
                    }
                    else
                    {
                        IOrganization pOrg = await _repo.GetOrgInfoAsync(entity.ParentCode);
                        if(pOrg == null)
                        {
                            throw new BizException("父组织不存在，请检查后重新保存");
                        }

                        if(entity.OrgType == (sbyte)OrgType.Division || entity.OrgType == (sbyte)OrgType.Company)
                        {
                            entity.UnitCode = entity.OrgCode;
                            entity.UnitName = entity.OrgName;
                        }
                        else
                        {
                            entity.UnitCode = pOrg.UnitCode;
                            entity.UnitName = pOrg.UnitName;
                        }
                        // 为新的节点腾出空间来
                        await _repo.UpdateOrgPoint(pOrg.Right);
                        entity.Left = pOrg.Right ;
                        entity.Right = entity.Left + 1;
                    }                                      
                    await this._repo.AddOrg(entity);
                    ret = 1;
                }
                else
                {
                    ret = await this._repo.UpdateOrg(entity);
                }
                scope.Complete();
            }
            return ret;
        }

        public async Task<int> RemoveOrgInfo(string orgCode)
        {
            using (var scope = this._repo.BeginTransScope())
            {
                orgCode = Utility.ClearSafeStringParma(orgCode);

                IOrganization org = await _repo.GetOrgInfoAsync(orgCode);
                if(org == null)
                {
                    throw new BizException("组织结构不存在");
                }
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

                await _repo.MinusOrgPoint(org.Right);


                int ret = await _repo.RemoveOrgInfo(orgCode);
                scope.Complete();
                return ret;
            }
        }

       
    }
}
