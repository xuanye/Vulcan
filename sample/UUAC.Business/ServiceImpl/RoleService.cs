using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;
using UUAC.Common;
using UUAC.Entity;
using UUAC.Interface.Repository;
using UUAC.Interface.Service;
using Vulcan.Core;
using Vulcan.Core.Exceptions;
using Vulcan.DataAccess;
using Vulcan.Core.Extensions;
using System;
using UUAC.Entity.DTOEntities;

namespace UUAC.Business.ServiceImpl
{
    public class RoleService : IRoleService
    {
        private readonly IDistributedCache _cache;
        private readonly IRoleRepository _repo;
        public RoleService(IRoleRepository repo, IDistributedCache cache)
        {
            this._repo = repo;
            this._cache = cache;
        }

        public async Task<int> AddRoleUserBatch(string roleCode, string userIds)
        {
            using (var scope = this._repo.BeginTransScope())
            {

                var list = await this._repo.GetRoleUsers(roleCode);

                var newList = new List<IRoleUser>();
                string[] ids = userIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in ids)
                {
                    bool hasExist = list.Exists(m => m.UserUid == item);
                    if (!hasExist)
                    {
                        DtoRoleUser relation = new DtoRoleUser();
                        relation.RoleCode = roleCode;
                        relation.UserUid = item;
                        newList.Add(relation);
                    }
                }
                if (newList.Count > 0)
                {

                    int ret = await this._repo.AddRoleUsers(newList);
                    scope.Complete();
                    return ret;
                }
                return 1;
            }
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

        public Task<List<string>> GetUserRoleCodeList(string appCode, string userId)
        {
            return _repo.GetUserRoleCodeList(appCode, userId);
        }

        public async Task<bool> IsInRole(string identity, string roleCode)
        {
            List<string> list = null;

            var cacheKey = Constans.APP_CODE + "_" + identity + "_R";

            list = _cache.GetObjectFromByteArray<List<string>>(cacheKey);
            if (list == null)
            {
                list = await this.GetUserRoleCodeList(Constans.APP_CODE, identity);
                var option = new DistributedCacheEntryOptions()
                {
                    SlidingExpiration = TimeSpan.FromMinutes(20)
                };
                _cache.SetObjectAsByteArray(cacheKey, list, option);

            }

            return list.Exists(x => x == roleCode);
        }

        public Task<List<IRoleInfo>> QueryRoleByParentCode(string appCode, string pCode)
        {
            appCode = Utility.ClearSafeStringParma(appCode);
            pCode = Utility.ClearSafeStringParma(pCode);

            return this._repo.QueryRoleByParentCode(appCode, pCode);
        }
        public Task<List<string>> QueryRolePrivilegeList(string appCode, string roleCode)
        {
            appCode = Utility.ClearSafeStringParma(appCode);
            roleCode = Utility.ClearSafeStringParma(roleCode);

            return this._repo.QueryRolePrivilegeList(appCode, roleCode);
        }

        public Task<PagedList<IUserInfo>> QueryRoleUsers(string roleCode, string queryText, PageView page)
        {
            queryText = Utility.ClearSafeStringParma(queryText);
            roleCode = Utility.ClearSafeStringParma(roleCode);
            return _repo.QueryRoleUsers(roleCode, queryText, page);
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
            foreach (var role in rlist)
            {
                bool hasParent = rlist.Exists(x => x.Left > role.Left && x.Left < role.Right);
                if (!hasParent)
                {
                    rl.Add(role);
                }
            }
            return rl;
        }

        public async Task<int> RemoveRole(string code)
        {
            int ret;
            using (var scope = this._repo.BeginTransScope())
            {
                code = Utility.ClearSafeStringParma(code);

                IRoleInfo role = await this._repo.GetRole(code);
                if (role == null)
                {
                    throw new BizException("角色不存在，请检查后重试");
                }
                int diff = role.Right - role.Left + 1;
                if (diff > 2)
                {
                    throw new BizException("存在子角色不能删除！");
                }

                await _repo.MinusRolePoint(role.AppCode, role.Right);

                //清除用户和角色的关系
                await this._repo.RemovRolePrivilegeAsync(code, "");
                await this._repo.RemovRoleUserAsync(code, "");

                ret = await this._repo.RemoveRole(code);

                scope.Complete();
            }
            return ret;
        }

        public Task<int> RomveRoleUser(string roleCode, string useId)
        {
            return this._repo.RemovRoleUserAsync(roleCode, useId);
        }

        public async Task<int> SaveRole(IRoleInfo entity, int type)
        {
            int ret = -1;
            using (var scope = this._repo.BeginTransScope())
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
                        ret = -1;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(entity.ParentCode)) // 根组织
                        {                          
                            entity.Left = 1;
                            entity.Right = entity.Left + 1;
                        }
                        else
                        {
                            IRoleInfo pRole = await _repo.GetRole(entity.ParentCode);
                            if (pRole == null)
                            {
                                throw new BizException("父角色不存在，请检查后重新保存");
                            }

                            await _repo.UpdateRolePoint(entity.AppCode, pRole.Right);
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

        public async Task<int> SaveRolePrivileges(string roleCode, List<IRolePrivilege> plist)
        {
            int ret = -1;
            using (var scope = this._repo.BeginTransScope())
            {
                ret = await _repo.RemovRolePrivilegeAsync(roleCode, "");

                if (plist.Count > 0)
                {
                    ret = await _repo.SaveRolePrivileges(plist);
                }
                scope.Complete();
            }
            return ret;
        }
    }
}
