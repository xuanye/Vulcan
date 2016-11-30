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
            using (ConnectionScope scope = new ConnectionScope())
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
                        return -1;
                    }

                    await this._repo.AddRole(entity);
                    return 1;

                }
                else
                {
                    return await this._repo.UpdateRole(entity);
                }
            }
        }

        public async Task<int> RemoveRole(string code)
        {
            int ret;
            using (TransScope scope = new TransScope())
            {
                code = Utility.ClearSafeStringParma(code);

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
            var list = this._contextAccessor.HttpContext.Session.GetObjectFromJson<List<string>>(rCacheKey);
            if (list == null)
            {
                // 获取用户的所有权限
                list = await this.GetUserRoleCodeList(Constans.APP_CODE, identity);
                this._contextAccessor.HttpContext.Session.SetObjectAsJson(rCacheKey, list);
            }
            return list.Exists(x => x == roleCode);
        }
    }
}
