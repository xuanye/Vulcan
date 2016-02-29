using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Vulcan;
using Vulcan.DataAccess;
using Vulcan.DataAccess.ORMapping;
using Vulcan.DataAccess.ORMapping.MySql;
using Vulcan.Test.MockObject;


namespace Vulcan.Test
{
   [TestFixture]
    public class TranScopeTest
    {
        [Test]
        public void TestTranScopeRollback()
        {
            string constr = "server=192.168.100.126;port=3306;database=xgcalendar;uid=xgcalendar;pwd=xgcalendar@123;charset=utf8;Connection Timeout=18000;";
            ConnectionFactory.Default = new MySqlConnectionFactory();

            TestMysqlRepository repo = new TestMysqlRepository(constr); 
                   
            repo.Insert(new UserDefined(){
                user_identity = Guid.NewGuid().ToString(),
                mobile_phone_en = "abasdsadsasdadadadad"
            });
           
        }
    }

   [TableName("user_defined")]
   public partial class UserDefined : MySqlEntity
   {
       private string _user_identity;
       /// <summary>
       /// 用户唯一标识
       ///  varchar(50)
       /// </summary>
       [PrimaryKey(1)]
       public string user_identity
       { get { return _user_identity; } set { _user_identity = value; OnPropertyChanged("user_identity"); } }
       private string _mobile_phone_en;
       /// <summary>
       /// 用户手机号加密后的数据
       ///  varchar(100)
       /// </summary>

       public string mobile_phone_en
       { get { return _mobile_phone_en; } set { _mobile_phone_en = value; OnPropertyChanged("mobile_phone_en"); } }
   }
}
