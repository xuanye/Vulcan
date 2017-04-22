using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UUAC.Entity;
using UUAC.Entity.DTOEntities;

namespace UUAC.Interface.Service
{
    public interface IAppManageService
    {
        Task<List<IAppInfo>> QueryAppInfoList(string appCode, string appName);
        IAppInfo GetAppInfo(string appCode);
        Task<int> SaveAppInfo(IAppInfo appInfo, int type);
        Task<int> RemoveAppInfo(string appCode);
        Task<List<IAppInfo>> GetUserViewApp(string userId);
    }
}
