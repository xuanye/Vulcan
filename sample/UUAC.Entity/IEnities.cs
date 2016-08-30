using System;


namespace UUAC.Entity
{
    public interface IAppInfo: IEntity
    {
        string AppCode { get; set; }
        string AppName { get; set; }
        string Description { get; set; }
    }
    public interface IPrivilege: IEntity
    {
        string PrivilegeCode { get; set; }
        string PrivilegeName { get; set; }
        sbyte PrivilegeType { get; set; }
        string Remark { get; set; }
        string ParentCode { get; set; }
        string ParentName { get; set; }
        string Resource { get; set; }
        int Sequence { get; set; }
        string AppCode { get; set; }
        string AppName { get; set; }
        string Mark { get; set; }
        bool HasChild { get; set; }
    }

    public interface IUserInfo : IEntity
    {
        string UserUid { get; set; }
        string FullName { get; set; }
        string Password { get; set; }
        sbyte AccountType { get; set; }
        string OrgCode { get; set; }
        string OrgName { get; set; }
        sbyte IsAdmin { get; set; }
        int Sequence { get; set; }
        sbyte Status { get; set; }
        string UserNum { get; set; }
        sbyte? Gender { get; set; }
    }
    public interface IRoleInfo : IEntity
    {
        string RoleCode { get; set; }
        string RoleName { get; set; }
        string ParentCode { get; set; }
        string ParentName { get; set; }
        bool HasChild { get; set; }
        sbyte IsSystemRole { get; set; }
        string Remark { get; set; }
        string RolePath { get; set; }
        string AppCode { get; set; }
        string AppName { get; set; }
    }
    public interface IApiAuth : IEntity
    {
        int Idx { get; set; }
        string ApiCode { get; set; }
        string SecretKey { get; set; }
        string AppCode { get; set; }
        string ClientName { get; set; }
        string LinkUser { get; set; }
        string AllowIps { get; set; }
        sbyte? Status { get; set; }
        sbyte Channel { get; set; }
    }

    public interface IOrganization: IEntity
    {
        string OrgCode { get; set; }
        string OrgName { get; set; }
        string ParentCode { get; set; }
        string ParentName { get; set; }
        string Remark { get; set; }
        int    Sequence { get; set; }
        sbyte  OrgType { set; get; }
        string UnitCode { get; set; }
        string UnitName { get; set; }
        bool HasChild { get; set; }
    }
    public interface IEntity
    {
        string LastModifyUserId { get; set; }
        string LastModifyUserName { get; set; }
        DateTime LastModifyTime { get; set; }
    }
}
