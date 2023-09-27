using LibraryService.Api.Works;
using LibraryService.Persistence;

namespace LibraryService.Tests.Api.Works
{
    public class WorkServiceTests : TestWithSqliteBase
    {
        [Fact]
        public void Create_SavesToDb()
        {
            var service = new WorkService(DbContext);

            service.Create("WorkTitle");

            var work = DbContext.Works.Single();
            Assert.Equal("WorkTitle", work.Title);
        }

        [Fact]
        public void Update_SavesToDb()
        {
            var workId = CreateWork("TitleBeforeUpdate");
            var service = new WorkService(DbContext);

            service.Update(workId, "TitleForUpdate");

            var work = DbContext.Works.Single();
            Assert.Equal("TitleForUpdate", work.Title);
        }

        [Fact]
        public void GetAll_ReturnsAllWorks()
        {
            var workId1 = CreateWork("Title1");
            var workId2 = CreateWork("Title2");
            var workId3 = CreateWork("Title3");
            var service = new WorkService(DbContext);

            var getAllResult = service.GetAll();
            
            Assert.IsAssignableFrom<IEnumerable<Work>>(getAllResult);
            var works = getAllResult as Work[] ?? getAllResult.ToArray();
            Assert.Equal("Title1", works.First(w => w.Id == workId1).Title);
            Assert.Equal("Title2", works.First(w => w.Id == workId2).Title);
            Assert.Equal("Title3", works.First(w => w.Id == workId3).Title);
        }

        [Fact]
        public void Get_validId_ReturnsWork()
        {
            var workId = CreateWork("WorkTitle");
            var service = new WorkService(DbContext);

            var work = service.Get(workId);

            Assert.Equal("WorkTitle", work.Title);
        }

        [Fact]
        public void Get_invalidId_ReturnsNull()
        {
            var service = new WorkService(DbContext);

            var work = service.Get(1);

            Assert.Null(work);
        }

        [Fact]
        public void Exists_validId_ReturnsTrue()
        {
            var workId = CreateWork("WorkTitle");
            var service = new WorkService(DbContext);

            var exists = service.Exists(workId);

            Assert.True(exists);
        }

        [Fact]
        public void Exists_invalidId_ReturnsFalse()
        {
            var service = new WorkService(DbContext);

            var exists = service.Exists(1);

            Assert.False(exists);
        }
        
        private int CreateWork(string title)
        {
            using var dbContext = CreateDbContext();
            var work = new Work() { Title = title };
            dbContext.Works.Add(work);
            dbContext.SaveChanges();

            return work.Id;
        }
    }
}
