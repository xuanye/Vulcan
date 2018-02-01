using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UUAC.Entity.DTOEntities
{
    public class DtoAppInfo : IAppInfo
    {
        public string AppCode { get; set; }
        public string AppName { get; set; }
        public string Description { get; set; }

        public string LastModifyUserId { get; set; }
        public string LastModifyUserName { get; set; }
        public DateTime LastModifyTime { get; set; }
    }

    public class DtoOrganization : IOrganization
    {
        public string LastModifyUserId { get; set; }
        public string LastModifyUserName { get; set; }
        public DateTime LastModifyTime { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
        public string ParentCode { get; set; }
        public string ParentName { get; set; }
        public string Remark { get; set; }
        public int Sequence { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public sbyte OrgType { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public bool HasChild { get; set; }
    }

    public class DtoUserInfo : IUserInfo
    {
        public string LastModifyUserId { get; set; }
        public string LastModifyUserName { get; set; }
        public DateTime LastModifyTime { get; set; }
        public string UserUid { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public sbyte AccountType { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }

        public string ViewRootCode{get;set;}
        public string ViewRootName{get;set;}
        public int Sequence { get; set; }
        public sbyte Status { get; set; }
        public string UserNum { get; set; }
        public sbyte? Gender { get; set; }
    }


    public class DtoPrivilege:IPrivilege
    {
        public string LastModifyUserId { get; set; }
        public string LastModifyUserName { get; set; }
        public DateTime LastModifyTime { get; set; }
        public string PrivilegeCode { get; set; }
        public string PrivilegeName { get; set; }
        public sbyte PrivilegeType { get; set; }
        public string Remark { get; set; }
        public string ParentCode { get; set; }
        public string ParentName { get; set; }
        public string AppName { get; set; }
        public string Resource { get; set; }
        public int Sequence { get; set; }
        public string AppCode { get; set; }
        public string Mark { get; set; }
        public bool HasChild { get; set; }
    }

    public class DtoRole : IRoleInfo
    {
        public string LastModifyUserId { get; set; }
        public string LastModifyUserName { get; set; }
        public DateTime LastModifyTime { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string ParentCode { get; set; }
        public string ParentName { get; set; }
        public bool HasChild { get; set; }
        public sbyte IsSystemRole { get; set; }
        public string Remark { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public string AppCode { get; set; }
        public string AppName { get; set; }
    }

    public class DtoRoleUser : IRoleUser
    {
        public string UserUid { get; set; }
        public string RoleCode { get; set; }
    }
    public class DtoRolePrivilege : IRolePrivilege
    {
        public string PrivilegeCode { get; set; }
        public string RoleCode { get; set; }
    }
}
