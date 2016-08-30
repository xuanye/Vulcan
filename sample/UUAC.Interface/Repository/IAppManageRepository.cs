using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UUAC.Entity;

namespace UUAC.Interface.Repository
{
    public interface IAppManageRepository
    {
        Task<List<IAppInfo>> QueryAppInfoList(string appCode, string appName);
        IAppInfo GetAppInfo(string appCode);
        Task<int> CheckAppCodeExists(string appCode);
        Task<int> AddAppInfo(IAppInfo appinfo);
        Task<int> UpdateAppInfo(IAppInfo appinfo);
        Task<int> RemoveAppInfo(string appCode);
    }
}
