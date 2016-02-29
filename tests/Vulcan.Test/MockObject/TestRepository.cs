using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vulcan.DataAccess.ORMapping.MySql;

namespace Vulcan.Test.MockObject
{
    public class TestMysqlRepository : MySqlRepository
    {
        public TestMysqlRepository(string constr)
            :base(constr)
        {

        }
        public long Count(string tableName)
        {
            string sql = "select count(1) from " + tableName;
            return base.Get<long>(sql, null);
        }
        public new int Excute(string sql,object obj)
        {
            return base.Excute(sql, obj);
        }
    }
}
