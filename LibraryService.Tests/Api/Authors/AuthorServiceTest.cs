using LibraryService.Api.Authors;
using LibraryService.Persistence;

namespace LibraryService.Tests.Api.Authors
{
    public class AuthorServiceTest : TestWithSqliteBase
    {
        [Fact]
        public void Create_SavesToDb()
        {
            var service = new AuthorService(DbContext);

            service.Create("NameForCreate");

            var author = DbContext.Authors.Single();
            Assert.Equal("NameForCreate", author.Name);
        }

        [Fact]
        public void Update_SavesToDb()
        {
            var authorId = CreateAuthor("NameBeforeUpdate");
            AuthorService service = new AuthorService(DbContext);
            
            service.Update(authorId, "NameForUpdate");

            var author = DbContext.Authors.Single();           
            Assert.Equal("NameForUpdate", author.Name);
        }

        [Fact]
        public void GetAll_ReturnsAllAuthors()
        {
            var authorId1 = CreateAuthor("Name1");
            var authorId2 = CreateAuthor("Name2");
            var authorId3 = CreateAuthor("Name3");
            AuthorService service = new AuthorService(DbContext);

            var authors = service.GetAll();

            Assert.IsAssignableFrom<IEnumerable<Author>>(authors);
            
            var enumerable = authors as Author[] ?? authors.ToArray();

            Assert.Equal("Name1", enumerable.First(a => a.Id == authorId1).Name);
            Assert.Equal("Name2", enumerable.First(a => a.Id == authorId2).Name);
            Assert.Equal("Name3", enumerable.First(a => a.Id == authorId3).Name);
        }

        [Fact]
        public void Get_validId_ReturnsAuthor()
        {
            var authorId = CreateAuthor("AuthorName");
            AuthorService service = new AuthorService(DbContext);

            var author = service.Get(authorId);

            Assert.Equal("AuthorName", author.Name);
        }

        [Fact]
        public void Get_invalidId_ReturnsNull()
        {
            AuthorService service = new AuthorService(DbContext);

            var author = service.Get(1);

            Assert.Null(author);
        }

        [Fact]
        public void Exists_validId_ReturnsTrue()
        {
            var authorId = CreateAuthor("AuthorName");
            AuthorService service = new AuthorService(DbContext);

            var exists = service.Exists(authorId);

            Assert.True(exists);
        }

        [Fact]
        public void Exists_invalidId_ReturnsFalse()
        {
            AuthorService service = new AuthorService(DbContext);

            var exists = service.Exists(1);

            Assert.False(exists);
        }

        private int CreateAuthor(string name)
        {
            using var dbContext = CreateDbContext();
            var author = new Author() { Name = name };
            dbContext.Authors.Add(author);
            dbContext.SaveChanges();

            return author.Id;
        }
    }
}
