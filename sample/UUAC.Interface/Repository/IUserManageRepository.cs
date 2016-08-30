using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UUAC.Entity;
using UUAC.Entity.DTOEntities;
using Vulcan.Core.Enities;

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
    }
}
