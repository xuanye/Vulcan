using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UUAC.DataAccess.Mysql.Entitis;
using UUAC.Entity;
using UUAC.Interface.Repository;

namespace UUAC.DataAccess.Mysql.Repository
{
    public class OrgManageRepository:BaseRepository, IOrgManageRepository
    {
        public Task<List<IOrganization>> QueryAllOrgList()
        {
            string sql = "SELECT org_code as OrgCode ,org_name as OrgName ,parent_code as ParentCode,unit_code as UnitCode,unit_name as UnitName from organization order by sequence";
            return base.QueryAsync<Organization>(sql,null).ContinueWith<List<IOrganization>>(x => x.Result.ToList<IOrganization>());
        }

        public Task<List<IOrganization>> QueryRootOrgList()
        {
            string sql = @"SELECT org_code as OrgCode ,org_name as OrgName ,remark as Remark,
                        org_type as OrgType,sequence as Sequence,last_modify_user_id as LastModifyUserId ,
                        last_modify_user_name as LastModifyUserName,last_modify_time as LastModifyTime,
                        parent_code as ParentCode,unit_code as UnitCode,unit_name as UnitName from organization where parent_code is null 
                        order by sequence";
            return base.QueryAsync<Organization>(sql, null).ContinueWith<List<IOrganization>>(x => x.Result.ToList<IOrganization>());
        }
        public Task<List<IOrganization>> QueryRootOrgTree()
        {
            string sql = @"SELECT a.org_code as OrgCode ,a.org_name as OrgName ,a.remark as Remark,
			a.org_type as OrgType,a.sequence as Sequence,a.last_modify_user_id as LastModifyUserId ,
			a.last_modify_user_name as LastModifyUserName,a.last_modify_time as LastModifyTime,
			a.parent_code as ParentCode,a.unit_code as UnitCode,a.unit_name as UnitName,b.HasChild
from organization a 
left join (select count(1)>0 as HasChild,parent_code from organization 
group by parent_code) b  on a.org_code = b.parent_code
                        where a.parent_code is null
                        order by a.sequence";
            return base.QueryAsync<Organization>(sql, null).ContinueWith<List<IOrganization>>(x => x.Result.ToList<IOrganization>());
        }
        public Task<List<IOrganization>> QueryOrgListByParentCode(string pcode)
        {
            string sql = @"SELECT org_code as OrgCode ,org_name as OrgName ,remark as Remark,
                        org_type as OrgType,sequence as Sequence,last_modify_user_id as LastModifyUserId ,
                        last_modify_user_name as LastModifyUserName,last_modify_time as LastModifyTime,
                        parent_code as ParentCode,unit_code as UnitCode,unit_name as UnitName from organization 
                        where parent_code=@ParentCode
                        order by sequence";
            return base.QueryAsync<Organization>(sql, new { ParentCode  = pcode }).ContinueWith<List<IOrganization>>(x => x.Result.ToList<IOrganization>());
        }

        public Task<List<IOrganization>> QueryOrgTreeByParentCode(string pcode)
        {
            string sql = @"SELECT a.org_code as OrgCode ,a.org_name as OrgName ,a.remark as Remark,
			a.org_type as OrgType,a.sequence as Sequence,a.last_modify_user_id as LastModifyUserId ,
			a.last_modify_user_name as LastModifyUserName,a.last_modify_time as LastModifyTime,
			a.parent_code as ParentCode,a.unit_code as UnitCode,a.unit_name as UnitName,b.HasChild
from organization a 
left join (select count(1)>0 as HasChild,parent_code from organization 
group by parent_code) b  on a.org_code = b.parent_code
                        where a.parent_code=@ParentCode
                        order by a.sequence";
            return base.QueryAsync<Organization>(sql, new { ParentCode = pcode }).ContinueWith<List<IOrganization>>(x => x.Result.ToList<IOrganization>());
        }
        public IOrganization GetOrgInfo(string orgCode)
        {
            string sql = @"SELECT a.org_code as OrgCode ,a.org_name as OrgName ,a.remark as Remark,
                        a.org_type as OrgType,a.sequence as Sequence,a.last_modify_user_id as LastModifyUserId ,
                        a.last_modify_user_name as LastModifyUserName,a.last_modify_time as LastModifyTime,a.`left` as `Left`,a.`right` as `Right`,
                        a.parent_code as ParentCode,a.unit_code as UnitCode,a.unit_name as UnitName,b.org_name as ParentName from organization a
                        LEFT JOIN organization b on a.parent_code = b.org_code
                        where a.org_code=@OrgCode ";

            return base.Get<Organization>(sql, new { OrgCode = orgCode });
        }
        public async Task<IOrganization> GetOrgInfoAsync(string orgCode)
        {
            string sql = @"SELECT a.org_code as OrgCode ,a.org_name as OrgName ,a.remark as Remark,
                        a.org_type as OrgType,a.sequence as Sequence,a.last_modify_user_id as LastModifyUserId ,
                        a.last_modify_user_name as LastModifyUserName,a.last_modify_time as LastModifyTime,a.`left` as `Left`,a.`right` as `Right`,
                        a.parent_code as ParentCode,a.unit_code as UnitCode,a.unit_name as UnitName,b.org_name as ParentName from organization a
                        LEFT JOIN organization b on a.parent_code = b.org_code
                        where a.org_code=@OrgCode ";

            return await base.GetAsync<Organization>(sql, new { OrgCode = orgCode });
        }

