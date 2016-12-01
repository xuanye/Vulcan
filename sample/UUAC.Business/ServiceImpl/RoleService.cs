using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UUAC.Common;
using UUAC.Entity;
using UUAC.Interface.Repository;
using UUAC.Interface.Service;
using Vulcan.Core;
using Vulcan.AspNetCoreMvc.Extensions;
using Vulcan.Core.Exceptions;

namespace UUAC.Business.ServiceImpl
{
    public class RoleService:IRoleService
    {

        private readonly IRoleRepository _repo;
        private readonly IHttpContextAccessor _contextAccessor;
        public RoleService(IRoleRepository repo, IHttpContextAccessor httpContextAccessor)
        {
            this._repo = repo;
            this._contextAccessor = httpContextAccessor;
        }
        public Task<List<IRoleInfo>> QueryRoleByParentCode(string appCode, string pCode)
        {
            appCode = Utility.ClearSafeStringParma(appCode);
            pCode = Utility.ClearSafeStringParma(pCode);

            return this._repo.QueryRoleByParentCode(appCode, pCode);
        }

        public Task<bool> CheckCode(string id, string appCode, string privilegeCode)
        {
            if (!privilegeCode.StartsWith(appCode))
            {
                privilegeCode = appCode + "_" + privilegeCode;
            }

            if (id == privilegeCode)
            {
                return Task.FromResult<bool>(true);
            }
            return this._repo.CheckCode(privilegeCode);
        }

        public Task<IRoleInfo> GetRole(string code)
        {
            code = Utility.ClearSafeStringParma(code);
            return this._repo.GetRole(code);
        }

        public async Task<int> SaveRole(IRoleInfo entity, int type)
        {
            int ret = -1;
            using (TransScope scope = new TransScope())
            {
               
                if (type == 1) // 新增
                {
                  
                    if (!entity.RoleCode.StartsWith(entity.AppCode))
                    {
                        entity.RoleCode = entity.AppCode + "_" + entity.RoleCode;
                    }
                    // 校验
                    bool result = await this._repo.CheckCode(entity.RoleCode);
                    if (!result)
                    {
                        ret = - 1;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(entity.ParentCode)) // 根组织
                        {
                            int maxPos = await _repo.GetMaxOrgPoint(entity.AppCode);
                            entity.Left = maxPos + 1;
                            entity.Right = entity.Left + 1;
                        }
                        else
                        {
                            IRoleInfo pRole = await _repo.GetRole(entity.ParentCode);
                            if (pRole == null)
                            {
                                throw new BizException("父角色不存在，请检查后重新保存");
                            }

                            await _repo.UpdateRolePoint(entity.AppCode,pRole.Right);
                            entity.Left = pRole.Right;
                            entity.Right = entity.Left + 1;
                        }

                        await this._repo.AddRole(entity);
                        ret = 1;
                    }
                }
                else
                {
                    ret = await this._repo.UpdateRole(entity);
                }
                scope.Complete();               
            }
            return ret;
        }

        public async Task<int> RemoveRole(string code)
        {
            int ret;
            using (TransScope scope = new TransScope())
            {
                code = Utility.ClearSafeStringParma(code);

                IRoleInfo role = await this._repo.GetRole(code);
                if(role ==null)
                {
                    throw new BizException("角色不存在，请检查后重试");
                }               
                int diff = role.Right - role.Left + 1;
                if (diff>2)
                {
                    throw new BizException("存在子角色不能删除！");
                }

                await _repo.MinusRolePoint(role.AppCode, role.Right);

                //清除用户和角色的关系 
                await this._repo.RemovRolePrivilegeAsync(code);
                await this._repo.RemovRoleUserAsync(code);


                ret = await this._repo.RemoveRole(code);

                scope.Complete();

            }
            return ret;
        }

        public async Task<List<IRoleInfo>> QueryUserTopRole(string appCode, string userId)
        {
            bool spAdmin = await this.IsInRole(userId, Constans.SUPPER_ADMIN_ROLE);
            if (spAdmin) //如果超级管理员则返回全部顶层角色
            {
                return await this.QueryRoleByParentCode(appCode, "");
            }
            
            var rlist = await _repo.QueryUserRoles(appCode, userId);

            var rl = new List<IRoleInfo>();
            foreach( var role in rlist)
            {
                bool hasParent = rlist.Exists(x => x.Left > role.Left && x.Left < role.Right);
                if (!hasParent)
                {
                    rl.Add(role);
                }
            }
            return rl;

        }

        public Task<List<string>> GetUserRoleCodeList(string appCode, string userId)
        {
            return _repo.GetUserRoleCodeList(appCode, userId);
        }
        private readonly string rCacheKey = Constans.APP_CODE + "_USER_R";
        public async Task<bool> IsInRole(string identity, string roleCode)
        {
            var list = this._contextAccessor.HttpContext.Session.GetObjectFromByteArray<List<string>>(rCacheKey);
            if (list == null)
            {
                // 获取用户的所有权限
                list = await this.GetUserRoleCodeList(Constans.APP_CODE, identity);
                this._contextAccessor.HttpContext.Session.SetObjectAsByteArray(rCacheKey, list);
            }
            return list.Exists(x => x == roleCode);
        }
    }
}
