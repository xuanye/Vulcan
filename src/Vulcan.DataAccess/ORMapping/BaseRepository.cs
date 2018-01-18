using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Vulcan.DataAccess.ORMapping
{
    public abstract class BaseRepository
    {
        protected BaseRepository(IConnectionManagerFactory mgr,string constr)
            : this(mgr, null, constr)
        {

        }
       
        protected BaseRepository(IConnectionManagerFactory mgr,IConnectionFactory factory,string constr)
        {
            this._conStr = constr;
            this._dbFactory = factory;
        }

        private readonly IConnectionFactory _dbFactory;
        private readonly string _conStr;
        private readonly IConnectionManagerFactory _mgr;
        /// <summary>
        /// 如果主键是自增返回插入主键 否则返回0
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public long Insert(AbstractBaseEntity entity)
        {
            long ret;
            using (ConnectionManager mgr = GetConnection())
            {
                ret = mgr.Connection.Query<long>(entity.GetInsertSQL(), entity, mgr.Transaction, false, null, CommandType.Text).Single();
            }
            return ret;
        }

        /// <summary>
        /// 异步执行插入操作
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<long> InsertAsync(AbstractBaseEntity entity)
        {
            Task<long> ret;
            using (ConnectionManager mgr = GetConnection())
            {
                ret = mgr.Connection.QueryFirstAsync<long>(entity.GetInsertSQL(), entity, mgr.Transaction, null, CommandType.Text);
            }
            return ret;
           
        }
        /// <summary>
        /// 批量新增 （新增相同的列）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public int BatchInsert<T>(List<T> list) where T : AbstractBaseEntity
        {
            int ret = -1;
            if (list != null && list.Count > 0)
            {
                string sql = list[0].GetInsertSQL();
                ret = Excute(sql, list);
            }
            return ret;
        }

        public async Task<int> BatchInsertAsync<T>(List<T> list) where T : AbstractBaseEntity
        {
            int ret = -1;
            if (list != null && list.Count > 0)
            {
                string sql = list[0].GetInsertSQL();
                ret = await ExcuteAsync(sql, list);
            }
            return ret;
        }

        public int Update(AbstractBaseEntity model)
        {
            return Excute(model.GetUpdateSQL(), model);
        }
        public Task<int> UpdateAsync(AbstractBaseEntity model)
        {
            return ExcuteAsync(model.GetUpdateSQL(), model);
        }
        /// <summary>
        /// 批量修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public int BatchUpdate<T>(List<T> list) where T : AbstractBaseEntity
        {
            int ret = -1;
            if (list != null && list.Count > 0)
            {
                string sql = list[0].GetUpdateSQL();
                ret = Excute(sql, list);
            }
            return ret;
        }

        public async Task<int> BatchUpdateAsync<T>(List<T> list) where T : AbstractBaseEntity
        {
            int ret = -1;
            if (list != null && list.Count > 0)
            {
                string sql = list[0].GetUpdateSQL();
                ret = await ExcuteAsync(sql, list);
            }
            return ret;
        }

        /// <summary>
        /// 开启一个事务
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public TransScope BeginTransScope(TransScopeOption option = TransScopeOption.Required)
        {
            return new TransScope(this._mgr,this._dbFactory,this._conStr, option);
        }

        /// <summary>
        /// 开启一个数据库操作模块，模块中的数据操作，将共用同一个链接，当然如果是同一个链接的话
        /// </summary>
        /// <returns></returns>
        public ConnectionScope BeginConnectionScope()
        {
            return new ConnectionScope(this._mgr, this._conStr, this._dbFactory);
        }

        protected int Excute(string sql, object paras)
        {
            int ret;
            using (ConnectionManager mgr = GetConnection())
            {
                ret = mgr.Connection.Execute(sql, paras, mgr.Transaction,null, CommandType.Text);
            }
            return ret;
        }

        protected int Excute(string sql,int timeOut, object paras)
        {
            int ret;
            using (ConnectionManager mgr = GetConnection())
            {
                ret = mgr.Connection.Execute(sql, paras, mgr.Transaction,timeOut, CommandType.Text);
            }
            return ret;
        }

        protected Task<int> ExcuteAsync(string sql, object paras)
        {
            Task<int> ret;
            using (ConnectionManager mgr = GetConnection())
            {
                ret = mgr.Connection.ExecuteAsync(sql, paras, mgr.Transaction, null, CommandType.Text);
            }
            return ret;
        }
        protected Task<int> ExcuteAsync(string sql, int timeOut, object paras)
        {
            Task<int> ret;
            using (ConnectionManager mgr = GetConnection())
            {
                ret = mgr.Connection.ExecuteAsync(sql, paras, mgr.Transaction, timeOut, CommandType.Text);
            }
            return ret;
        }

       

        protected T Get<T>(string sql, object paras)
        {
            return Query<T>(sql, paras).FirstOrDefault();
        }

        protected Task<T> GetAsync<T>(string sql, object paras)
        {
            using (ConnectionManager mgr = GetConnection())
            {
                return mgr.Connection.QueryFirstOrDefaultAsync<T>(sql, paras, mgr.Transaction, null, CommandType.Text);
            }
        }


        protected List<T> Query<T>(string sql, object paras)
        {
            List<T> list;
            using (ConnectionManager mgr = GetConnection())
            {
                list = mgr.Connection.Query<T>(sql, paras, mgr.Transaction, false, null, CommandType.Text).ToList();
            }
            return list;
        }


        protected async Task<List<T>> QueryAsync<T>(string sql, object paras)
        {
            var list = default(List<T>);
            using (ConnectionManager mgr = GetConnection())
            {
                var qlist= await mgr.Connection.QueryAsync<T>(sql, paras, mgr.Transaction, null, CommandType.Text);
                list = qlist.ToList();
            }
            return list;
        }


        protected List<T> Query<T>(string sql, int timeOut, object paras)
        {
            List<T> list;
            using (ConnectionManager mgr = GetConnection())
            {
                list = mgr.Connection.Query<T>(sql, paras, mgr.Transaction, false, timeOut, CommandType.Text).ToList();
            }
            return list;
        }

        protected async Task<List<T>> QueryAsync<T>(string sql, int timeOut, object paras)
        {
            List<T> list;
            using (ConnectionManager mgr = GetConnection())
            {
                var qlist = await mgr.Connection.QueryAsync<T>(sql, paras, mgr.Transaction, timeOut, CommandType.Text);
                list = qlist.ToList();
            }
            return list;
        }


        protected List<T> Query<T, T1>(string sql, object paras, Func<T, T1, T> parse, string splitOn)
        {
            List<T> list;
            using (ConnectionManager mgr = GetConnection())
            {
                list = mgr.Connection.Query<T, T1, T>(sql, parse, paras, mgr.Transaction, false, splitOn, 60000, CommandType.Text).ToList();
            }
            return list;
        }

        protected List<T> Query<T, T1, T2>(string sql, object paras, Func<T, T1, T2, T> parse, string splitOn)
        {
            List<T> list;
            using (ConnectionManager mgr = GetConnection())
            {
                list = mgr.Connection.Query<T, T1, T2, T>(sql, parse, paras, mgr.Transaction, false, splitOn, 60000, CommandType.Text).ToList();
            }
            return list;
        }

        protected List<T> Query<T, T1, T2, T3>(string sql, object paras, Func<T, T1, T2, T3, T> parse, string splitOn)
        {
            List<T> list;
            using (ConnectionManager mgr = GetConnection())
            {
                list = mgr.Connection.Query<T, T1, T2, T3, T>(sql, parse, paras, mgr.Transaction, false, splitOn, 60000, CommandType.Text).ToList();
            }
            return list;
        }

        protected async Task<List<T>> QueryAsync<T, T1>(string sql, object paras, Func<T, T1, T> parse, string splitOn)
        {
            List<T> list;
            using (ConnectionManager mgr = GetConnection())
            {
                var qlist = await mgr.Connection.QueryAsync<T, T1, T>(sql, parse, paras, mgr.Transaction, false, splitOn, 60000, CommandType.Text);
                list = qlist.ToList();                  
            }
            return list;

        }

        protected async Task<List<T>> QueryAsync<T, T1, T2>(string sql, object paras, Func<T, T1, T2, T> parse, string splitOn)
        {
            List<T> list;
            using (ConnectionManager mgr = GetConnection())
            {
                var qlist = await mgr.Connection.QueryAsync<T, T1, T2, T>(sql, parse, paras, mgr.Transaction, false, splitOn, 60000, CommandType.Text);
                list = qlist.ToList();
            }
            return list;
        }

        protected async Task<List<T>> QueryAsync<T, T1, T2, T3>(string sql, object paras, Func<T, T1, T2, T3, T> parse, string splitOn)
        {
            List<T> list;
            using (ConnectionManager mgr = GetConnection())
            {
                var qlist = await mgr.Connection.QueryAsync<T, T1, T2, T3, T>(sql, parse, paras, mgr.Transaction, false, splitOn, 60000, CommandType.Text);
                list = qlist.ToList();
            }
            return list;
        }



        protected int SPExcute(string spName, object paras)
        {
            int ret;
            using (ConnectionManager mgr = GetConnection())
            {
                ret = mgr.Connection.Execute(spName, paras, mgr.Transaction,null, CommandType.StoredProcedure);
            }
            return ret;
        }

        protected Task<int> SPExcuteAsync(string spName, object paras)
        {
            Task<int> ret;
            using (ConnectionManager mgr = GetConnection())
            {
                ret = mgr.Connection.ExecuteAsync(spName, paras, mgr.Transaction, null, CommandType.StoredProcedure);
            }
            return ret;
        }

        protected T SPGet<T>(string spName, object paras)
        {
            return SPQuery<T>(spName, paras).FirstOrDefault();
        }
        protected Task<T> SPGetAsync<T>(string spName, object paras)
        {
            Task<T> task;
            using (ConnectionManager mgr = GetConnection())
            {
                task = mgr.Connection.QueryFirstOrDefaultAsync<T>(spName, paras, mgr.Transaction, null, CommandType.StoredProcedure);
            }
            return task;
        }

        protected List<T> SPQuery<T>(string spName, object paras)
        {
            List<T> list;
            using (ConnectionManager mgr = GetConnection())
            {
                list = mgr.Connection.Query<T>(spName, paras, mgr.Transaction, false, null, CommandType.StoredProcedure).ToList();
            }
            return list;
        }

        protected Task<List<T>> SPQueryAsync<T>(string spName, object paras)
        {
            Task<List<T>> task;
            using (ConnectionManager mgr = GetConnection())
            {
                task = mgr.Connection.QueryAsync<T>(spName, paras, mgr.Transaction, null, CommandType.StoredProcedure)
                    .ContinueWith<List<T>>(x => x.Result.ToList());
            }
            return task;
        }


        protected ConnectionManager GetConnection()
        {
            return _mgr.GetConnectionManager(_dbFactory, this._conStr);
        }
    }
}
