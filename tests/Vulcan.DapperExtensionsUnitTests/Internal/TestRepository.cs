using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Vulcan.DapperExtensions;
using Vulcan.DapperExtensions.Contract;
using Vulcan.DapperExtensions.ORMapping;

namespace Vulcan.DapperExtensionsUnitTests.Internal
{
    public abstract class TestRepository:BaseRepository
    {
        protected TestRepository(IConnectionManagerFactory mgr, string constr, IConnectionFactory factory = null)
            : base(mgr, constr, factory)
        {
        }

        protected abstract string GetInitialSQL();
        public async Task InitialTestDb()
        {
            var sql = GetInitialSQL();
            await ExecuteAsync(sql, null);

            var fixture = new Fixture();
            var testItems = fixture.CreateMany<TestItem>().ToList();

            await base.BatchInsertAsync(testItems);
        }

        public TransScope CreateScope()
        {
            return base.BeginTransScope();
        }

        public ConnectionScope CreateConnectionScope()
        {
            return base.BeginConnectionScope();
        }

        public Task<TestItem> GetTestItemAsync(int Id)
        {
            const string sql = "select * from test_item_table where id=@Id ";
            return base.GetAsync<TestItem>(sql,new { Id});
        }


        public TestItem GetTestItem(int Id)
        {
            const string sql = "select * from test_item_table where id=@Id ";
            return base.Get<TestItem>(sql,new { Id});
        }

        public List<TestItem> QueryTestItemList()
        {
            const string sql = "select * from test_item_table";
            return base.Query<TestItem>(sql, null);
        }

        public Task<List<TestItem>> QueryTestItemListAsync()
        {
            const string sql = "select * from test_item_table";
            return base.QueryAsync<TestItem>(sql, null);
        }

        public List<TestItem> QueryTestItemListByGreaterThanId(int Id)
        {
            const string sql = "select * from test_item_table where id>@Id";
            return base.Query<TestItem>(sql,new { Id });
        }
        public Task<List<TestItem>> QueryTestItemListByGreaterThanIdAsync(int Id)
        {
            const string sql = "select * from test_item_table where id>@Id";
            return base.QueryAsync<TestItem>(sql,new { Id });
        }

        public int Delete(int Id)
        {
            const string sql = "delete from test_item_table where id=@Id";
            return base.Execute(sql,new { Id });
        }

        public Task<int> DeleteAsync(int Id)
        {   const string sql = "delete from test_item_table where id=@Id";
            return base.ExecuteAsync(sql,new { Id });
        }
    }
}
