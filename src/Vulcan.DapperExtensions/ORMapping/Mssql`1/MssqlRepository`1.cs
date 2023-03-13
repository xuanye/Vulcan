using System.Threading.Tasks;
using Vulcan.DapperExtensions.Contract;

namespace Vulcan.DapperExtensions.ORMapping.Mssql
{
    public class MssqlRepository : BaseRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MSSqlRepository" /> class.
        /// </summary>
        /// <param name="connectionManagerFactory">connectionManagerFactory</param>
        /// <param name="connectionString">The Connection String.</param>
        /// <param name="factory"> Connection Factory</param>
        protected MssqlRepository(IConnectionManagerFactory connectionManagerFactory, string connectionString,
            IConnectionFactory factory = null) : base(connectionManagerFactory, connectionString, factory)
        {
        }

        public PagedList<T> PagedQuery<T>(PageView view, string sqlColumns, string sqlTable, string sqlCondition,
            object param, string SqlPk, string SqlOrder)
        {
            var pList = new PagedList<T>();
            long totalCount = -1;
            if (view.PageIndex == 0)
            {
                var totalSql = $" select count(1) from {sqlTable} where 1=1 {sqlCondition}  ";
                totalCount = Get<int>(totalSql, param);
            }

            if (string.IsNullOrEmpty(SqlOrder)) SqlOrder = " ORDER BY " + SqlPk;
            var pageStartIndex = view.PageSize * view.PageIndex + 1;
            var pageEndIndex = view.PageSize * (view.PageIndex + 1);
            var Sql =
                $" select {sqlColumns},ROW_NUMBER() OVER({SqlOrder}) AS RowNumber  from {sqlTable} where 1=1  {sqlCondition} ";
            var pageSql =
                $" select * from ({Sql}) as PagedTable where RowNumber >={pageStartIndex}  and RowNumber<= {pageEndIndex}  ";
            pList.DataList = Query<T>(pageSql, param);
            pList.Total = (int)totalCount;
            pList.PageIndex = view.PageIndex;
            pList.PageSize = view.PageSize;
            return pList;
        }

        public async Task<PagedList<T>> PagedQueryAsync<T>(PageView view, string sqlColumns, string sqlTable, string sqlCondition,
      object param, string SqlPk, string SqlOrder)
        {
            var pList = new PagedList<T>();
            long totalCount = -1;
            if (view.PageIndex == 0)
            {
                var totalSql = $" select count(1) from {sqlTable} where 1=1 {sqlCondition}  ";
                totalCount = await GetAsync<int>(totalSql, param);
            }

            if (string.IsNullOrEmpty(SqlOrder)) SqlOrder = " ORDER BY " + SqlPk;
            var pageStartIndex = view.PageSize * view.PageIndex + 1;
            var pageEndIndex = view.PageSize * (view.PageIndex + 1);
            var Sql =
                $" select {sqlColumns},ROW_NUMBER() OVER({SqlOrder}) AS RowNumber  from {sqlTable} where 1=1  {sqlCondition} ";
            var pageSql =
                $" select * from ({Sql}) as PagedTable where RowNumber >={pageStartIndex}  and RowNumber<= {pageEndIndex}  ";
            pList.DataList = await QueryAsync<T>(pageSql, param);
            pList.Total = (int)totalCount;
            pList.PageIndex = view.PageIndex;
            pList.PageSize = view.PageSize;
            return pList;
        }
    }
}
