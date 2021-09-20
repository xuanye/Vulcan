using Vulcan.DapperExtensions.Contract;
using Vulcan.DapperExtensionsUnitTests.Internal;

namespace Vulcan.DapperExtensionsUnitTests.MySQL
{
    public class MySQLUnitTestRepository : TestRepository
    {
        public MySQLUnitTestRepository(IConnectionManagerFactory mgr, string constr, IConnectionFactory factory = null)
            : base(mgr, constr, factory)
        {
        }

        protected override string GetInitialSQL()
        {
            return  @"
-- ----------------------------
-- Table structure for test_item_table
-- ----------------------------
DROP TABLE IF EXISTS `test_item_table`;
create table  `test_item_table`
(
       id                INT primary key auto_increment not null comment 'primary key',
       name              VARCHAR(50) comment 'name',
       mobile_phone      VARCHAR(50) comment 'mobile phone',
       address           VARCHAR(250) comment 'link address',
       is_default        BIT default 0 not null comment 'test fo bool',
       `status`          TINYINT default 0 not null comment 'test for enum 0= initial status 99=deleted',
       creator_id        VARCHAR(50) not null,
       create_time       TIMESTAMP default current_timestamp not null,
       modifier_id       VARCHAR(50) not null,
       modify_time       TIMESTAMP default current_timestamp not null
);";
        }
    }
}
