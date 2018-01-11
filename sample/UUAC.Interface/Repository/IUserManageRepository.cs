using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UUAC.Entity;
using Vulcan.DataAccess;

namespace UUAC.Interface.Repository
{
    public interface IUserManageRepository
    {
        Task<PagedList<IUserInfo>> QueryUserList(string orgCode, string qText, PageView view);
        Task<IUserInfo> GetUserInfo(string userId);
        Task<bool> CheckUserId(string userId);
        Task<long> AddUser(IUserInfo entity);
        Task<int> UpdateUser(IUserInfo entity);
        Task<int> RemoveUserRolesAsync(string userId);
        Task<int> RemoveUserInfoAsync(string userId);
        Task<List<IUserInfo>> QueryUserListByParentCode(string parentCode);
    }
}
