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

        protected abstract string GetInitialSql(int type);
        public async Task InitialTestDbAsync()
        {
            var Sql = GetInitialSql(2);
            await ExecuteAsync(Sql, null);

            var fixture = new Fixture();
            var testItems = fixture.CreateMany<AsyncTestItem>().ToList();

            await base.BatchInsertAsync(testItems);
        }

        public void InitialTestDb()
        {
            var Sql = GetInitialSql(1);
            Execute(Sql, null);

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
            const string Sql = "select * from async_test_item where id=@Id ";
            return base.GetAsync<AsyncTestItem>(Sql,new { Id});
        }


        public TestItem GetTestItem(int Id)
        {
            const string Sql = "select * from test_item where id=@Id ";
            return base.Get<TestItem>(Sql,new { Id});
        }

        public List<TestItem> QueryTestItemList()
        {
            const string Sql = "select * from test_item";
            return base.Query<TestItem>(Sql, null);
        }

        public Task<List<AsyncTestItem>> QueryTestItemListAsync()
        {
            const string Sql = "select * from async_test_item";
            return base.QueryAsync<AsyncTestItem>(Sql, null);
        }

        public List<TestItem> QueryTestItemListByGreaterThanId(int Id,int timeout=0)
        {
            const string Sql = "select * from test_item where id>@Id";
            if (timeout > 0)
            {
                return base.Query<TestItem>(Sql,timeout, new { Id });
            }
            return base.Query<TestItem>(Sql,new { Id });
        }
        public Task<List<AsyncTestItem>> QueryTestItemListByGreaterThanIdAsync(int Id,int timeout=0)
        {
            const string Sql = "select * from async_test_item where id>@Id";
            if (timeout > 0)
            {
                return base.QueryAsync<AsyncTestItem>(Sql, timeout,new { Id });
            }
            return base.QueryAsync<AsyncTestItem>(Sql,new { Id });
        }

        public int Delete(int Id,int timeout=0)
        {
            const string Sql = "delete from test_item where id=@Id";
            if (timeout > 0)
            {
                return base.Execute(Sql, timeout, new { Id });
            }
            return base.Execute(Sql,new { Id });
        }

        public Task<int> DeleteAsync(int Id,int timeout=0)
        {   const string Sql = "delete from async_test_item where id=@Id";
            if (timeout > 0)
            {
                return base.ExecuteAsync(Sql, timeout, new { Id });
            }
            return base.ExecuteAsync(Sql,new { Id });
        }
    }
}
