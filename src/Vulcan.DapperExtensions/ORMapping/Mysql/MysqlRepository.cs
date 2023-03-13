using System.Collections.Generic;
using System.Threading.Tasks;
using Vulcan.DapperExtensions.Contract;

namespace Vulcan.DapperExtensions.ORMapping.Mysql
{
    public class MysqlRepository : BaseRepository
    {
        protected MysqlRepository(IConnectionManagerFactory mgr, string constr, IConnectionFactory factory) :
            base(mgr, constr, factory)
        {
        }


        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <typeparam name="T">返回列表的实体对象类型</typeparam>
        /// <param name="view">分页查询信息</param>
        /// <param name="SqlColumns">查询的字符串</param>
        /// <param name="SqlTable">查询的表，可以为多表，即From后面 where之前的内容</param>
        /// <param name="SqlCondition">查询条件 Where 后面的部分.</param>
        /// <param name="param">查询实体值得实体对象.</param>
        /// <param name="SqlPk">该条查询的唯一键.</param>
        /// <param name="SqlOrder">排序字段 包含Order by.</param>
        /// <returns>返回分页信息，当查询为第一页时 返回总记录数</returns>
        public PagedList<T> PagedQuery<T>(PageView view, string sqlColumns, string sqlTable, string sqlCondition,
            object param, string sqlPk, string sqlOrder)
        {
            var pList = new PagedList<T>();
            long totalCount = -1;
            if (view.PageIndex == 0)
            {
                var totalSql = $" select count(1) from {sqlTable} where 1=1 {sqlCondition} ;";
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

            if (string.IsNullOrEmpty(sqlOrder)) sqlOrder = " ORDER BY " + sqlPk;
            var pageStartIndex = view.PageSize * view.PageIndex;
            var currentPageCount = view.PageSize;
            var Sql =
                $" select {sqlColumns} from {sqlTable} where 1=1  {sqlCondition} {sqlOrder} limit {pageStartIndex},{currentPageCount} ;";

            pList.DataList = Query<T>(Sql, param);
            pList.Total = (int)totalCount;
            pList.PageIndex = view.PageIndex;
            pList.PageSize = view.PageSize;
            return pList;
        }

        public async Task<PagedList<T>> PagedQueryAsync<T>(PageView view, string sqlColumns, string sqlTable,
            string sqlCondition, object param, string sqlPk, string sqlOrder)
        {
            var pList = new PagedList<T>();
            long totalCount = -1;
            if (view.PageIndex == 0)
            {
                var totalSql = $" select count(1) from {sqlTable} where 1=1 {sqlCondition} ;";
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

            if (string.IsNullOrEmpty(sqlOrder)) sqlOrder = " ORDER BY " + sqlPk;
            var pageStartIndex = view.PageSize * view.PageIndex;
            var currentPageCount = view.PageSize;
            var Sql =
                $" select {sqlColumns} from {sqlTable} where 1=1  {sqlCondition} {sqlOrder} limit {pageStartIndex},{currentPageCount} ;";

            pList.DataList = await QueryAsync<T>(Sql, param);
            pList.Total = (int)totalCount;
            pList.PageIndex = view.PageIndex;
            pList.PageSize = view.PageSize;
            return pList;
        }

        public void ReplaceInto(MysqlEntity entity)
        {
            var Sql = entity.GetReplaceInsertSql();

            Execute(Sql, entity);
        }

        public Task ReplaceIntoAsync(MysqlEntity entity)
        {
            var Sql = entity.GetReplaceInsertSql();

            return ExecuteAsync(Sql, entity);
        }
    }
}
