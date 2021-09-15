

namespace Vulcan.DataAccess.ORMapping.MSSql
{
    public class MSSqlRepository : BaseRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MSSqlRepository"/> class.
        /// </summary>
        /// <param name="connectionManagerFactory"></param>
        /// <param name="connectionString">The constr.</param>
        public MSSqlRepository(IConnectionManagerFactory connectionManagerFactory, string connectionString)
            : base(connectionManagerFactory,connectionString)
        {
        }

        protected MSSqlRepository(IConnectionManagerFactory connectionManagerFactory,IConnectionFactory factory, string constr)
            :base(connectionManagerFactory,factory, constr)
        {

        }
        public PagedList<T> PagedQuery<T>(PageView view, string sqlColumns, string sqlTable, string sqlCondition, object param, string sqlPk, string sqlOrder)
        {
            var pList = new PagedList<T>();
            long totalCount = -1;
            if (view.PageIndex == 0)
            {
                var totalSql = $" select count(1) from {sqlTable} where 1=1 {sqlCondition}  ";
                totalCount = Get<int>(totalSql, param);
            }

            if (string.IsNullOrEmpty(sqlOrder))
            {
                sqlOrder = " ORDER BY " + sqlPk;
            }
            var pageStartIndex = view.PageSize * view.PageIndex + 1;
            var pageEndIndex = view.PageSize * (view.PageIndex + 1);
            var sql =
                $" select {sqlColumns},ROW_NUMBER() OVER({sqlOrder}) AS RowNumber  from {sqlTable} where 1=1  {sqlCondition} ";
            var pageSql =
                $" select * from ({sql}) as PagedTable where RowNumber >={pageStartIndex}  and RowNumber<= {pageEndIndex}  ";
            pList.DataList = Query<T>(pageSql, param);
            pList.Total = (int)totalCount;
            pList.PageIndex = view.PageIndex;
            pList.PageSize = view.PageSize;
            return pList;
        }
    }
}