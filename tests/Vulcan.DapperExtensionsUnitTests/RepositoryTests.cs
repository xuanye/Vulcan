using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Vulcan.DapperExtensionsUnitTests.Internal;
using Xunit;

namespace Vulcan.DapperExtensionsUnitTests
{
    [Collection("Database collection")]
    public class RepositoryTests 
    {
        public SharedDatabaseFixture SharedDatabaseFixture { get; }

        public Fixture AutoFixture { get; }

        public RepositoryTests(SharedDatabaseFixture fixture)
        {
            SharedDatabaseFixture = fixture;
            AutoFixture = new Fixture();
        }


        #region  synchronous
        [Fact]
        public void Insert_ShouldReturnAutoIncrement()
        {
            //arrange
            var testItem = AutoFixture.Create<TestItem>();
            var repository = SharedDatabaseFixture.Repository;
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
            var repository = SharedDatabaseFixture.Repository;
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
        public void Update_ShouldBeOk_InsertAndUpdateAndDeleteWithTimeout()
        {
            //arrange
            var testItem = AutoFixture.Create<TestItem>();
            var repository = SharedDatabaseFixture.Repository;
            const string updateName = "TEST NAME";
            //act
            var newId = repository.Insert(testItem);
            var id = (int)newId;
            var dbItem = repository.GetTestItem(id);
            dbItem.Name = updateName;
            var affectRow = repository.Update(dbItem);
            var dbItem2 = repository.GetTestItem(id);

            var affectRows2 = repository.Delete(id,600);
            var dbItem3 = repository.GetTestItem(id);

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
        public void Query_ShouldReturnRightList_WithOutCondition()
        {
            //arrange
            var repository = SharedDatabaseFixture.Repository;
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
            var repository = SharedDatabaseFixture.Repository;
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

        [Fact]
        public void Query_ShouldReturnList_WithConditionAndTimeout()
        {
            //arrange
            var testItemList = AutoFixture.CreateMany<TestItem>().ToList();
            var repository = SharedDatabaseFixture.Repository;
            //act
            var newId = repository.Insert(testItemList[0]);
            var list1 = repository.QueryTestItemListByGreaterThanId((int)newId,600);
            for (var i = 1; i < testItemList.Count; i++)
            {
                repository.Insert(testItemList[i]);
            }
            var list2 = repository.QueryTestItemListByGreaterThanId((int)newId,600);
            //assert
            Assert.NotNull(list1);
            Assert.Empty(list1);
            Assert.NotNull(list2);
            Assert.Equal(testItemList.Count - 1, list2.Count);
        }

        [Fact]
        public void CreateTransScope_ShouldBeSuccess_Commit()
        {
            //arrange
            var repository = SharedDatabaseFixture.Repository;
            long newId;
            var testItem = AutoFixture.Create<TestItem>();
            //act

            using (var scope = repository.CreateScope())
            {

                newId = repository.Insert(testItem);
                scope.Commit();
            }
            var dbItem = repository.GetTestItem((int)newId);
            //assert
            Assert.True(newId > 0);
            Assert.NotNull(dbItem);

            Assert.Equal(testItem.Name, dbItem.Name);
            Assert.Equal(testItem.Address, dbItem.Address);

        }


        [Fact]
        public void CreateTransScope_ShouldBeOk_Rollback()
        {
            //arrange
            long newId;
            var repository = SharedDatabaseFixture.Repository;
            var testItem = AutoFixture.Create<TestItem>();
            //act


            using (repository.CreateScope())
            {

                newId = repository.Insert(testItem);
                //don't commit
                //scope.Commit();
            }
            var dbItem = repository.GetTestItem((int)newId);
            //assert
            Assert.True(newId > 0);
            Assert.Null(dbItem);
           
        }


        [Fact]
        public void BatchInsert_ShouldReturnNegativeOne_PassEmptyList()
        {
            //arrange
            var list = new List<TestItem>();
            var repository = SharedDatabaseFixture.Repository;
            //act

            var ret= repository.BatchInsert(list);

            //assert
            Assert.Equal(-1, ret);
        }

        [Fact]
        public void BatchInsert_ShouldReturnGreaterThanZero_PassList()
        {
            //arrange
            var list = AutoFixture.CreateMany<TestItem>().ToList();
            var repository = SharedDatabaseFixture.Repository;
            //act

            var ret = repository.BatchInsert(list);

            //assert
            Assert.True(ret>0);
            Assert.Equal(list.Count, ret);
        }

        [Fact]
        public void BatchUpdate_ShouldReturnNegativeOne_PassEmptyList()
        {
            //arrange
            var list = new List<TestItem>();
            var repository = SharedDatabaseFixture.Repository;
            //act

            var ret = repository.BatchUpdate(list);

            //assert
            Assert.Equal(-1, ret);
        }

        [Fact]
        public void BatchUpdate_ShouldReturnGreaterThanZero_PassList()
        {
            //arrange
            var list = AutoFixture.CreateMany<TestItem>().ToList();
            var repository = SharedDatabaseFixture.Repository;
            //act
            repository.BatchInsert(list);
            var newList = repository.QueryTestItemList();

            newList.ForEach(item =>
            {
                item.Name = AutoFixture.Create<string>();
            });

            var ret = repository.BatchUpdate(newList);

            //assert
            Assert.True(ret > 0);
            Assert.Equal(newList.Count, ret);
        }
        #endregion


    }
}
