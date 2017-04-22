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

namespace UUAC.Business.ServiceImpl
{
    public class AppManageService: IAppManageService
    {
        private readonly IAppManageRepository _repo;
        private readonly IRoleService _rService;
        public AppManageService(IAppManageRepository repo, IRoleService rService)
        {
            this._repo = repo;
            this._rService = rService;
        }
        public Task<List<IAppInfo>> QueryAppInfoList(string appCode, string appName)
        {
            appCode = Utility.ClearSafeStringParma(appCode);
            appName = Utility.ClearSafeStringParma(appName);
            return this._repo.QueryAppInfoList(appCode, appName);
        }

        public async Task<int> SaveAppInfo(IAppInfo appinfo,int type)
        {
            using (ConnectionScope scope = new ConnectionScope())
            {
                if (type == 1) // 新增
                {
                    // 校验
                    int result = await this._repo.CheckAppCodeExists(appinfo.AppCode);
                    if (result > 0)
                    {
                        return -1;
                    }
                    else
                    {
                        await this._repo.AddAppInfo(appinfo);
                        return 1;
                    }
                }
                else
                {
                    return await this._repo.UpdateAppInfo(appinfo);
                }
            }
            
        }

        public Task<int> RemoveAppInfo(string appCode)
        {
            appCode = Utility.ClearSafeStringParma(appCode);
           
            return this._repo.RemoveAppInfo(appCode);
        }

        public IAppInfo GetAppInfo(string appCode)
        {
            appCode = Utility.ClearSafeStringParma(appCode);
            return _repo.GetAppInfo(appCode);
        }

        public async Task<List<IAppInfo>> GetUserViewApp(string userId)
        {
            bool isAdmin = await _rService.IsInRole(userId, Constans.SUPPER_ADMIN_ROLE);
            if (isAdmin)
            {
                return await this.QueryAppInfoList("", "");
            }

            return await _repo.GetUserViewApp(userId);
        }
    }
}