        public async Task<int> RemoveOrgInfo(string orgCode)
        {
            string sql = "DELETE FROM organization where org_code=@OrgCode";

            return await base.ExcuteAsync(sql, new { OrgCode = orgCode });
        }

        public async Task<bool> CheckChildOrg(string parentCode)
        {
            string sql = "SELECT 1 FROM organization where parent_code = @ParentCode";
            int ret = await base.GetAsync<int>(sql, new { ParentCode = parentCode });
            return ret == 0;
        }

        public async Task<bool> CheckOrgUser(string orgCode)
        {
            string sql = "SELECT 1 FROM user_info where org_code = @OrgCode";
            int ret = await base.GetAsync<int>(sql, new { OrgCode = orgCode });
            return ret == 0;
        }

       


        public async Task<bool> CheckOrgCode(string orgCode)
        {
            string sql = "select 1 from organization where org_code = @OrgCode";

            int ret = await base.GetAsync<int>(sql, new { OrgCode = orgCode });

            return ret == 0;
        }

        public Task<long> AddOrg(IOrganization entity)
        {
            return base.InsertAsync(Map(entity));
        }

        public Task<int> UpdateOrg(IOrganization entity)
        {
            var org = Map(entity);

            org.RemoveUpdateColumn("unit_code");
            org.RemoveUpdateColumn("unit_name");

            return base.UpdateAsync(org);
        }

  

        private static Organization Map(IOrganization source)
        {
            Organization org = new Organization
            {
                OrgCode = source.OrgCode,
                OrgName = source.OrgName,
                OrgType = source.OrgType,
                ParentCode = source.ParentCode,
                Remark = source.Remark,
                Sequence = source.Sequence,
                UnitCode = source.UnitCode,
                UnitName = source.UnitName,
                Left = source.Left,
                Right = source.Right,
                LastModifyTime = source.LastModifyTime,
                LastModifyUserId = source.LastModifyUserId,
                LastModifyUserName = source.LastModifyUserName
            };



            return org;

        }

        public async Task<bool> CheckOrgCodeInView(string orgCode, int point)
        {
            string sql = "select 1 from organization where left>@Point and right<@Point";
            int ret = await base.GetAsync<int>(sql, new { Point = point });
            return ret > 0;
        }

        public Task<int> GetMaxOrgPoint()
        {
            string sql = "select max(`right`) from organization";
            return base.GetAsync<int>(sql, null);
        }

        public Task<int> UpdateOrgPoint(int point)
        {         
            string sql = "update organization set `right` = `right`+2 where `right`>=@Point; update organization set `left` = `left`+2 where `left`>@Point;";
            return base.ExcuteAsync(sql, new { Point = point });
        }

        public Task MinusOrgPoint(int point)
        {
            string sql = "update organization set `right` = `right`-2 where `right`>=@Point; update organization set `left` = `left`-2 where `left`>@Point;";
            return base.ExcuteAsync(sql, new { Point = point });
        }
    }
}
