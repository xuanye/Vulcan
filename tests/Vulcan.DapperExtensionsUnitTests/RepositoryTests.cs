using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Vulcan.DapperExtensionsUnitTests.Internal;
using Xunit;

namespace Vulcan.DapperExtensionsUnitTests
{
    public class RepositoryTests : SharedDatabaseTest
    {
        public RepositoryTests(SharedDatabaseFixture fixture) : base(fixture)
        {
        }


        #region  synchronous
        [Fact]
        public void Insert_ShouldReturnAutoIncrement()
        {
            //arrange
            var testItem = AutoFixture.Create<TestItem>();
            var repository = base.SharedDatabaseFixture.Repository;
            //act
            var newId = repository.Insert(testItem);
            var dbItem = repository.GetTestItem((int)newId);
            //assert
            Assert.NotNull(dbItem);

            Assert.Equal(testItem.Name,dbItem.Name);
            Assert.Equal(testItem.Address,dbItem.Address);
        }

        [Fact]
        public void Update_ShouldBeOk_InsertAndUpdateAndDelete()
        {
            //arrange
            var testItem = AutoFixture.Create<TestItem>();
            var repository = base.SharedDatabaseFixture.Repository;
            const string updateName = "TEST NAME";
            //act
            var newId = repository.Insert(testItem);
            var id = (int) newId;
            var dbItem = repository.GetTestItem(id);
            dbItem.Name = updateName;
            var affectRow = repository.Update(dbItem);
            var dbItem2 = repository.GetTestItem(id);

            var affectRows2 = repository.Delete(id);
            var dbItem3 = repository.GetTestItem(id);

            //assert
            //updated
            Assert.NotNull(dbItem2);
            Assert.Equal(1,affectRow);
            Assert.Equal(updateName,dbItem.Name);

            //delete
            Assert.Equal(1,affectRows2);
            Assert.Null(dbItem3);

        }



        [Fact]
        public void Query_ShouldReturnRightList_WithOutCondition()
        {
            //arrange
            var repository = base.SharedDatabaseFixture.Repository;
            //act

            var list = repository.QueryTestItemList();

            //assert
            Assert.NotNull(list);
            Assert.True(list.Count>0);
        }

        [Fact]
        public void Query_ShouldReturnList_WithCondition()
        {
            //arrange
            var testItemList = AutoFixture.CreateMany<TestItem>().ToList();
            var repository = base.SharedDatabaseFixture.Repository;
            //act
            var newId = repository.Insert(testItemList[0]);
            var  list1 = repository.QueryTestItemListByGreaterThanId((int) newId);
            for (var i = 1; i < testItemList.Count; i++)
            {
                repository.Insert(testItemList[i]);
            }
            var  list2 = repository.QueryTestItemListByGreaterThanId((int) newId);
            //assert
            Assert.NotNull(list1);
            Assert.Empty(list1);
            Assert.NotNull(list2);
            Assert.Equal(testItemList.Count-1,list2.Count);
        }


        #endregion

        #region  asynchronous
        [Fact]
        public async Task InsertAsync_ShouldReturnAutoIncrement()
        {
            //arrange
            var testItem = AutoFixture.Create<TestItem>();
            var repository = base.SharedDatabaseFixture.Repository;
            //act
            var newId = await repository.InsertAsync(testItem);
            var dbItem = await repository.GetTestItemAsync((int)newId);
            //assert
            Assert.NotNull(dbItem);

            Assert.Equal(testItem.Name,dbItem.Name);
            Assert.Equal(testItem.Address,dbItem.Address);
        }

        [Fact]
        public async Task QueryAsync_ShouldReturnList_WithoutCondition()
        {
            //arrange
            var repository = base.SharedDatabaseFixture.Repository;
            //act
            var list = await repository.QueryTestItemListAsync();

            //assert
            Assert.NotNull(list);
            Assert.True(list.Count>0);
        }


        [Fact]
        public async Task UpdateAsync_ShouldBeOk_InsertAndUpdateAndDelete()
        {
            //arrange
            var testItem = AutoFixture.Create<TestItem>();
            var repository = base.SharedDatabaseFixture.Repository;
            const string updateName = "TEST NAME";
            //act
            var newId = await repository.InsertAsync(testItem);
            var id = (int) newId;
            var dbItem = await repository.GetTestItemAsync(id);
            dbItem.Name = updateName;
            var affectRow = await repository.UpdateAsync(dbItem);
            var dbItem2 =await repository.GetTestItemAsync(id);

            var affectRows2 = await repository.DeleteAsync(id);
            var dbItem3 =await repository.GetTestItemAsync(id);

            //assert
            //updated
            Assert.NotNull(dbItem2);
            Assert.Equal(1,affectRow);
            Assert.Equal(updateName,dbItem.Name);

            //delete
            Assert.Equal(1,affectRows2);
            Assert.Null(dbItem3);
        }

        [Fact]
        public async Task QueryAsync_ShouldReturnList_WithCondition()
        {
            //arrange
            var testItemList = AutoFixture.CreateMany<TestItem>().ToList();
            var repository = base.SharedDatabaseFixture.Repository;
            //act
            var newId =  await repository.InsertAsync(testItemList[0]);
            var  list1 = await repository.QueryTestItemListByGreaterThanIdAsync((int) newId);

            var newList = new List<TestItem>();
            for (var i = 1; i < testItemList.Count; i++)
            {
                newList.Add(testItemList[i]);
            }

            await repository.BatchInsertAsync(newList);

            var  list2 = await repository.QueryTestItemListByGreaterThanIdAsync((int) newId);
            //assert
            Assert.NotNull(list1);
            Assert.Empty(list1);
            Assert.NotNull(list2);
            Assert.Equal(testItemList.Count-1,list2.Count);
        }
        #endregion
    }
}
