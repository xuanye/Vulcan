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
using Vulcan.Core.Exceptions;
using Vulcan.AspNetCoreMvc.Extensions;

namespace UUAC.Business.ServiceImpl
{
    public class PrivilegeService: IPrivilegeService
    {
        private readonly IPrivilegeRepository _repo;
        private readonly IHttpContextAccessor _contextAccessor;
        public PrivilegeService(IPrivilegeRepository repo, IHttpContextAccessor httpContextAccessor)
        {
            this._repo = repo;
            this._contextAccessor = httpContextAccessor;
        }
        public Task<List<IPrivilege>> QueryUserPrivilegeList(string appCode, string userId, int type)
        {
            return _repo.QueryUserPrivilegeList(appCode, userId, type);
        }

        public Task<List<IPrivilege>> QueryPrivilegeByParentCode(string appCode, string pCode)
        {
            appCode = Utility.ClearSafeStringParma(appCode);
            pCode = Utility.ClearSafeStringParma(pCode);
            return _repo.QueryPrivilegeByParentCode(appCode, pCode);
        }

        public Task<IPrivilege> GetPrivilege(string code)
        {
            code = Utility.ClearSafeStringParma(code);
            return _repo.GetPrivilege(code);
        }

        public async Task<int> SavePrivilege(IPrivilege entity, int type)
        {
            using (ConnectionScope scope = new ConnectionScope())
            {
                if (type == 1) // 新增
                {
                    if (!entity.PrivilegeCode.StartsWith(entity.AppCode))
                    {
                        entity.PrivilegeCode = entity.AppCode + "_" + entity.PrivilegeCode;
                    }
                    // 校验
                    bool result = await this._repo.CheckCode(entity.PrivilegeCode);
                    if (!result)
                    {
                        return -1;
                    }
                   
                    await this._repo.AddPrivilege(entity);
                    return 1;

                }
                else
                {
                    return await this._repo.UpdatePrivilege(entity);
                }
            }
        }

        public async Task<int> RemovePrivilege(string code)
        {
            int ret;
            using (TransScope scope = new TransScope())
            {
                code = Utility.ClearSafeStringParma(code);

                //清除用户和角色的关系 
                await this._repo.RemovRolePrivilegeAsync(code);

                ret = await this._repo.RemovePrivilege(code);
                scope.Complete();

            }
            return ret;
        }

        public Task<bool> CheckCode(string id,string appCode,string privilegeCode)
        {
            if (!privilegeCode.StartsWith(appCode))
            {
                privilegeCode = appCode + "_" + privilegeCode;
            }

            if (id == privilegeCode)
            {
                return Task.FromResult<bool>(true);
            }
            return  this._repo.CheckCode(privilegeCode);
        }

        public Task<List<string>> QueryUserPrivilegeCodeList(string appCode, string identity, int pType)
        {
            return  this._repo.QueryUserPrivilegeCodeList(appCode, identity, pType);
        }
        private readonly string pCacheKey = Constans.APP_CODE + "_USER_P";
        public async Task<bool> HasPrivilege(string identity, string privilegeCode)
        {
            var list = this._contextAccessor.HttpContext.Session.GetObjectFromJson<List<string>>(pCacheKey);
            if (list == null)
            {
                // 获取用户的所有权限
                list = await this._repo.QueryUserPrivilegeCodeList(Constans.APP_CODE, identity, -1);
                this._contextAccessor.HttpContext.Session.SetObjectAsJson(pCacheKey, list);
            }
            return list.Exists(x => x == privilegeCode);
        }
    }
}
