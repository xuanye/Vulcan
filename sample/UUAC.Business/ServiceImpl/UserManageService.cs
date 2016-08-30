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
using Vulcan.Core.Enities;
using Vulcan.Core.Exceptions;

namespace UUAC.Business.ServiceImpl
{
    public class UserManageService: IUserManageService
    {
        private readonly IUserManageRepository _repo;
        private readonly IOrgManageRepository _orgRepo;
        public UserManageService(IUserManageRepository repo,IOrgManageRepository orgRepo)
        {
            this._repo = repo;
            this._orgRepo = orgRepo;
        }

        public Task<PagedList<IUserInfo>> QueryUserList(string orgCode, string qText, PageView view)
        {
            orgCode = Utility.ClearSafeStringParma(orgCode);
            qText = Utility.ClearSafeStringParma(qText);

            return _repo.QueryUserList(orgCode, qText, view);
        }

        public Task<IUserInfo> GetUserInfo(string userId)
        {
            userId = Utility.ClearSafeStringParma(userId);
            return _repo.GetUserInfo(userId);
        }

        public Task<bool> CheckUserId(string id, string userId)
        {
            if (id == userId)
            {
                return Task.FromResult<bool>(true);
            }
            return _repo.CheckUserId(userId);
        }

        public async Task<int> SaveUserInfo(DtoUserInfo entity, int type)
        {
            using (ConnectionScope scope = new ConnectionScope())
            {
                if (type == 1) // 新增
                {
                    // 校验
                    bool result = await this._repo.CheckUserId(entity.UserUid);
                    if (!result)
                    {
                        return -1;
                    }

                    IOrganization pOrg = await _orgRepo.GetOrgInfoAsync(entity.OrgCode);
                    if (pOrg == null)
                    {
                        throw new BizException("父组织不存在，请检查后重新保存");
                    }

                    entity.OrgName = pOrg.OrgName;
                 
                    if(entity.AccountType == 0) //外部用户
                    {
                        if (string.IsNullOrEmpty(entity.Password))
                        {
                            throw new BizException("初始密码不能为空");
                        }

                        entity.Password = CryptographyManager.Md5Encrypt(entity.UserUid + entity.Password);
                    }
                    else
                    {
                        entity.Password = "";
                    }
                  
                    await this._repo.AddUser(entity);
                    return 1;

                }
                else
                {
                    return await this._repo.UpdateUser(entity);
                }
            }
        }

        public async Task<int> RemoveUserInfo(string userId)
        {
            int ret;
            using (TransScope scope = new TransScope())
            {
                userId = Utility.ClearSafeStringParma(userId);

                //清除用户和角色的关系 
                await this._repo.RemoveUserRolesAsync(userId);

                ret = await this._repo.RemoveUserInfoAsync(userId);
                scope.Complete();
                
            }
            return ret;
        }
    }
}
