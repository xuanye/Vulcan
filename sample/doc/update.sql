-- 修改用户表，删除是否组织管理员字段，添加用户可见组织范围字段
ALTER TABLE `unified_user_access_control`.`user_info`
DROP COLUMN `is_admin`,
ADD COLUMN `view_root_code` VARCHAR(50) NULL COMMENT '用户可见组织范围-最高层组织编码' AFTER `gender`,
ADD COLUMN `view_root_name` VARCHAR(200) NULL COMMENT '用户可见组织范围-最高层组织名称' AFTER `view_root_code`;
