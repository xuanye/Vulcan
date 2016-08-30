using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UUAC.Entity;
using UUAC.Entity.DTOEntities;
using Vulcan.Core.Enities;

namespace UUAC.Interface.Service
{
    public interface IUserManageService
    {
        Task<PagedList<IUserInfo>> QueryUserList(string orgCode, string qText, PageView view);
        Task<IUserInfo> GetUserInfo(string userId);
        Task<bool> CheckUserId(string id, string userId);
        Task<int> SaveUserInfo(DtoUserInfo entity, int type);
        Task<int> RemoveUserInfo(string userId);
    }
}
