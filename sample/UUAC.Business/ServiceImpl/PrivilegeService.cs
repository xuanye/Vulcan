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

namespace UUAC.Business.ServiceImpl
{
    public class PrivilegeService: IPrivilegeService
    {
        private readonly IPrivilegeRepository _repo;
        public PrivilegeService(IPrivilegeRepository repo)
        {
            this._repo = repo;
        }
        public List<IPrivilege> QueryUserPrivilegeList(string appCode, string userId, int type)
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
    }
}
