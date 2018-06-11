using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;

namespace Vulcan.DataAccess.ORMapping.MySql
{
    public class MySqlRepository : BaseRepository
    {
        public MySqlRepository(IConnectionManagerFactory cmFactory, string constr)
            : base(cmFactory, constr)
        {
        }

        protected MySqlRepository(IConnectionManagerFactory cmFactory, IConnectionFactory factory, string constr)
            : base(cmFactory, factory, constr)
        {
        }

        public override long Insert(AbstractBaseEntity entity)
        {
            long ret;
            using (ConnectionManager mgr = GetConnection())
            {
                using (ISQLMetrics metrics = CreateSQLMetrics())
                {
                    string sql = entity.GetInsertSQL();

                    string[] splitSqL = sql.Split(';');

                    mgr.Connection.Execute(splitSqL[0], entity);

                    ret = mgr.Connection.QueryFirstOrDefault<long>(splitSqL[1]);

                    metrics.AddToMetrics(sql, entity);
                }
            }
            return ret;
        }

        public override async Task<long> InsertAsync(AbstractBaseEntity entity)
        {
            long ret;
            using (ConnectionManager mgr = GetConnection())
            {
                using (ISQLMetrics metrics = CreateSQLMetrics())
                {
                    string sql = entity.GetInsertSQL();

                    string[] splitSqL = sql.Split(';');

                    await mgr.Connection.ExecuteAsync(splitSqL[0], entity);

                    ret = await mgr.Connection.QueryFirstOrDefaultAsync<long>(splitSqL[1]);

                    metrics.AddToMetrics(sql, entity);
                }
            }
            return ret;
        }

        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <typeparam name="T">返回列表的实体对象类型</typeparam>
        /// <param name="view">分页查询信息</param>
        /// <param name="sqlColumns">查询的字符串</param>
        /// <param name="sqlTable">查询的表，可以为多表，即From后面 where之前的内容</param>
        /// <param name="sqlCondition">查询条件 Where 后面的部分.</param>
        /// <param name="param">查询实体值得实体对象.</param>
        /// <param name="sqlPk">该条查询的唯一键.</param>
        /// <param name="sqlOrder">排序字段 包含Order by.</param>
        /// <returns>返回分页信息，当查询为第一页时 返回总记录数</returns>
        public PagedList<T> PagedQuery<T>(PageView view, string sqlColumns, string sqlTable, string sqlCondition, object param, string sqlPk, string sqlOrder)
        {
            PagedList<T> pList = new PagedList<T>();
            long totalCount = -1;
            if (view.PageIndex == 0)
            {
                string totalSql = string.Format(" select count(1) from {0} where 1=1 {1} ;", sqlTable, sqlCondition);
                totalCount = Get<long>(totalSql, param);
                if (totalCount == 0)
                {
                    pList.DataList = new List<T>();
                    pList.Total = 0;
                    pList.PageIndex = view.PageIndex;
                    pList.PageSize = view.PageSize;
                    return pList;
                }
            }

            if (string.IsNullOrEmpty(sqlOrder))
            {
                sqlOrder = " ORDER BY " + sqlPk;
            }
            int pageStartIndex = view.PageSize * view.PageIndex;
            int currentPageCount = view.PageSize;
            string sql = string.Format(" select {0} from {1} where 1=1  {2} {3} limit {4},{5} ;", sqlColumns, sqlTable, sqlCondition, sqlOrder, pageStartIndex, currentPageCount);

            pList.DataList = Query<T>(sql, param);
            pList.Total = (int)totalCount;
            pList.PageIndex = view.PageIndex;
            pList.PageSize = view.PageSize;
            return pList;
        }

        public async Task<PagedList<T>> PagedQueryAsync<T>(PageView view, string sqlColumns, string sqlTable, string sqlCondition, object param, string sqlPk, string sqlOrder)
        {
            PagedList<T> pList = new PagedList<T>();
            long totalCount = -1;
            if (view.PageIndex == 0)
            {
                string totalSql = string.Format(" select count(1) from {0} where 1=1 {1} ;", sqlTable, sqlCondition);
                totalCount = await GetAsync<long>(totalSql, param);
                if (totalCount == 0)
                {
                    pList.DataList = new List<T>();
                    pList.Total = 0;
                    pList.PageIndex = view.PageIndex;
                    pList.PageSize = view.PageSize;
                    return pList;
                }
            }

            if (string.IsNullOrEmpty(sqlOrder))
            {
                sqlOrder = " ORDER BY " + sqlPk;
            }
            int pageStartIndex = view.PageSize * view.PageIndex;
            int currentPageCount = view.PageSize;
            string sql = string.Format(" select {0} from {1} where 1=1  {2} {3} limit {4},{5} ;", sqlColumns, sqlTable, sqlCondition, sqlOrder, pageStartIndex, currentPageCount);

            pList.DataList = await QueryAsync<T>(sql, param);
            pList.Total = (int)totalCount;
            pList.PageIndex = view.PageIndex;
            pList.PageSize = view.PageSize;
            return pList;
        }

        public void ReplaceInto(MySqlEntity entity)
        {
            string sql = entity.GetReplaceInsertSQL();

            base.Excute(sql, entity);
        }

        public Task ReplaceIntoAsync(MySqlEntity entity)
        {
            string sql = entity.GetReplaceInsertSQL();

            return base.ExcuteAsync(sql, entity);
        }
    }
}
