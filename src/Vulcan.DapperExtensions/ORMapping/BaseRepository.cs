using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Vulcan.DapperExtensions.Contract;
using Vulcan.DapperExtensions.Internal;

namespace Vulcan.DapperExtensions.ORMapping
{
    public abstract class BaseRepository
    {
        private readonly string _conStr;

        private readonly IConnectionFactory _dbFactory;
        private readonly IConnectionManagerFactory _mgr;

        protected BaseRepository(IConnectionManagerFactory mgr, string constr, IConnectionFactory factory = null)
        {
            _conStr = constr;
            _dbFactory = factory;
            _mgr = mgr;
        }

        /// <summary>
        ///     insert entity value to db
        ///     If the primary key is an identity column, return auto-increment value otherwise returns 0
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public long Insert(BaseEntity entity)
        {
            long ret;
            using (var mgr = GetConnection())
            {
                using (var metrics = CreateSQLMetrics())
                {
                    var sql = entity.GetInsertSQL(_dbFactory.SQLBuilder);
                    ret = mgr.Connection.QueryFirstOrDefault<long>(sql, entity, mgr.Transaction, null,
                        CommandType.Text);
                    metrics.AddToMetrics(sql, entity);
                }
            }

            return ret;
        }

        /// <summary>
        ///     async insert entity value to db
        ///     If the primary key is an identity column, return auto-increment value otherwise returns 0
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<long> InsertAsync(BaseEntity entity)
        {
            long ret;
            using (var mgr = GetConnection())
            {
                using (var metrics = CreateSQLMetrics())
                {
                    var sql = entity.GetInsertSQL(_dbFactory.SQLBuilder);
                    ret = await mgr.Connection.QueryFirstOrDefaultAsync<long>(entity.GetInsertSQL(_dbFactory.SQLBuilder), entity,
                        mgr.Transaction, null, CommandType.Text);
                    metrics.AddToMetrics(sql, entity);
                }
            }

            return ret;
        }

        /// <summary>
        ///     batch insert
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        [Obsolete("Cyclic insertion, recommended only when the data volume is small")]
        public int BatchInsert<T>(List<T> list) where T : BaseEntity
        {
            if (list == null || list.Count == 0) return -1;

            var sql = list[0].GetInsertSQL(_dbFactory.SQLBuilder);
            return Execute(sql, list);
        }

        /// <summary>
        ///     batch insert asynchronous
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Obsolete("Cyclic insertion, recommended only when the data volume is small")]
        public Task<int> BatchInsertAsync<T>(List<T> list) where T : BaseEntity
        {
            if (list == null || list.Count <= 0) return Task.FromResult(-1);

            var sql = list[0].GetInsertSQL(_dbFactory.SQLBuilder);
            return ExecuteAsync(sql, list);
        }

        /// <summary>
        ///     update entity changed value
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(BaseEntity model)
        {
            return Execute(model.GetUpdateSQL(_dbFactory.SQLBuilder), model);
        }

        /// <summary>
        ///     update asynchronous
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<int> UpdateAsync(BaseEntity model)
        {
            return ExecuteAsync(model.GetUpdateSQL(_dbFactory.SQLBuilder), model);
        }

        /// <summary>
        ///     batch update
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        [Obsolete("Cyclic insertion, recommended only when the data volume is small")]
        public int BatchUpdate<T>(List<T> list) where T : BaseEntity
        {
            if (list == null || list.Count == 0) return -1;

            var sql = list[0].GetUpdateSQL(_dbFactory.SQLBuilder);
            return Execute(sql, list);
        }

