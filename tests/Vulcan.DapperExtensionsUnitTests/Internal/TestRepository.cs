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

        protected abstract string GetInitialSQL(int type);
        public async Task InitialTestDbAsync()
        {
            var sql = GetInitialSQL(2);
            await ExecuteAsync(sql, null);

            var fixture = new Fixture();
            var testItems = fixture.CreateMany<AsyncTestItem>().ToList();

            await base.BatchInsertAsync(testItems);
        }

        public void InitialTestDb()
        {
            var sql = GetInitialSQL(1);
            Execute(sql, null);

            var fixture = new Fixture();
            var testItems = fixture.CreateMany<TestItem>().ToList();

            BatchInsert(testItems);
        }

        public TransScope CreateScope()
        {
            return base.BeginTransScope();
        }

        public ConnectionScope CreateConnectionScope()
        {
            return base.BeginConnectionScope();
        }

        public Task<AsyncTestItem> GetTestItemAsync(int Id)
        {
            const string sql = "select * from async_test_item where id=@Id ";
            return base.GetAsync<AsyncTestItem>(sql,new { Id});
        }


        public TestItem GetTestItem(int Id)
        {
            const string sql = "select * from test_item where id=@Id ";
            return base.Get<TestItem>(sql,new { Id});
        }

        public List<TestItem> QueryTestItemList()
        {
            const string sql = "select * from test_item";
            return base.Query<TestItem>(sql, null);
        }

        public Task<List<AsyncTestItem>> QueryTestItemListAsync()
        {
            const string sql = "select * from async_test_item";
            return base.QueryAsync<AsyncTestItem>(sql, null);
        }

        public List<TestItem> QueryTestItemListByGreaterThanId(int Id,int timeout=0)
        {
            const string sql = "select * from test_item where id>@Id";
            if (timeout > 0)
            {
                return base.Query<TestItem>(sql,timeout, new { Id });
            }
            return base.Query<TestItem>(sql,new { Id });
        }
        public Task<List<AsyncTestItem>> QueryTestItemListByGreaterThanIdAsync(int Id,int timeout=0)
        {
            const string sql = "select * from async_test_item where id>@Id";
            if (timeout > 0)
            {
                return base.QueryAsync<AsyncTestItem>(sql, timeout,new { Id });
            }
            return base.QueryAsync<AsyncTestItem>(sql,new { Id });
        }

        public int Delete(int Id,int timeout=0)
        {
            const string sql = "delete from test_item where id=@Id";
            if (timeout > 0)
            {
                return base.Execute(sql, timeout, new { Id });
            }
            return base.Execute(sql,new { Id });
        }

        public Task<int> DeleteAsync(int Id,int timeout=0)
        {   const string sql = "delete from async_test_item where id=@Id";
            if (timeout > 0)
            {
                return base.ExecuteAsync(sql, timeout, new { Id });
            }
            return base.ExecuteAsync(sql,new { Id });
        }
    }
}
