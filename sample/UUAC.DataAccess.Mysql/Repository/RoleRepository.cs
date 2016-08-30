using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UUAC.DataAccess.Mysql.Entitis;
using UUAC.Entity;
using UUAC.Interface.Repository;

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
                RolePath = source.RolePath
            };
             
        }
    }
}
