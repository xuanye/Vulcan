using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UUAC.DataAccess.Mysql.Entitis;
using UUAC.Entity;
using UUAC.Interface.Repository;
using Vulcan.Core.Enities;

namespace UUAC.DataAccess.Mysql.Repository
{
    public class RoleRepository:BaseRepository, IRoleRepository
    {
        public async Task<List<IRoleInfo>> QueryRoleByParentCode(string appCode, string pCode)
        {
            string sql = @"select a.role_code as RoleCode,a.role_name as RoleName, a.parent_code as ParentCode,a.is_system_role as IsSystemRole,a.remark as Remark,a.app_code as AppCode,
                        a.last_modify_user_id as LastModifyUserId,a.last_modify_time as LastModifyTime,a.last_modify_user_name as LastModifyUserName,b.HasChild as HasChild 
                        from role_info a
                        left join (select (count(1)>0) as HasChild ,parent_code from role_info group by parent_code) b on a.role_code=b.parent_code
                        where a.app_code = @AppCode ";

            if (string.IsNullOrEmpty(pCode))
            {
                sql += " AND IFNULL(a.parent_code,'') = ''";
            }
            else
            {
                sql += " AND a.parent_code = @ParentCode";
            }

          
            var list = await base.QueryAsync<RoleInfo>(sql, new { AppCode = appCode, ParentCode = pCode });

            return list.ToList<IRoleInfo>();
        }

        public async Task<bool> CheckCode(string code)
        {
            string sql = "SELECT 1 FROM role_info where role_code =@RoleCode";

            int ret = await base.GetAsync<int>(sql, new { RoleCode = code });

            return ret == 0;
        }

        public async Task<IRoleInfo> GetRole(string code)
        {
            string sql = @"select a.role_code as RoleCode,a.role_name as RoleName, a.parent_code as ParentCode,a.is_system_role as IsSystemRole,a.remark as Remark,a.app_code as AppCode,
                        a.`left` as `Left`,a.`right` as `Right`,
                        a.last_modify_user_id as LastModifyUserId,a.last_modify_time as LastModifyTime,a.last_modify_user_name as LastModifyUserName,b.app_name as AppName,c.role_name as ParentName
                        from role_info a
                        left join app_info b on a.app_code = b.app_code
                        left join role_info c on a.parent_code = c.role_code
                        where a.role_code = @RoleCode";

            return await base.GetAsync<RoleInfo>(sql, new { RoleCode = code });
        }

        public Task<int> RemovRolePrivilegeAsync(string code)
        {
            string sql = "DELETE FROM role_privilege_relation WHERE role_code = @RoleCode";
            return base.ExcuteAsync(sql, new { RoleCode = code });
        }

        public Task<int> RemovRoleUserAsync(string code)
        {
            string sql = "DELETE FROM role_user_relation WHERE role_code = @RoleCode";
            return base.ExcuteAsync(sql, new { RoleCode = code });
        }

        public Task<int> RemoveRole(string code)
        {
            string sql = "DELETE FROM role_info WHERE role_code = @RoleCode";
            return base.ExcuteAsync(sql, new { RoleCode = code });
        }

        public Task<long> AddRole(IRoleInfo entity)
        {
            var data = Map(entity);
            return base.InsertAsync(data);
        }

        public Task<int> UpdateRole(IRoleInfo entity)
        {
            var data = Map(entity);
            data.RemoveUpdateColumn("AppCode");
            data.RemoveUpdateColumn("IsSystemRole");
            data.RemoveUpdateColumn("ParentCode");
            return base.UpdateAsync(data);

        }


        private static RoleInfo Map(IRoleInfo source)
        {
            return new RoleInfo()
            {
                AppCode = source.AppCode,
                IsSystemRole = source.IsSystemRole,
                LastModifyTime = source.LastModifyTime,
                LastModifyUserId = source.LastModifyUserId,
                LastModifyUserName = source.LastModifyUserName,
                ParentCode = source.ParentCode,
                Remark= source.Remark,
                RoleCode = source.RoleCode,
                RoleName = source.RoleName,
                Left = source.Left,
                Right = source.Right
            };
             
        }

        public Task<List<string>> GetUserRoleCodeList(string appCode, string identity)
        {
            string sql = @"select distinct a.role_code from role_info a 
inner join role_user_relation b on a.role_code =b.role_code
where a.app_code=@AppCode and b.user_uid =@UserId";

            return base.QueryAsync<string>(sql, new { AppCode = appCode, UserId = identity });
        }

        public async Task<List<IRoleInfo>> QueryUserRoles(string appCode, string userId)
        {
            string sql = @"select b.role_code as RoleCode,b.role_name as RoleName,IFNULL(c.childCount,0)>0 as HasChild,b.parent_code as ParentCode,
                        b.is_system_role as IsSystemRole,b.`left` as `Left`,b.`right` as `Right` from role_user_relation a
                            inner join role_info b on a.role_code= b.role_code
                            left join (select count(1) as childCount,parent_code from role_info where app_code='UUAC' group by parent_code) c on b.role_code = c.parent_code
                            where b.app_code=@AppCode and a.user_uid=@UserId";

            var list = await base.QueryAsync<RoleInfo>(sql, new { AppCode = appCode, UserId = userId });

            return list.ToList<IRoleInfo>();
        }

        public Task<int> GetMaxOrgPoint(string appCode)
        {
            string sql = @"select max(`right`) from role_info where app_code =@AppCode";

            return base.GetAsync<int>(sql, new { AppCode = appCode });
        }

        public Task UpdateRolePoint(string appCode,int point)
        {
            string sql = "update role_info set `right` = `right`+2 where app_code=@AppCode and  `right`>=@Point; update role_info set `left` = `left`+2 where app_code=@AppCode and `left`>@Point;";
            return base.ExcuteAsync(sql, new { AppCode =appCode, Point = point });
        }

        public async Task<bool> CheckChildRole(string appCode,string roleCode)
        {
            string sql = "select 1 from role_info where app_code =@AppCode and parent_code =@RoleCode";
            int ret = await base.GetAsync<int>(sql, new { AppCode = appCode, RoleCode = roleCode });

            return ret > 0;
        }

        public Task MinusRolePoint(string appCode, int point)
        {
            string sql = "update role_info set `right` = `right`-2 where app_code=@AppCode and  `right`>=@Point ; update role_info set `left` = `left`-2 where app_code=@AppCode and `left`>@Point;";
            return base.ExcuteAsync(sql, new { AppCode = appCode, Point = point });
        }

        public async Task<PagedList<IUserInfo>> QueryRoleUsers(string roleCode, string queryText, PageView page)
        {
            string cols = @"a.user_uid as UserUid,a.full_name as FullName,a.org_code as OrgCode,a.org_name as OrgName,a.status as Status,a.user_num as UserNum";
            string from = "user_info a inner join role_user_relation b on a.user_uid = b.user_uid";
            string where = " and b.role_code = @RoleCode";
            if (!string.IsNullOrEmpty(queryText))
            {
                where += " and (a.user_uid like '%" + queryText + "%' or a.full_name like '%" + queryText + "%')";
            }

            var list = await base.PagedQueryAsync<UserInfo>(page, cols, from, where, new { RoleCode = roleCode }, "a.user_uid", "");

            PagedList<IUserInfo> plist = new PagedList<IUserInfo>();
            plist.DataList = list.DataList.ToList<IUserInfo>();
            plist.PageIndex = list.PageIndex;
            plist.PageSize = list.PageSize;
            plist.Total = list.Total;
            return plist;
        }
    }
}
