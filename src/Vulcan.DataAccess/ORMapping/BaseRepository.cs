using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;

namespace Vulcan.DataAccess.ORMapping
{
    public abstract class BaseRepository
    {
        protected BaseRepository(string constr)
        {
            this._conStr = constr;
        }

        private readonly string _conStr;

        /// <summary>
        /// 如果主键是自增返回插入主键 否则返回0
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public long Insert(BaseEntity entity)
        {
            long ret;
            using (ConnectionManager mgr = GetConnection())
            {
                ret = mgr.Connection.Query<long>(entity.GetInsertSQL(), entity, mgr.Transaction, false, null, CommandType.Text).Single();
            }
            return ret;
        }

        /// <summary>
        /// 批量新增 （新增相同的列）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public int BatchInsert<T>(List<T> list) where T : BaseEntity
        {
            int ret = -1;
            if (list != null && list.Count > 0)
            {
                string sql = list[0].GetInsertSQL();
                ret = Excute(sql, list);
            }
            return ret;
        }

        public int Update(BaseEntity model)
        {
            return Excute(model.GetUpdateSQL(), model);
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public int BatchUpdate<T>(List<T> list) where T : BaseEntity
        {
            int ret = -1;
            if (list != null && list.Count > 0)
            {
                string sql = list[0].GetUpdateSQL();
                ret = Excute(sql, list);
            }
            return ret;
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
        protected T Get<T>(string sql, object paras)
        {
            return Query<T>(sql, paras).FirstOrDefault();
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
        protected List<T> Query<T>(string sql, int timeOut, object paras)
        {
            List<T> list;
            using (ConnectionManager mgr = GetConnection())
            {
                list = mgr.Connection.Query<T>(sql, paras, mgr.Transaction, false, timeOut, CommandType.Text).ToList();
            }
            return list;
        }
        /*
   public static IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TReturn> map, object param, IDbTransaction transaction, bool buffered, string splitOn, int? commandTimeout, CommandType? commandType);
   public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param, IDbTransaction transaction, bool buffered, string splitOn, int? commandTimeout, CommandType? commandType);
   public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param, IDbTransaction transaction, bool buffered, string splitOn, int? commandTimeout, CommandType? commandType);
    */

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

        protected int SPExcute(string spName, object paras)
        {
            int ret;
            using (ConnectionManager mgr = GetConnection())
            {
                ret = mgr.Connection.Execute(spName, paras, mgr.Transaction,null, CommandType.StoredProcedure);
            }
            return ret;
        }

        protected T SPGet<T>(string spName, object paras)
        {
            return SPQuery<T>(spName, paras).FirstOrDefault();
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

        protected ConnectionManager GetConnection()
        {
            return ConnectionManager.GetManager(this._conStr);
        }
    }
}