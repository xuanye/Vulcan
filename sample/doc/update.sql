-- 修改用户表，删除是否组织管理员字段，添加用户可见组织范围字段
ALTER TABLE `unified_user_access_control`.`user_info`
DROP COLUMN `is_admin`,
ADD COLUMN `view_root_code` VARCHAR(50) NULL COMMENT '用户可见组织范围-最高层组织编码' AFTER `gender`,
ADD COLUMN `view_root_name` VARCHAR(200) NULL COMMENT '用户可见组织范围-最高层组织名称' AFTER `view_root_code`;

-- 修改组织结构表，添加树级支持
ALTER TABLE `unified_user_access_control`.`organization` 
ADD COLUMN `left` INT NOT NULL DEFAULT 0 COMMENT '左侧节点-标示' AFTER `last_modify_time`,
ADD COLUMN `right` INT NOT NULL DEFAULT 0 COMMENT '右侧节点标识' AFTER `left`;

-- 修改角色表，添加树级支持
ALTER TABLE `unified_user_access_control`.`role_info` 
DROP COLUMN `role_path`,
ADD COLUMN `left` INT NOT NULL DEFAULT 0 AFTER `last_modify_time`,
ADD COLUMN `right` INT NOT NULL DEFAULT 0 AFTER `left`;
