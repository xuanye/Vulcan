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
    public class PrivilegeRepository: BaseRepository,IPrivilegeRepository
    {
        public PrivilegeRepository(IConnectionManagerFactory factory,
            IOptions<DBOption> Option,
            ILoggerFactory loggerFactory) : base(factory, Option.Value.Master, loggerFactory)
        {

        }


        public async Task<List<IPrivilege>> QueryUserPrivilegeList(string appCode,string userId,int type)
        {
            string sql = @"SELECT DISTINCT A.privilege_code as PrivilegeCode,A.privilege_name as PrivilegeName,A.privilege_type as PrivilegeType,A.parent_code as ParentCode,A.mark as Mark,A.resource as Resource,A.sequence as Sequence FROM privilege A
                            INNER JOIN role_privilege_relation B on A.privilege_code = B.privilege_code 
                            INNER JOIN role_user_relation C on B.role_code = C.role_code
                           WHERE  A.app_code = @AppCode";
    

            if (type > 0)
            {
                sql += " and A.privilege_type=" + type;
            }

            sql += " ORDER BY A.sequence ASC";
            var param = new
            {
                AppCode = appCode,
                UserUID = userId,
                EveryoneRoleCode = RuleHelper.GetAppEveryoneRuleCode(appCode)
            };
            var list = await base.QueryAsync<Privilege>(sql, param);
            return list.ToList<IPrivilege>();
        }

        public async Task<List<IPrivilege>> QueryPrivilegeByParentCode(string appCode, string pCode)
        {
            string sql = @"SELECT A.privilege_code as PrivilegeCode,A.privilege_name as PrivilegeName,
                            A.privilege_type as PrivilegeType,A.parent_code as ParentCode,A.mark as Mark
                            ,A.resource as Resource,A.app_code as AppCode,A.sequence as Sequence,B.HasChild FROM privilege A 
                            LEFT JOIN 
                            (SELECT (COUNT(1)>0) as HasChild ,parent_code FROM privilege group by parent_code) B 
                            ON A.privilege_code = B.parent_code
                            WHERE A.app_code =@AppCode ";

            if (string.IsNullOrEmpty(pCode))
            {
                sql += " AND IFNULL(A.parent_code,'') = ''";
            }
            else
            {
                sql += " AND A.parent_code = @ParentCode";
            }

            sql += " Order By A.sequence";
            var list = await base.QueryAsync<Privilege>(sql, new { AppCode = appCode, ParentCode = pCode });

            return list.ToList<IPrivilege>();
        }

        public async Task<IPrivilege> GetPrivilege(string code)
        {
            string sql = @"SELECT A.privilege_code as PrivilegeCode,A.privilege_name as PrivilegeName,A.privilege_type as PrivilegeType
                            ,A.parent_code as ParentCode,A.mark as Mark,A.resource as Resource,A.sequence as Sequence,B.privilege_name as ParentName,
                            A.app_code as AppCode,A.last_modify_user_id as LastModifyUserId, A.last_modify_time as LastModifyTime, A.last_modify_user_name as LastModifyUserName,
                            C.app_name as AppName 
                            FROM privilege A
                            LEFT JOIN privilege B ON A.parent_code = B.privilege_code
                            LEFT JOIN app_info C ON A.app_code = C.app_code 
                            WHERE A.privilege_code = @PrivilegeCode";

           return await base.GetAsync<Privilege>(sql, new { PrivilegeCode = code });
        }

        public async Task<bool> CheckCode(string privilegeCode)
        {
            string sql = "select 1 FROM privilege where privilege_code =@PrivilegeCode";

            int ret = await base.GetAsync<int>(sql, new { PrivilegeCode = privilegeCode });

            return ret == 0;
        }

        public Task<long> AddPrivilege(IPrivilege entity)
        {
            var p = Map(entity);

            return base.InsertAsync(p);
        }

        public Task<int> UpdatePrivilege(IPrivilege entity)
        {
            var p = Map(entity);

            p.RemoveUpdateColumn("AppCode");
            return base.UpdateAsync(p);
        }

        public Task<int> RemovRolePrivilegeAsync(string code)
        {
            string sql = "DELETE FROM role_privilege_relation WHERE privilege_code = @PrivielegeCode";

            return base.ExcuteAsync(sql, new { PrivielegeCode = code });

            
        }

        public Task<int> RemovePrivilege(string code)
        {
            string sql = "DELETE FROM privilege WHERE privilege_code = @PrivielegeCode";

            return base.ExcuteAsync(sql, new { PrivielegeCode = code });
        }

        private static Privilege Map(IPrivilege source)
        {
            Privilege p = new Privilege
            {
                PrivilegeCode = source.PrivilegeCode,
                PrivilegeName = source.PrivilegeName,
                PrivilegeType = source.PrivilegeType,
                AppCode = source.AppCode,
                ParentCode = source.ParentCode,
                Mark = source.Mark,
                LastModifyTime = source.LastModifyTime,
                LastModifyUserId = source.LastModifyUserId,
                LastModifyUserName = source.LastModifyUserName,
                Remark = source.Remark,
                Resource = source.Resource,
                Sequence = source.Sequence
            };


            return p;
        }

        public Task<List<string>> QueryUserPrivilegeCodeList(string appCode, string identity, int pType)
        {
            string sql = @"select distinct a.privilege_code from privilege a
                            inner join role_privilege_relation b on a.privilege_code = b.privilege_code
                            inner join role_user_relation c on b.role_code = c.role_code
                            where c.user_uid = @UserId and a.app_code=@AppCode";
            if (pType >= 0)
            {
                sql += " and a.privilege_type=" + pType;
            }
            return base.QueryAsync<string>(sql, new { UserId = identity, AppCode = appCode });
        }

        public async Task<List<IPrivilege>> QueryPrivilegeList(string appCode)
        {
            string sql = @"SELECT A.privilege_code as PrivilegeCode,A.privilege_name as PrivilegeName,
                            A.privilege_type as PrivilegeType,A.parent_code as ParentCode,A.mark as Mark
                            ,A.resource as Resource,A.app_code as AppCode,A.sequence as Sequence FROM privilege A  
                            WHERE A.app_code =@AppCode Order By A.sequence";

          
            var list = await base.QueryAsync<Privilege>(sql, new { AppCode = appCode });

            return list.ToList<IPrivilege>();
        }
    }
}
