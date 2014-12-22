 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Vulcan;
using Vulcan.DataAccess;
using Vulcan.Test.MockObject;


namespace Vulcan.Test
{
   [TestFixture]
    public class TranScopeTest
    {
        [Test]
        public void TestTranScopeRollback()
        {
            string constr = "server=192.168.57.186;port=3306;database=vipsystemv2;uid=vipsystem;pwd=vipsystem;charset=utf8;Connection Timeout=18000;";
            ConnectionFactory.Default = new MySqlConnectionFactory();
         
            try
            {
                

                using (TransScope scope = new TransScope(constr))
                {
                    TestMysqlRepository repo = new TestMysqlRepository(constr);

                    repo.Excute("delete from testscope", null);
                    repo.Excute("insert into testscope(`col1`,`col2`) values ('1','2');", null);

                    var count =repo.Count("testscope");


                    Assert.AreEqual(1, count);
                    if(count ==1)
                    {
                        throw new Exception("抛出一个异常");
                    }                  

                    scope.Commit();
                }
            }
            catch
            {

            }

            TestMysqlRepository repo2 = new TestMysqlRepository(constr);          
            var count2 = repo2.Count("testscope");
            Assert.AreEqual(0, count2);
        }
    }
}
