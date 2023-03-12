using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vulcan.DapperExtensions;
using Vulcan.DapperExtensionsUnitTests.Internal;
using Xunit;

namespace Vulcan.DapperExtensionsUnitTests
{
    [Collection("Async Database collection")]
    public class RepositoryAsyncTests : IDisposable
    {
        public AsyncSharedDatabaseFixture SharedDatabaseFixture { get; }
        public Fixture AutoFixture { get; }

        public RepositoryAsyncTests(AsyncSharedDatabaseFixture dbFixture)
        {
            AsyncLocalStorage.LocalValue = new Dictionary<string, object>();
            SharedDatabaseFixture = dbFixture;
            AutoFixture = new Fixture();
        }



        #region  asynchronous
        [Fact]
        public async Task InsertAsync_ShouldReturnAutoIncrement()
        {

            //arrange
            var testItem = AutoFixture.Create<AsyncTestItem>();
            var repository = SharedDatabaseFixture.Repository;
            //act            
            var newId = await repository.InsertAsync(testItem);
            var dbItem = await repository.GetTestItemAsync((int)newId);


            //assert
            Assert.NotNull(dbItem);

            Assert.Equal(testItem.Name, dbItem.Name);
            Assert.Equal(testItem.Address, dbItem.Address);


        }

        [Fact]
        public async Task QueryAsync_ShouldReturnList_WithoutCondition()
        {
            //arrange
            var repository = SharedDatabaseFixture.Repository;
            //act           

            var list = await repository.QueryTestItemListAsync();

            //assert
            Assert.NotNull(list);
            Assert.True(list.Count > 0);
        }


        [Fact]
        public async Task UpdateAsync_ShouldBeOk_InsertAndUpdateAndDelete()
        {
            //arrange
            var testItem = AutoFixture.Create<AsyncTestItem>();
            var repository = SharedDatabaseFixture.Repository;
            const string updateName = "TEST NAME";
            //act         
            var newId = await repository.InsertAsync(testItem);
            var id = (int)newId;
            var dbItem = await repository.GetTestItemAsync(id);
            dbItem.Name = updateName;
            var affectRow = await repository.UpdateAsync(dbItem);
            var dbItem2 = await repository.GetTestItemAsync(id);

            var affectRows2 = await repository.DeleteAsync(id);
            var dbItem3 = await repository.GetTestItemAsync(id);

            //assert
            //updated
            Assert.NotNull(dbItem2);
            Assert.Equal(1, affectRow);
            Assert.Equal(updateName, dbItem.Name);

            //delete
            Assert.Equal(1, affectRows2);
            Assert.Null(dbItem3);
        }


        [Fact]
        public async Task UpdateAsync_ShouldBeOk_InsertAndUpdateAndDeleteWithTimeout()
        {
            //arrange
            var testItem = AutoFixture.Create<AsyncTestItem>();
            var repository = SharedDatabaseFixture.Repository;
            const string updateName = "TEST NAME";
            //act         
            var newId = await repository.InsertAsync(testItem);
            var id = (int)newId;
            var dbItem = await repository.GetTestItemAsync(id);
            dbItem.Name = updateName;
            var affectRow = await repository.UpdateAsync(dbItem);
            var dbItem2 = await repository.GetTestItemAsync(id);

            var affectRows2 = await repository.DeleteAsync(id, 600);
            var dbItem3 = await repository.GetTestItemAsync(id);

            //assert
            //updated
            Assert.NotNull(dbItem2);
            Assert.Equal(1, affectRow);
            Assert.Equal(updateName, dbItem.Name);

            //delete
            Assert.Equal(1, affectRows2);
            Assert.Null(dbItem3);
        }

        [Fact]
        public async Task QueryAsync_ShouldReturnList_WithCondition()
        {
            //arrange
            var testItemList = AutoFixture.CreateMany<AsyncTestItem>().ToList();
            var repository = SharedDatabaseFixture.Repository;
            //act           
            var newId = await repository.InsertAsync(testItemList[0]);
            var list1 = await repository.QueryTestItemListByGreaterThanIdAsync((int)newId);

            var newList = new List<AsyncTestItem>();
            for (var i = 1; i < testItemList.Count; i++)
            {
                newList.Add(testItemList[i]);
            }

            await repository.BatchInsertAsync(newList);

            var list2 = await repository.QueryTestItemListByGreaterThanIdAsync((int)newId);
            //assert
            Assert.NotNull(list1);
            Assert.Empty(list1);
            Assert.NotNull(list2);
            Assert.Equal(testItemList.Count - 1, list2.Count);
        }

