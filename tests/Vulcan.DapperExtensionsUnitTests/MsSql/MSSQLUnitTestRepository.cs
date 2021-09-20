using Vulcan.DapperExtensions.Contract;
using Vulcan.DapperExtensionsUnitTests.Internal;

namespace Vulcan.DapperExtensionsUnitTests.MSSQL
{
    public class MSSQLUnitTestRepository : TestRepository
    {
        public MSSQLUnitTestRepository(IConnectionManagerFactory mgr, string constr, IConnectionFactory factory = null)
            : base(mgr, constr, factory)
        {
        }

        protected override string GetInitialSQL()
        {
            return  @"
-- ----------------------------
-- Table structure for test_item_table
-- ----------------------------
DROP TABLE IF EXISTS [test_item_table];
create table  test_item_table
(
       id                INT identity(1, 1) not null 	/*primary key*/,
       name              VARCHAR(50) 	/*name*/,
       mobile_phone      VARCHAR(50) 	/*mobile phone*/,
       address           VARCHAR(250) 	/*link address*/,
       is_default        BIT default 0 not null 	/*test fo bool*/,
       [status]          TINYINT default 0 not null 	/*test for enum 0= initial status 99=deleted*/,
       creator_id        VARCHAR(50) not null,
       create_time       DATETIME default getdate() not null,
       modifier_id       VARCHAR(50) not null,
       modify_time       DATETIME default getdate() not null
);
alter  table test_item_table
       add constraint PK_test_item_table_id primary key (id);
EXEC sp_addextendedproperty 'MS_Description', 'primary key', 'user', dbo, 'table', test_item_table, 'column', id;
EXEC sp_addextendedproperty 'MS_Description', 'name', 'user', dbo, 'table', test_item_table, 'column', name;
EXEC sp_addextendedproperty 'MS_Description', 'mobile phone', 'user', dbo, 'table', test_item_table, 'column', mobile_phone;
EXEC sp_addextendedproperty 'MS_Description', 'link address', 'user', dbo, 'table', test_item_table, 'column', address;
EXEC sp_addextendedproperty 'MS_Description', 'test fo bool', 'user', dbo, 'table', test_item_table, 'column', is_default;
EXEC sp_addextendedproperty 'MS_Description', 'test for enum 0= initial status 99=deleted', 'user', dbo, 'table', test_item_table, 'column', [status];";
        }


    }
}
