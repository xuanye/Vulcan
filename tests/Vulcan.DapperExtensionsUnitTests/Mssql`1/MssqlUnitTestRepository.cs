using Vulcan.DapperExtensions.Contract;
using Vulcan.DapperExtensionsUnitTests.Internal;

namespace Vulcan.DapperExtensionsUnitTests.Mssql
{
    public class MssqlUnitTestRepository : TestRepository
    {
        public MssqlUnitTestRepository(IConnectionManagerFactory mgr, string constr, IConnectionFactory factory = null)
            : base(mgr, constr, factory)
        {
        }

        protected override string GetInitialSql(int type = 1)
        {
            var tableName = type == 1 ? "test_item" : "async_test_item";
            return @$"
-- ----------------------------
-- Table structure for {tableName}
-- ----------------------------
DROP TABLE IF EXISTS [{tableName}];
create table  {tableName}
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
alter  table {tableName}
       add constraint PK_{tableName}_id primary key (id);
EXEC sp_addextendedproperty 'MS_Description', 'primary key', 'user', dbo, 'table', {tableName}, 'column', id;
EXEC sp_addextendedproperty 'MS_Description', 'name', 'user', dbo, 'table', {tableName}, 'column', name;
EXEC sp_addextendedproperty 'MS_Description', 'mobile phone', 'user', dbo, 'table', {tableName}, 'column', mobile_phone;
EXEC sp_addextendedproperty 'MS_Description', 'link address', 'user', dbo, 'table', {tableName}, 'column', address;
EXEC sp_addextendedproperty 'MS_Description', 'test fo bool', 'user', dbo, 'table', {tableName}, 'column', is_default;
EXEC sp_addextendedproperty 'MS_Description', 'test for enum 0= initial status 99=deleted', 'user', dbo, 'table', {tableName}, 'column', [status];";
        }



    }
}
