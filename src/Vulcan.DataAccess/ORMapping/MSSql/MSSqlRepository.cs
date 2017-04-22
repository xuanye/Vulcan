

namespace Vulcan.DataAccess.ORMapping.MSSql
{
    public class MSSqlRepository : BaseRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MSSqlRepository"/> class.
        /// </summary>
        /// <param name="constr">The constr.</param>
        public MSSqlRepository(string constr)
            : base(constr)
        {
        }

        protected MSSqlRepository(IConnectionFactory factory, string constr)
            :base(factory,constr)
        {
            
        }
        public PagedList<T> PagedQuery<T>(PageView view, string sqlColumns, string sqlTable, string sqlCondition, object param, string sqlPk, string sqlOrder)
        {
            PagedList<T> pList = new PagedList<T>();
            long totalCount = -1;
            if (view.PageIndex == 0)
            {
                string totalSql = string.Format(" select count(1) from {0} where 1=1 {1}  ", sqlTable, sqlCondition);
                totalCount = Get<int>(totalSql, param);
            }

            if (string.IsNullOrEmpty(sqlOrder))
            {
                sqlOrder = " ORDER BY " + sqlPk;
            }
            int pageStartIndex = view.PageSize * view.PageIndex + 1;
            int pageEndIndex = view.PageSize * (view.PageIndex + 1);
            string sql = string.Format(" select {0},ROW_NUMBER() OVER({1}) AS RowNumber  from {2} where 1=1  {3} ", sqlColumns, sqlOrder, sqlTable, sqlCondition);
            string pageSql =
                string.Format(" select * from ({0}) as pagetable where RowNumber >={1}  and RowNumber<= {2}  ", sql, pageStartIndex, pageEndIndex);
            pList.DataList = Query<T>(pageSql, param);
            pList.Total = (int)totalCount;
            pList.PageIndex = view.PageIndex;
            pList.PageSize = view.PageSize;
            return pList;
        }
    }
}