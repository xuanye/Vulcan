using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UUAC.Common;
using UUAC.DataAccess.Mysql.Entitis;
using UUAC.Entity;
using UUAC.Interface.Repository;
using Vulcan.DataAccess;

namespace UUAC.DataAccess.Mysql.Repository
{
    public class AppManageRepository: BaseRepository,IAppManageRepository
    {
        public AppManageRepository(IConnectionManagerFactory factory,
            IOptions<DBOption> Option,
            ILoggerFactory loggerFactory) : base(factory, Option.Value.Master, loggerFactory)
        {
        }



        public Task<List<IAppInfo>> QueryAppInfoList(string appCode, string appName)
        {
            string sql = "select app_code as AppCode,app_name as AppName,description as Description from app_info where 1=1";

            if (!string.IsNullOrEmpty(appCode))
            {
                sql += " and app_code like '%" + appCode + "%'";
            }
            if (!string.IsNullOrEmpty(appName))
            {
                sql += " and app_name like '%" + appName + "%'";
            }

            return base.QueryAsync<AppInfo>(sql, null).ContinueWith<List<IAppInfo>>(x => x.Result.ToList<IAppInfo>());
        }

        public IAppInfo GetAppInfo(string appCode)
        {
            string sql = "select app_code as AppCode,app_name as AppName,description as Description from app_info where app_code=@AppCode";

            return base.Get<AppInfo>(sql, new { AppCode = appCode });
        }

        public Task<int> CheckAppCodeExists(string appCode)
        {
            string sql = "select 1 from app_info where  app_code=@AppCode";
            return base.GetAsync<int>(sql, new { AppCode = appCode });
        }

        public async Task<int> AddAppInfo(IAppInfo appinfo)
        {
            AppInfo entity = ConvertToEntity(appinfo);

            long ret= await base.InsertAsync(entity);

            return (int)ret;
            //string sql = "insert into app_info(app_code,app_name,description,last_modify_userid,last_modify_username,last_modify_time) Values ()"

        }

        public Task<int> UpdateAppInfo(IAppInfo appinfo)
        {
            AppInfo entity = ConvertToEntity(appinfo);

            return base.UpdateAsync(entity);
        }

        public Task<int> RemoveAppInfo(string appCode)
        {
            string sql = "DELETE FROM app_info WHERE app_code=@AppCode";

            return base.ExcuteAsync(sql, new { AppCode = appCode });
        }


        private AppInfo ConvertToEntity(IAppInfo appinfo)
        {
            AppInfo entity = new AppInfo
            {
                AppCode = appinfo.AppCode,
                AppName = appinfo.AppName,
                Description = appinfo.Description,
                LastModifyTime = appinfo.LastModifyTime,
                LastModifyUserId = appinfo.LastModifyUserId,
                LastModifyUserName = appinfo.LastModifyUserName
            };


            return entity;
        }

        public Task<List<IAppInfo>> GetUserViewApp(string userId)
        {
            string sql = @"select distinct a.app_code as AppCode,a.app_name as AppName from app_info a
                            inner join role_info b on b.app_code = a.app_code
                            inner join role_user_relation c on b.role_code=c.role_code
                            where c.user_uid=@UserId";
            return base.QueryAsync<AppInfo>(sql, new { UserId = userId }).ContinueWith<List<IAppInfo>>(x => x.Result.ToList<IAppInfo>());
        }
    }
}
