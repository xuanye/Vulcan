

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for api_auth
-- ----------------------------
DROP TABLE IF EXISTS `api_auth`;
CREATE TABLE `api_auth` (
  `idx` int(11) NOT NULL AUTO_INCREMENT,
  `app_code` varchar(50) NOT NULL COMMENT ' /* comment truncated */ /*系统标识*/',
  `api_code` varchar(50) NOT NULL COMMENT ' /* comment truncated */ /*客户端访问id*/',
  `secret_key` varchar(50) NOT NULL COMMENT ' /* comment truncated */ /*密钥*/',
  `client_name` varchar(50) NOT NULL COMMENT ' /* comment truncated */ /*客户端名称--调用方系统名*/',
  `link_user` varchar(50) NOT NULL COMMENT ' /* comment truncated */ /*对方联系人*/',
  `allow_ips` varchar(2000) DEFAULT NULL COMMENT ' /* comment truncated */ /*允许访问的IP列表，用逗号分隔*/',
  `status` tinyint(4) DEFAULT NULL COMMENT ' /* comment truncated */ /*状态  0  = 无效  1= 有效*/',
  `channel` tinyint(4) NOT NULL DEFAULT '1' COMMENT ' /* comment truncated */ /*渠道-扩展字段 */',
  `last_modify_user_id` varchar(50) NOT NULL COMMENT ' /* comment truncated */ /*最后一次更新人账号*/',
  `last_modify_user_name` varchar(200) NOT NULL COMMENT ' /* comment truncated */ /*最后一次更新人名称*/',
  `last_modify_time` datetime NOT NULL COMMENT ' /* comment truncated */ /*最后一次更新时间*/',
  PRIMARY KEY (`idx`),
  KEY `fk_api_auth_app_info1_idx` (`app_code`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT=' /* comment truncated */ /*接口授权表*/';

-- ----------------------------
-- Records of api_auth
-- ----------------------------

-- ----------------------------
-- Table structure for app_info
-- ----------------------------
DROP TABLE IF EXISTS `app_info`;
CREATE TABLE `app_info` (
  `app_code` varchar(50) NOT NULL,
  `app_name` varchar(200) NOT NULL,
  `description` varchar(2000) DEFAULT NULL,
  `last_modify_user_id` varchar(50) NOT NULL COMMENT ' /* comment truncated */ /*最后一次更新人账号*/',
  `last_modify_user_name` varchar(200) NOT NULL COMMENT ' /* comment truncated */ /*最后一次更新人名称*/',
  `last_modify_time` datetime NOT NULL COMMENT ' /* comment truncated */ /*最后一次更新事件*/',
  PRIMARY KEY (`app_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT=' /* comment truncated */ /*应用信息表*/';

-- ----------------------------
-- Records of app_info
-- ----------------------------
INSERT INTO `app_info` VALUES ('UUAC', '统一用户和授权管理系统', '统一用户和授权管理系统', 'admin', '超级管理员', '2016-07-08 15:49:46');

-- ----------------------------
-- Table structure for organization
-- ----------------------------
DROP TABLE IF EXISTS `organization`;
CREATE TABLE `organization` (
  `org_code` varchar(50) NOT NULL,
  `org_name` varchar(200) NOT NULL,
  `parent_code` varchar(50) DEFAULT NULL,
  `remark` varchar(500) DEFAULT NULL,
  `sequence` int(11) NOT NULL DEFAULT '0',
  `org_type` tinyint(4) NOT NULL DEFAULT '0' COMMENT ' /* comment truncated */ /*组织类型 ： 0 组  1 部门  2 中心  3 公司*/',
  `unit_code` varchar(50) DEFAULT NULL COMMENT ' /* comment truncated */ /*所属中心*/',
  `unit_name` varchar(200) DEFAULT NULL COMMENT ' /* comment truncated */ /*所属中心名称*/',
  `last_modify_user_id` varchar(50) NOT NULL COMMENT ' /* comment truncated */ /*最后一次更新人账号*/',
  `last_modify_user_name` varchar(200) NOT NULL COMMENT ' /* comment truncated */ /*最后一次更新人名称*/',
  `last_modify_time` datetime NOT NULL COMMENT ' /* comment truncated */ /*最后一次更新事件*/',
  PRIMARY KEY (`org_code`),
  UNIQUE KEY `OrgCode_UNIQUE` (`org_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT=' /* comment truncated */ /*组织机构表*/';

-- ----------------------------
-- Records of organization
-- ----------------------------
INSERT INTO `organization` VALUES ('CAOFG', 'CAO分管', null, '备注内容很实际', '13', '4', 'CAOFG', 'CAO分管', 'admin', 'admin', '2016-08-12 09:46:03');
INSERT INTO `organization` VALUES ('GAOCENG', '公司高层', null, null, '1', '1', 'GAOCENG', '公司高层', 'admin', '超级管理员', '2016-08-02 11:09:35');
INSERT INTO `organization` VALUES ('JSBZZX', '技术保障中心', 'TECHNOLOGY', '技术保障中心', '1', '3', null, null, 'admin', 'admin', '2016-08-12 09:48:07');
INSERT INTO `organization` VALUES ('OPERATION', '运营体系', null, null, '11', '4', 'OPERATION', '运营体系', 'admin', 'admin', '2016-08-05 15:58:50');
INSERT INTO `organization` VALUES ('PINGPAIGUANLI', '品牌管理', null, null, '12', '4', 'PINGPAIGUANLI', '品牌管理', 'admin', 'admin', '2016-08-11 09:36:07');
INSERT INTO `organization` VALUES ('PUBLIC', '公共服务', null, null, '1', '1', 'PUBLIC', '公共服务', 'admin', '超级管理员', '2016-08-02 11:08:39');
INSERT INTO `organization` VALUES ('TECHNOLOGY', '技术体系', null, null, '10', '4', null, null, 'admin', '超级管理员', '2016-08-05 15:55:22');

-- ----------------------------
-- Table structure for privilege
-- ----------------------------
DROP TABLE IF EXISTS `privilege`;
CREATE TABLE `privilege` (
  `privilege_code` varchar(100) NOT NULL COMMENT ' /* comment truncated */ /*权限标识*/',
  `privilege_name` varchar(200) NOT NULL COMMENT ' /* comment truncated */ /*权限名称*/',
  `privilege_type` tinyint(4) NOT NULL DEFAULT '0' COMMENT ' /* comment truncated */ /*权限类型： 0 菜单权限  1 一般权限*/',
  `remark` varchar(500) DEFAULT NULL COMMENT ' /* comment truncated */ /*备注*/',
  `parent_code` varchar(100) DEFAULT NULL COMMENT ' /* comment truncated */ /*父权限标识*/',
  `resource` varchar(2000) DEFAULT NULL COMMENT ' /* comment truncated */ /*资源  一般为URL*/',
  `sequence` int(11) NOT NULL DEFAULT '0' COMMENT ' /* comment truncated */ /*排序*/',
  `app_code` varchar(50) NOT NULL,
  `last_modify_user_id` varchar(50) NOT NULL COMMENT ' /* comment truncated */ /*最后一次更新人账号*/',
  `last_modify_user_name` varchar(200) NOT NULL COMMENT ' /* comment truncated */ /*最后一次更新人名称*/',
  `last_modify_time` datetime NOT NULL COMMENT ' /* comment truncated */ /*最后一次更新事件*/',
  `mark` varchar(100) DEFAULT NULL COMMENT ' /* comment truncated */ /*标识-一般用于菜单图标什么*/',
  PRIMARY KEY (`privilege_code`,`app_code`),
  UNIQUE KEY `PrivilegeCode_UNIQUE` (`privilege_code`),
  KEY `fk_privilege_AppInfo1_idx` (`app_code`),
  CONSTRAINT `fk_privilege_AppInfo1` FOREIGN KEY (`app_code`) REFERENCES `app_info` (`app_code`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT=' /* comment truncated */ /*权限信息表*/';

-- ----------------------------
-- Records of privilege
-- ----------------------------
INSERT INTO `privilege` VALUES ('UUAC_AppManage', '应用系统管理', '1', '', 'UUAC_SystemManage', '/App/List', '20', 'UUAC', 'admin', '超级管理员', '2016-07-08 15:54:23', null);
INSERT INTO `privilege` VALUES ('UUAC_OrgManage', '部门管理', '1', '', 'UUAC_SystemManage', '/Org/List', '0', 'UUAC', 'admin', '超级管理员', '2016-07-08 15:54:26', null);
INSERT INTO `privilege` VALUES ('UUAC_PrivilegeManage', '权限管理', '1', '', 'UUAC_SystemManage', '/Privilege/List', '30', 'UUAC', 'admin', '超级管理员', '2016-07-08 15:57:54', null);
INSERT INTO `privilege` VALUES ('UUAC_RoleManage', '角色管理', '1', '', 'UUAC_SystemManage', '/Role/RoleList', '40', 'UUAC', 'admin', '超级管理员', '2016-07-08 15:57:57', null);
INSERT INTO `privilege` VALUES ('UUAC_SystemManage', '系统管理', '1', '', '', '', '0', 'UUAC', 'admin', '超级管理员', '2016-07-08 15:58:00', null);
INSERT INTO `privilege` VALUES ('UUAC_UserManage', '用户管理', '1', '', 'UUAC_SystemManage', '/UserList', '10', 'UUAC', 'admin', '超级管理员', '2016-07-08 15:58:04', null);

-- ----------------------------
-- Table structure for role_info
-- ----------------------------
DROP TABLE IF EXISTS `role_info`;
CREATE TABLE `role_info` (
  `role_code` varchar(100) NOT NULL COMMENT ' /* comment truncated */ /*角色代码*/',
  `role_name` varchar(200) NOT NULL COMMENT ' /* comment truncated */ /*角色名称*/',
  `parent_code` varchar(100) DEFAULT NULL COMMENT ' /* comment truncated */ /*父角色代码*/',
  `is_system_role` tinyint(4) NOT NULL COMMENT ' /* comment truncated */ /*是否系统角色*/',
  `remark` varchar(500) DEFAULT NULL COMMENT ' /* comment truncated */ /*备注*/',
  `role_path` varchar(3000) DEFAULT NULL COMMENT ' /* comment truncated */ /*角色层级路径*/',
  `app_code` varchar(50) NOT NULL,
  `last_modify_user_id` varchar(50) NOT NULL COMMENT ' /* comment truncated */ /*最后一次更新人账号*/',
  `last_modify_user_name` varchar(200) NOT NULL COMMENT ' /* comment truncated */ /*最后一次更新人名称*/',
  `last_modify_time` datetime NOT NULL COMMENT ' /* comment truncated */ /*最后一次更新事件*/',
  PRIMARY KEY (`role_code`,`app_code`),
  UNIQUE KEY `RoleCode_UNIQUE` (`role_code`),
  KEY `fk_roleinfo_AppInfo1_idx` (`app_code`),
  CONSTRAINT `fk_roleinfo_AppInfo1` FOREIGN KEY (`app_code`) REFERENCES `app_info` (`app_code`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT=' /* comment truncated */ /*角色信息表*/';

-- ----------------------------
-- Records of role_info
-- ----------------------------
INSERT INTO `role_info` VALUES ('UUAC_admin', '管理员', '', '1', '管理员', '.UUAC_admin.', 'UUAC', 'admin', '系统管理员', '2016-07-08 15:48:06');
INSERT INTO `role_info` VALUES ('UUAC_AppAdmin', '应用系统管理员', '', '0', '应用系统管理员', '.UUAC_AppAdmin.', 'UUAC', 'admin', '系统管理员', '2016-07-08 15:48:10');
INSERT INTO `role_info` VALUES ('UUAC_everyone', '所有人', '', '1', '所有人', '.UUAC_everyone.', 'UUAC', 'admin', '系统管理员', '2016-07-08 15:48:13');
INSERT INTO `role_info` VALUES ('UUAC_UserManage', '用户管理', 'UUAC_admin', '0', '用户管理', '.UUAC_admin.UUAC_UserManage.', 'UUAC', 'admin', '系统管理员', '2016-07-08 15:48:15');

-- ----------------------------
-- Table structure for role_privilege_relation
-- ----------------------------
DROP TABLE IF EXISTS `role_privilege_relation`;
CREATE TABLE `role_privilege_relation` (
  `privilege_code` varchar(100) NOT NULL,
  `role_code` varchar(100) NOT NULL,
  PRIMARY KEY (`privilege_code`,`role_code`),
  KEY `fk_Privilege_has_RoleInfo_Privilege1_idx` (`privilege_code`),
  KEY `REF_RPR_ROLE_idx` (`role_code`),
  CONSTRAINT `REF_RPR_ROLE` FOREIGN KEY (`role_code`) REFERENCES `role_info` (`role_code`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `REF_RPR_PRIVILEGE` FOREIGN KEY (`privilege_code`) REFERENCES `privilege` (`privilege_code`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT=' /* comment truncated */ /*角色权限关系表*/';

-- ----------------------------
-- Records of role_privilege_relation
-- ----------------------------
INSERT INTO `role_privilege_relation` VALUES ('UUAC_AppManage', 'UUAC_admin');
INSERT INTO `role_privilege_relation` VALUES ('UUAC_OrgManage', 'UUAC_admin');
INSERT INTO `role_privilege_relation` VALUES ('UUAC_PrivilegeManage', 'UUAC_admin');
INSERT INTO `role_privilege_relation` VALUES ('UUAC_RoleManage', 'UUAC_admin');
INSERT INTO `role_privilege_relation` VALUES ('UUAC_SystemManage', 'UUAC_admin');
INSERT INTO `role_privilege_relation` VALUES ('UUAC_UserManage', 'UUAC_admin');

-- ----------------------------
-- Table structure for role_user_relation
-- ----------------------------
DROP TABLE IF EXISTS `role_user_relation`;
CREATE TABLE `role_user_relation` (
  `role_code` varchar(100) NOT NULL COMMENT ' /* comment truncated */ /*角色账号*/',
  `user_uid` varchar(50) NOT NULL COMMENT ' /* comment truncated */ /*用户账号*/',
  PRIMARY KEY (`role_code`,`user_uid`),
  KEY `fk_RoleInfo_has_UserInfo_RoleInfo1_idx` (`role_code`),
  KEY `REF_RUR_USER_idx` (`user_uid`),
  CONSTRAINT `REF_RUR_USER` FOREIGN KEY (`user_uid`) REFERENCES `user_info` (`user_uid`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `REF_RUR_ROLE` FOREIGN KEY (`role_code`) REFERENCES `role_info` (`role_code`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT=' /* comment truncated */ /*角色用户关系表*/';

-- ----------------------------
-- Records of role_user_relation
-- ----------------------------
INSERT INTO `role_user_relation` VALUES ('UUAC_admin', 'admin');

-- ----------------------------
-- Table structure for user_info
-- ----------------------------
DROP TABLE IF EXISTS `user_info`;
CREATE TABLE `user_info` (
  `user_uid` varchar(50) NOT NULL COMMENT ' /* comment truncated */ /*用户账号唯一标识*/',
  `full_name` varchar(200) NOT NULL COMMENT ' /* comment truncated */ /*用户姓名*/',
  `password` varchar(200) DEFAULT NULL COMMENT ' /* comment truncated */ /*密码*/',
  `account_type` tinyint(4) NOT NULL DEFAULT '0' COMMENT ' /* comment truncated */ /*账号类型 ：0 自建OR外部  1 内部员工  9 系统*/',
  `org_code` varchar(50) NOT NULL COMMENT ' /* comment truncated */ /*所属结构代码*/',
  `org_name` varchar(200) NOT NULL COMMENT ' /* comment truncated */ /*所属机构名称*/',
  `is_admin` tinyint(4) NOT NULL COMMENT ' /* comment truncated */ /*是否当前机构管理员*/',
  `sequence` int(11) NOT NULL DEFAULT '0' COMMENT ' /* comment truncated */ /*排序号*/',
  `status` tinyint(4) NOT NULL DEFAULT '0' COMMENT ' /* comment truncated */ /*账号状态： 0 正常 1 停用*/',
  `last_modify_user_id` varchar(50) NOT NULL COMMENT ' /* comment truncated */ /*最后一次更新人账号*/',
  `last_modify_user_name` varchar(200) NOT NULL COMMENT ' /* comment truncated */ /*最后一次更新人名称*/',
  `last_modify_time` datetime NOT NULL COMMENT ' /* comment truncated */ /*最后一次更新事件*/',
  `user_num` varchar(50) DEFAULT NULL COMMENT ' /* comment truncated */ /*工号*/',
  `gender` tinyint(4) DEFAULT NULL COMMENT ' /* comment truncated */ /*1：男，0：女*/',
  PRIMARY KEY (`user_uid`),
  UNIQUE KEY `UserUID_UNIQUE` (`user_uid`),
  KEY `fk_userinfo_organization_idx` (`org_code`),
  CONSTRAINT `fk_userinfo_organization` FOREIGN KEY (`org_code`) REFERENCES `organization` (`org_code`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT=' /* comment truncated */ /*用户信息表*/';

-- ----------------------------
-- Records of user_info
-- ----------------------------
INSERT INTO `user_info` VALUES ('admin', '超级管理员', null, '0', 'PUBLIC', '', '1', '10', '1', 'admin', 'admin', '2016-08-17 10:55:01', '000001', '1');
INSERT INTO `user_info` VALUES ('test', '测试人员', '000102030405060708090a0b0c0d0e0f', '0', 'JSBZZX', '技术保障中心', '1', '11', '1', 'admin', 'admin', '2016-08-17 10:55:35', '112334', null);
