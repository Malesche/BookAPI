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