        /// <summary>
        ///     batch update asynchronous
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Obsolete("Cyclic insertion, recommended only when the data volume is small")]
        public async Task<int> BatchUpdateAsync<T>(List<T> list) where T : BaseEntity
        {
            if (list == null || list.Count == 0) return -1;

            var sql = list[0].GetUpdateSQL(_dbFactory.SQLBuilder);
            return await ExecuteAsync(sql, list);
        }

        /// <summary>
        ///     begin a transaction scope ,if it's exists, otherwise reuse same one
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public TransScope BeginTransScope(TransScopeOption option = TransScopeOption.Required)
        {
            return new TransScope(_mgr, _dbFactory, _conStr, option);
        }

        /// <summary>
        ///     create a connection scope to share db connection and reuse it;
        /// </summary>
        /// <returns></returns>
        public ConnectionScope BeginConnectionScope()
        {
            return new ConnectionScope(_mgr, _conStr, _dbFactory);
        }

        /// <summary>
        ///     execute sql whit parameters
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        protected int Execute(string sql, object paras)
        {
            int ret;
            using (var mgr = GetConnection())
            {
                using (var metrics = CreateSQLMetrics())
                {
                    ret = mgr.Connection.Execute(sql, paras, mgr.Transaction, null, CommandType.Text);
                    metrics.AddToMetrics(sql, paras);
                }
            }

            return ret;
        }

        /// <summary>
        ///     Execute parameterized SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="timeOut">number of timeout seconds</param>
        /// <param name="paras"></param>
        /// <returns></returns>
        protected int Execute(string sql, int timeOut, object paras)
        {
            int ret;
            using (var mgr = GetConnection())
            {
                using (var metrics = CreateSQLMetrics())
                {
                    ret = mgr.Connection.Execute(sql, paras, mgr.Transaction, timeOut, CommandType.Text);
                    metrics.AddToMetrics(sql, paras);
                }
            }

            return ret;
        }

        /// <summary>
        /// Execute parameterized SQL Asynchronous
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        protected async Task<int> ExecuteAsync(string sql, object paras)
        {
            int ret;
            using (var mgr = GetConnection())
            {
                using (var metrics = CreateSQLMetrics())
                {
                    ret = await mgr.Connection.ExecuteAsync(sql, paras, mgr.Transaction, null, CommandType.Text);
                    metrics.AddToMetrics(sql, paras);
                }
            }

            return ret;
        }

        /// <summary>
        /// Execute parameterized SQL Asynchronous
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="timeOut">number of timeout seconds</param>
        /// <param name="paras"></param>
        /// <returns></returns>
        protected async Task<int> ExecuteAsync(string sql, int timeOut, object paras)
        {
            int ret;
            using (var mgr = GetConnection())
            {
                using (var metrics = CreateSQLMetrics())
                {
                    ret = await mgr.Connection.ExecuteAsync(sql, paras, mgr.Transaction, timeOut, CommandType.Text);
                    metrics.AddToMetrics(sql, paras);
                }
            }

            return ret;
        }

        /// <summary>
        ///  get first or default result
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T Get<T>(string sql, object paras)
        {
            return Query<T>(sql, paras).FirstOrDefault();
        }

        /// <summary>
        /// get first or default result
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected async Task<T> GetAsync<T>(string sql, object paras)
        {
            T ret;
            using (var mgr = GetConnection())
            {
                using (var metrics = CreateSQLMetrics())
                {
                    ret = await mgr.Connection.QueryFirstOrDefaultAsync<T>(sql, paras, mgr.Transaction, null,
                        CommandType.Text);
                    metrics.AddToMetrics(sql, paras);
                }
            }

            return ret;
        }

        /// <summary>
        ///     Executes a query, returning the data typed as T
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected List<T> Query<T>(string sql, object paras)
        {
            List<T> list;
            using (var mgr = GetConnection())
            {
                using (var metrics = CreateSQLMetrics())
                {
                    list = mgr.Connection.Query<T>(sql, paras, mgr.Transaction, false, null, CommandType.Text).ToList();
                    metrics.AddToMetrics(sql, paras);
                }
            }

            return list;
        }

        /// <summary>
        ///     Executes a query, returning the data typed as T
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected async Task<List<T>> QueryAsync<T>(string sql, object paras)
        {
            List<T> list;
            using (var mgr = GetConnection())
            {
                using (var metrics = CreateSQLMetrics())
                {
                    var qList = await mgr.Connection.QueryAsync<T>(sql, paras, mgr.Transaction, null, CommandType.Text);
                    list = qList.ToList();

                    metrics.AddToMetrics(sql, paras);
                }
            }

            return list;
        }

        /// <summary>
        ///     Executes a query, returning the data typed as T
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="timeOut"></param>
        /// <param name="paras"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected List<T> Query<T>(string sql, int timeOut, object paras)
        {
            List<T> list;
            using (var mgr = GetConnection())
            {
                using (var metrics = CreateSQLMetrics())
                {
                    list = mgr.Connection.Query<T>(sql, paras, mgr.Transaction, false, timeOut, CommandType.Text)
                        .ToList();
                    metrics.AddToMetrics(sql, paras);
                }
            }

            return list;
        }

        /// <summary>
        ///     Executes a query, returning the data typed as T
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="timeOut"></param>
        /// <param name="paras"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected async Task<List<T>> QueryAsync<T>(string sql, int timeOut, object paras)
        {
            List<T> list;
            using (var mgr = GetConnection())
            {
                using (var metrics = CreateSQLMetrics())
                {
                    var qList = await mgr.Connection.QueryAsync<T>(sql, paras, mgr.Transaction, timeOut,
                        CommandType.Text);
                    list = qList.ToList();
                    metrics.AddToMetrics(sql, paras);
                }
            }

            return list;
        }

        /// <summary>
        ///     Executes a query, returning the data typed as T
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <param name="parse"></param>
        /// <param name="splitOn"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <returns></returns>
        protected List<T> Query<T, T1>(string sql, object paras, Func<T, T1, T> parse, string splitOn)
        {
            List<T> list;
            using (var mgr = GetConnection())
            {
                using (var metrics = CreateSQLMetrics())
                {
                    list = mgr.Connection
                        .Query(sql, parse, paras, mgr.Transaction, false, splitOn, 360, CommandType.Text).ToList();
                    metrics.AddToMetrics(sql, paras);
                }
            }

            return list;
        }

        protected List<T> Query<T, T1, T2>(string sql, object paras, Func<T, T1, T2, T> parse, string splitOn)
        {
            List<T> list;
            using (var mgr = GetConnection())
            {
                using (var metrics = CreateSQLMetrics())
                {
                    list = mgr.Connection
                        .Query(sql, parse, paras, mgr.Transaction, false, splitOn, 360, CommandType.Text).ToList();
                    metrics.AddToMetrics(sql, paras);
                }
            }

            return list;
        }

        protected List<T> Query<T, T1, T2, T3>(string sql, object paras, Func<T, T1, T2, T3, T> parse, string splitOn)
        {
            List<T> list;
            using (var mgr = GetConnection())
            {
                using (var metrics = CreateSQLMetrics())
                {
                    list = mgr.Connection
                        .Query(sql, parse, paras, mgr.Transaction, false, splitOn, 360, CommandType.Text).ToList();
                    metrics.AddToMetrics(sql, paras);
                }
            }

            return list;
        }

        protected async Task<List<T>> QueryAsync<T, T1>(string sql, object paras, Func<T, T1, T> parse, string splitOn)
        {
            List<T> list;
            using (var mgr = GetConnection())
            {
                using (var metrics = CreateSQLMetrics())
                {
                    var qList = await mgr.Connection.QueryAsync(sql, parse, paras, mgr.Transaction, false, splitOn, 360,
                        CommandType.Text);
                    list = qList.ToList();
                    metrics.AddToMetrics(sql, paras);
                }
            }

            return list;
        }

        protected async Task<List<T>> QueryAsync<T, T1, T2>(string sql, object paras, Func<T, T1, T2, T> parse,
            string splitOn)
        {
            List<T> list;
            using (var mgr = GetConnection())
            {
                using (var metrics = CreateSQLMetrics())
                {
                    var qList = await mgr.Connection.QueryAsync(sql, parse, paras, mgr.Transaction, false, splitOn, 360,
                        CommandType.Text);
                    list = qList.ToList();
                    metrics.AddToMetrics(sql, paras);
                }
            }

            return list;
        }

        protected async Task<List<T>> QueryAsync<T, T1, T2, T3>(string sql, object paras, Func<T, T1, T2, T3, T> parse,
            string splitOn)
        {
            List<T> list;
            using (var mgr = GetConnection())
            {
                using (var metrics = CreateSQLMetrics())
                {
                    var qList = await mgr.Connection.QueryAsync(sql, parse, paras, mgr.Transaction, false, splitOn, 360,
                        CommandType.Text);
                    list = qList.ToList();
                    metrics.AddToMetrics(sql, paras);
                }
            }

            return list;
        }

        protected int SPExecute(string spName, object paras)
        {
            int ret;
            using (var mgr = GetConnection())
            {
                using (var metrics = CreateSQLMetrics())
                {
                    ret = mgr.Connection.Execute(spName, paras, mgr.Transaction, null, CommandType.StoredProcedure);
                    metrics.AddToMetrics(spName, paras);
                }
            }

            return ret;
        }

        protected async Task<int> SPExecuteAsync(string spName, object paras)
        {
            int ret;
            using (var mgr = GetConnection())
            {
                using (var metrics = CreateSQLMetrics())
                {
                    ret = await mgr.Connection.ExecuteAsync(spName, paras, mgr.Transaction, null,
                        CommandType.StoredProcedure);
                    metrics.AddToMetrics(spName, paras);
                }
            }

            return ret;
        }

        protected T SPGet<T>(string spName, object paras)
        {
            return SPQuery<T>(spName, paras).FirstOrDefault();
        }

        protected async Task<T> SPGetAsync<T>(string spName, object paras)
        {
            T ret;
            using (var mgr = GetConnection())
            {
                using (var metrics = CreateSQLMetrics())
                {
                    ret = await mgr.Connection.QueryFirstOrDefaultAsync<T>(spName, paras, mgr.Transaction, null,
                        CommandType.StoredProcedure);
                    metrics.AddToMetrics(spName, paras);
                }
            }

            return ret;
        }

        protected List<T> SPQuery<T>(string spName, object paras)
        {
            List<T> list;
            using (var mgr = GetConnection())
            {
                using (var metrics = CreateSQLMetrics())
                {
                    list = mgr.Connection
                        .Query<T>(spName, paras, mgr.Transaction, false, null, CommandType.StoredProcedure).ToList();
                    metrics.AddToMetrics(spName, paras);
                }
            }

            return list;
        }

        protected async Task<List<T>> SPQueryAsync<T>(string spName, object paras)
        {
            List<T> ret;
            using (var mgr = GetConnection())
            {
                using (var metrics = CreateSQLMetrics())
                {
                    var list = await mgr.Connection.QueryAsync<T>(spName, paras, mgr.Transaction, null,
                        CommandType.StoredProcedure);
                    ret = list.ToList();
                    metrics.AddToMetrics(spName, paras);
                }
            }

            return ret;
        }

        protected ConnectionManager GetConnection()
        {
            return _mgr.GetConnectionManager(_conStr, _dbFactory);
        }

        protected virtual ISQLMetrics CreateSQLMetrics()
        {
            return new NoopSQLMetrics();
        }
    }
}
