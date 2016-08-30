using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UUAC.DataAccess.Mysql.Entitis;
using UUAC.Entity;
using UUAC.Entity.DTOEntities;
using UUAC.Interface.Repository;
using Vulcan.Core.Enities;

namespace UUAC.DataAccess.Mysql.Repository
{
    public class UserManageRepository:BaseRepository, IUserManageRepository
    {
        public async Task<PagedList<IUserInfo>> QueryUserList(string orgCode, string qText, PageView view)
        {
            string cols = @"user_uid as UserUid, full_name as FullName, account_type as AccountType,org_code as OrgCode,org_name as OrgName
,is_admin as IsAdmin,sequence as Sequence,`status` as `Status`,last_modify_time as LastModifyTime,last_modify_user_id as LastModifyUserId
,last_modify_user_name as LastModifyUserName,user_num as UserNum , gender as Gender";
            string table = "user_info";
            string condition = "";
            if (!string.IsNullOrEmpty(orgCode))
            {
                condition += " AND org_code='"+orgCode+"'";
            }
            if (!string.IsNullOrEmpty(qText))
            {
                condition += " AND ( user_uid LIKE '%" + qText + "%' OR  full_name LIKE '%"+qText+"%')";
            }


            PagedList<UserInfo> slist = await base.PagedQueryAsync<UserInfo>(view, cols, table, condition, new { }, "user_uid", "Order By sequence");

            PagedList<IUserInfo> rlist = new PagedList<IUserInfo>
            {
                PageIndex = slist.PageIndex,
                PageSize = slist.PageSize,
                Total = slist.Total
            };

            foreach(var user in slist.DataList)
           {
                rlist.DataList.Add(user);
           }
            return rlist;

        }

        public async Task<IUserInfo> GetUserInfo(string userId)
        {
            string sql = @"SELECT user_uid as UserUid, full_name as FullName, account_type as AccountType,org_code as OrgCode,org_name as OrgName
,is_admin as IsAdmin,sequence as Sequence,`status` as `Status`,last_modify_time as LastModifyTime,last_modify_user_id as LastModifyUserId
,last_modify_user_name as LastModifyUserName,user_num as UserNum , gender as Gender FROM user_info WHERE user_uid = @UserUId";

            var user = await base.GetAsync<UserInfo>(sql, new { UserUId = userId });

            return user;
        }

        public async Task<bool> CheckUserId(string userId)
        {
            string sql = "SELECT 1 FROM user_info WHERE user_uid = @UserUId";

            int ret = await base.GetAsync<int>(sql, new { UserUId = userId });

            return ret == 0;
        }

        public Task<long> AddUser(IUserInfo entity)
        {
            var user = Map(entity);

            return base.InsertAsync(user);
        }

        public Task<int> UpdateUser(IUserInfo entity)
        {
            var user = Map(entity);

            if (string.IsNullOrEmpty(entity.Password))
            {
                user.RemoveUpdateColumn("Password");
            }

            return base.UpdateAsync(user);
           
        }

        public Task<int> RemoveUserRolesAsync(string userId)
        {
            string sql = "DELETE FROM role_user_relation where user_uid =@UserUid";

            return base.ExcuteAsync(sql, new { UserUid = userId });
        }

        public Task<int> RemoveUserInfoAsync(string userId)
        {
            string sql = "DELETE FROM user_info where user_uid =@UserUid";

            return base.ExcuteAsync(sql, new { UserUid = userId });
        }

        private static UserInfo Map(IUserInfo source)
        {
            UserInfo user = new UserInfo
            {
                OrgCode = source.OrgCode,
                OrgName = source.OrgName,
                AccountType = source.AccountType,
                Password = source.Password,
                IsAdmin = source.IsAdmin,
                Sequence = source.Sequence,
                UserUid = source.UserUid,
                UserNum = source.UserNum,
                FullName = source.FullName,
                Status = source.Status,
                LastModifyTime = source.LastModifyTime,
                LastModifyUserId = source.LastModifyUserId,
                LastModifyUserName = source.LastModifyUserName
            };



            return user;

        }
    }
}
