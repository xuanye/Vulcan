using MySql.Data.MySqlClient;
using System;
using Xunit;
using Dapper;
using System.Threading.Tasks;
using Vulcan.DataAccess.ORMapping;
using Vulcan.DataAccess.ORMapping.MySql;
using Vulcan.DataAccess;

namespace Vulcan.UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var conn = new MySqlConnection("server=127.0.0.1;port=3306;database=test;uid=root;pwd=Welcome@123;charset=utf8;Connection Timeout=18000;SslMode=none;");

            string sql = @"INSERT INTO `tbtest1` (`name`) " +
             "VALUES (@Name);SELECT CAST(LAST_INSERT_ID() AS SIGNED) as Id;";

          
            long id = 0;

            var svitem = new TbTest()
            {
                Name = "111",              
            };

            for(var i =0; i< 100; i++)
            {
                await conn.OpenAsync();

                id = await conn.QueryFirstOrDefaultAsync<long>(sql, svitem);
                Console.WriteLine(id);

                await conn.CloseAsync();

                Assert.True(id > 0);
            }
          
        }
   
    }

    public class TbTestRepository : MySqlRepository
    {
        public TbTestRepository(IConnectionManagerFactory cmFactory, string constr) : base(cmFactory, constr)
        {
        }
    }

    public class TbTest
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    [TableName("activity_simplevote")]
    public partial class ActivitySimplevote : MySqlEntity
    {
        private int _Id;

        /// <summary>
        /// 编号
        ///  int(10)
        /// </summary>
        [MapField("id"), Identity, PrimaryKey(1)]
        public int Id
        { get { return _Id; } set { _Id = value; } }

        private int _ActivityId;

        /// <summary>
        /// 活动ID
        ///  int(10)
        /// </summary>
        [MapField("activity_id")]
        public int ActivityId
        { get { return _ActivityId; } set { _ActivityId = value; OnPropertyChanged("activity_id"); } }

        private string _UserId;

        /// <summary>
        /// 用户ID
        ///  varchar(50)
        /// </summary>
        [MapField("user_id")]
        public string UserId
        { get { return _UserId; } set { _UserId = value; OnPropertyChanged("user_id"); } }

        private string _Title;

        /// <summary>
        /// 活动标题 活动标题 也可以昵称 用于查询
        ///  varchar(200)
        /// </summary>
        [MapField("title")]
        public string Title
        { get { return _Title; } set { _Title = value; OnPropertyChanged("title"); } }

        private string _Content;

        /// <summary>
        /// 活动文本内容 活动文本内容
        ///  varchar(2000)
        /// </summary>
        [MapField("content"), Nullable]
        public string Content
        { get { return _Content; } set { _Content = value; OnPropertyChanged("content"); } }

        private string _PicUrls;

        /// <summary>
        /// 照片地址 JSON 格式
        ///  varchar(1000)
        /// </summary>
        [MapField("pic_urls"), Nullable]
        public string PicUrls
        { get { return _PicUrls; } set { _PicUrls = value; OnPropertyChanged("pic_urls"); } }

        private string _VoiceUrl;

        /// <summary>
        /// 语音地址 语音地址
        ///  varchar(1000)
        /// </summary>
        [MapField("voice_url"), Nullable]
        public string VoiceUrl
        { get { return _VoiceUrl; } set { _VoiceUrl = value; OnPropertyChanged("voice_url"); } }

        private int _VoteCount;

        /// <summary>
        /// 总票数 总票数 0
        ///  int(10)
        /// </summary>
        [MapField("vote_count")]
        public int VoteCount
        { get { return _VoteCount; } set { _VoteCount = value; OnPropertyChanged("vote_count"); } }

        private int _CheatCount;

        /// <summary>
        /// 管理员添加的数量
        ///  int(10)
        /// </summary>
        [MapField("cheat_count")]
        public int CheatCount
        { get { return _CheatCount; } set { _CheatCount = value; OnPropertyChanged("cheat_count"); } }

        private int _VoiceDuration;

        /// <summary>
        /// 语音时长 语音时长
        ///  int(10)
        /// </summary>
        [MapField("voice_duration")]
        public int VoiceDuration
        { get { return _VoiceDuration; } set { _VoiceDuration = value; OnPropertyChanged("voice_duration"); } }

        private sbyte _Status;

        /// <summary>
        /// 状态 状态 1= 有效 0=无效
        ///  tinyint(3)
        /// </summary>
        [MapField("status")]
        public sbyte Status
        { get { return _Status; } set { _Status = value; OnPropertyChanged("status"); } }

        private DateTime _CreateTime;

        /// <summary>
        /// 开始时间 开始时间
        ///  datetime
        /// </summary>
        [MapField("create_time")]
        public DateTime CreateTime
        { get { return _CreateTime; } set { _CreateTime = value; OnPropertyChanged("create_time"); } }
    }
}
