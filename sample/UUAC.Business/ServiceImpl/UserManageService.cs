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
using Vulcan.DataAccess;

namespace UUAC.Business.ServiceImpl
{
    public class UserManageService: IUserManageService
    {
        private readonly IUserManageRepository _repo;
        private readonly IOrgManageService _orgService;
        public UserManageService(IUserManageRepository repo, IOrgManageService orgService)
        {
            this._repo = repo;
            this._orgService = orgService;
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

        public async Task<int> SaveUserInfo(DtoUserInfo entity, int type,string viewRootCode)
        {
            using (var scope = this._repo.BeginConnectionScope())
            {
                bool inView = await this._orgService.CheckOrgCodeInView(entity.OrgCode, viewRootCode);
                if (!inView)
                {
                    throw new BizException("没有相应的权限");
                }

                if (type == 1) // 新增
                {
                    // 校验
                    bool result = await this._repo.CheckUserId(entity.UserUid);
                    if (!result)
                    {
                        return -1;
                    }
                }
                IOrganization pOrg = await this._orgService.GetOrgInfo(entity.OrgCode);
                if (pOrg == null)
                {
                    throw new BizException("父组织不存在，请检查后重新保存");
                }

                entity.OrgName = pOrg.OrgName;

                if (string.IsNullOrEmpty(entity.ViewRootCode))
                {
                    entity.ViewRootCode = entity.OrgCode;
                    entity.ViewRootName = entity.OrgName;
                }
                else if (entity.ViewRootCode == "000000")
                {
                    entity.ViewRootName = "根组织";
                }
                else
                {
                    IOrganization rOrg = await this._orgService.GetOrgInfo(entity.ViewRootCode);
                    if (rOrg == null)
                    {
                        throw new BizException("组织范围顶组织代码不存在，请检查后重试");
                    }
                    entity.ViewRootName = rOrg.OrgName;
                }

                if (type == 1) // 新增
                {
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
            using (var scope = this._repo.BeginTransScope())
            {
                userId = Utility.ClearSafeStringParma(userId);

                //清除用户和角色的关系
                await this._repo.RemoveUserRolesAsync(userId);

                ret = await this._repo.RemoveUserInfoAsync(userId);
                scope.Complete();

            }
            return ret;
        }

        public Task<List<IOrganization>> QueryOrgTreeByParentCode(string parentCode)
        {
            parentCode = Utility.ClearSafeStringParma(parentCode);
            return this._orgService.QueryOrgTreeByParentCode(parentCode);
        }

        public Task<List<IUserInfo>> QueryUserListByParentCode(string parentCode)
        {
            parentCode = Utility.ClearSafeStringParma(parentCode);
            return this._repo.QueryUserListByParentCode(parentCode);
        }

        public async Task<int> CheckLogin(string userId, string password)
        {
            var user =await _repo.GetUserOnlyWithPwd(userId);
            if(user ==null)
            {
                return -1;
            }

            string hspass = CryptographyManager.Md5Encrypt(userId + password);

            return hspass.Equals(user.Password, StringComparison.OrdinalIgnoreCase) ? 0 : -2;
        }
    }
}