        [Fact]
        public async Task QueryAsync_ShouldReturnList_WithConditionAndTimeout()
        {
            //arrange
            var testItemList = AutoFixture.CreateMany<AsyncTestItem>().ToList();
            var repository = SharedDatabaseFixture.Repository;
            //act           
            var newId = await repository.InsertAsync(testItemList[0]);
            var list1 = await repository.QueryTestItemListByGreaterThanIdAsync((int)newId, 600);

            var newList = new List<AsyncTestItem>();
            for (var i = 1; i < testItemList.Count; i++)
            {
                newList.Add(testItemList[i]);
            }

            await repository.BatchInsertAsync(newList);

            var list2 = await repository.QueryTestItemListByGreaterThanIdAsync((int)newId, 600);
            //assert
            Assert.NotNull(list1);
            Assert.Empty(list1);
            Assert.NotNull(list2);
            Assert.Equal(testItemList.Count - 1, list2.Count);
        }


        [Fact]
        public async Task CreateTransScope_ShouldBeSuccess_Commit()
        {
            //arrange
            var repository = SharedDatabaseFixture.Repository;
            long newId;
            var testItem = AutoFixture.Create<AsyncTestItem>();
            //act        

            using (var scope = repository.CreateScope())
            {
                newId = await repository.InsertAsync(testItem);
                scope.Commit();
            }
            var dbItem = await repository.GetTestItemAsync((int)newId);
            //assert
            Assert.True(newId > 0);
            Assert.NotNull(dbItem);

            Assert.Equal(testItem.Name, dbItem.Name);
            Assert.Equal(testItem.Address, dbItem.Address);

        }


        [Fact]
        public async Task CreateTransScope_ShouldBeOk_Rollback()
        {
            //arrange
            long newId;
            var repository = SharedDatabaseFixture.Repository;
            var testItem = AutoFixture.Create<AsyncTestItem>();
            //act
            using (repository.CreateScope())
            {

                newId = await repository.InsertAsync(testItem);
                //don't commit
                //scope.Commit();
            }
            var dbItem = await repository.GetTestItemAsync((int)newId);
            //assert
            Assert.True(newId > 0);
            Assert.Null(dbItem);
        }



        [Fact]
        public async Task BatchInsert_ShouldReturnNegativeOne_PassEmptyList()
        {
            //arrange
            var list = new List<TestItem>();
            var repository = SharedDatabaseFixture.Repository;
            //act

            var ret = await repository.BatchInsertAsync(list);

            //assert
            Assert.Equal(-1, ret);
        }

        [Fact]
        public async Task BatchInsert_ShouldReturnGreaterThanZero_PassList()
        {
            //arrange
            var list = AutoFixture.CreateMany<TestItem>().ToList();
            var repository = SharedDatabaseFixture.Repository;
            //act

            var ret = await repository.BatchInsertAsync(list);

            //assert
            Assert.True(ret > 0);
            Assert.Equal(list.Count, ret);
        }

        [Fact]
        public async Task BatchUpdate_ShouldReturnNegativeOne_PassEmptyList()
        {
            //arrange
            var list = new List<TestItem>();
            var repository = SharedDatabaseFixture.Repository;
            //act

            var ret = await repository.BatchUpdateAsync(list);

            //assert
            Assert.Equal(-1, ret);
        }

        [Fact]
        public async Task BatchUpdate_ShouldReturnGreaterThanZero_PassList()
        {
            //arrange
            var list = AutoFixture.CreateMany<TestItem>().ToList();
            var repository = SharedDatabaseFixture.Repository;
            //act
            repository.BatchInsert(list);
            var newList = await repository.QueryTestItemListAsync();

            newList.ForEach(item =>
            {
                item.Name = AutoFixture.Create<string>();
            });

            var ret = await repository.BatchUpdateAsync(newList);

            //assert
            Assert.True(ret > 0);
            Assert.Equal(newList.Count, ret);
        }


        public void Dispose()
        {
            AsyncLocalStorage.LocalValue = null;
        }
        #endregion
    }
}
