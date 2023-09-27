using LibraryService.Api.Books;
using LibraryService.Api.Books.Models;
using LibraryService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LibraryService.Tests.Api.Books
{
    public class BookServiceTests : TestWithSqliteBase
    {
        [Fact]
        public void Create_SavesToDb()
        {
            var service = new BookService(DbContext);
            var workId = CreateWork("WorkTitle");
            var author1Id = CreateAuthor("Author1");
            var author2Id = CreateAuthor("Author2");
            var authorList = new List<BookAuthorWriteModel>
            {
                new(){AuthorId = author1Id, AuthorRole = AuthorRole.Translator},
                new(){AuthorId = author2Id, AuthorRole = AuthorRole.Narrator}
            };
            var bookWriteModel = new BookWriteModel()
            {
                Title = "BookTitle",
                Format = "paperback",
                Language = "english",
                WorkId = workId,
                BookAuthors = authorList
            };

            service.Create(bookWriteModel);

            var book = DbContext.Books
                .Include(b => b.BookAuthors)
                .Single();
            Assert.Equal("BookTitle", book.Title);
            Assert.Equal("paperback", book.Format);
            Assert.Equal("english", book.Language);
            Assert.Equal(workId, book.WorkId);
            var bookAuthors = book.BookAuthors as BookAuthor[] ?? book.BookAuthors.ToArray();
            Assert.IsAssignableFrom<IEnumerable<BookAuthor>>(book.BookAuthors);
            Assert.Contains(bookAuthors, ba => ba.AuthorId == author1Id && ba.AuthorRole == AuthorRole.Translator);
            Assert.Contains(bookAuthors, ba => ba.AuthorId == author2Id && ba.AuthorRole == AuthorRole.Narrator);
        }

        [Fact]
        public void Update_SavesToDb()
        {
            var service = new BookService(DbContext);
            var bookId = CreateBook("TitleBeforeUpdate", "hardcover", "german");
            var workId = CreateWork("WorkTitle");
            var author1Id = CreateAuthor("Author1");
            var author2Id = CreateAuthor("Author2");
            var authorList = new List<BookAuthorWriteModel>
            {
                new(){AuthorId = author1Id, AuthorRole = AuthorRole.Translator},
                new(){AuthorId = author2Id, AuthorRole = AuthorRole.Narrator}
            };
            var bookWriteModel = new BookWriteModel
            {
                Title = "BookTitle",
                Format = "paperback",
                Language = "english",
                WorkId = workId,
                BookAuthors = authorList
            };

            service.Update(bookId, bookWriteModel);

            var book = DbContext.Books
                .Include(b => b.BookAuthors)
                .Single();
            Assert.Equal("BookTitle", book.Title);
            Assert.Equal("paperback", book.Format);
            Assert.Equal("english", book.Language);
            Assert.Equal(workId, book.WorkId);

            var bookAuthors = book.BookAuthors as BookAuthor[] ?? book.BookAuthors.ToArray();
            Assert.IsAssignableFrom<IEnumerable<BookAuthor>>(book.BookAuthors);
            Assert.Contains(bookAuthors, ba => ba.AuthorId == author1Id && ba.AuthorRole == AuthorRole.Translator);
            Assert.Contains(bookAuthors, ba => ba.AuthorId == author2Id && ba.AuthorRole == AuthorRole.Narrator);
        }

        [Fact]
        public void AddBookAuthor_SavesToDb()
        {
            var service = new BookService(DbContext);
            var bookId = CreateBook("BookTitle", "paperback", "english");
            var authorId = CreateAuthor("AuthorName");

            service.AddBookAuthor(bookId, authorId, AuthorRole.Author);

            var book = DbContext.Books
                .Include(b => b.BookAuthors)
                .Single();
            Assert.Equal("BookTitle", book.Title);
            Assert.Equal("paperback", book.Format);
            Assert.Equal("english", book.Language);
            var bookAuthors = book.BookAuthors as BookAuthor[] ?? book.BookAuthors.ToArray();
            Assert.IsAssignableFrom<IEnumerable<BookAuthor>>(bookAuthors);
            Assert.Equal(AuthorRole.Author, bookAuthors.First(ba => ba.Id == authorId).AuthorRole);
            Assert.Contains(bookAuthors, ba => ba.AuthorId == authorId && ba.AuthorRole == AuthorRole.Author);

        }

        [Fact]
        public void RemoveBookAuthorInRole_SavesToDb()
        {
            var bookId = CreateBook("BookTitle", "paperback", "english");
            var authorId = CreateAuthor("AuthorName");
            AddBookAuthorInRole(bookId, authorId, AuthorRole.Author);
            AddBookAuthorInRole(bookId, authorId, AuthorRole.Translator);
            AddBookAuthorInRole(bookId, authorId, AuthorRole.Narrator);
            var service = new BookService(DbContext);

            service.RemoveBookAuthorInRole(bookId, authorId, AuthorRole.Translator);

            var book = DbContext.Books
                .Include(b => b.BookAuthors)
                .Single();
            var bookAuthors = book.BookAuthors as BookAuthor[] ?? book.BookAuthors.ToArray();
            Assert.Equal(2, bookAuthors.Length);
            Assert.Contains(bookAuthors, ba => ba.AuthorId == authorId && ba.AuthorRole == AuthorRole.Author);
            Assert.Contains(bookAuthors, ba => ba.AuthorId == authorId && ba.AuthorRole == AuthorRole.Narrator);
        }

        [Fact]
        public void RemoveBookAuthor_SavesToDb()
        {
            var bookId = CreateBook("BookTitle", "paperback", "english");
            var authorId = CreateAuthor("AuthorName");
            AddBookAuthorInRole(bookId, authorId, AuthorRole.Author);
            AddBookAuthorInRole(bookId, authorId, AuthorRole.Translator);
            AddBookAuthorInRole(bookId, authorId, AuthorRole.Narrator);
            var service = new BookService(DbContext);

            service.RemoveBookAuthor(bookId, authorId);

            var book = DbContext.Books
                .Include(b => b.BookAuthors)
                .Single();
            var bookAuthors = book.BookAuthors as BookAuthor[] ?? book.BookAuthors.ToArray();
            Assert.Empty(bookAuthors);
        }

        [Fact]
        public void GetAll_ReturnsAllBooks()
        {
            var authorId = CreateAuthor("AuthorName");
            var workId = CreateWork("WorkTitle");
            var bookId1 = CreateBook("Book1", "paperback", "english", workId);
            var bookId2 = CreateBook("Book2", "hardback", "english", workId);
            var bookId3 = CreateBook("Book3", "audiobook", "english");
            AddBookAuthorInRole(bookId1, authorId, AuthorRole.Author);
            AddBookAuthorInRole(bookId2, authorId, AuthorRole.Translator);
            AddBookAuthorInRole(bookId2, authorId, AuthorRole.Narrator);
            var service = new BookService(DbContext);

            var getAllResult = service.GetAll();

            Assert.IsAssignableFrom<IEnumerable<Book>>(getAllResult);
            var allBooks = getAllResult as Book[] ?? getAllResult.ToArray();
            var book1 = allBooks.First(b => b.Id == bookId1);
            var book2 = allBooks.First(b => b.Id == bookId2);
            var book3 = allBooks.First(b => b.Id == bookId3);
            var book1Authors = book1.BookAuthors as BookAuthor[] ?? book1.BookAuthors.ToArray();
            var book2Authors = book2.BookAuthors as BookAuthor[] ?? book2.BookAuthors.ToArray();
            Assert.IsAssignableFrom<IEnumerable<BookAuthor>>(book1Authors);
            Assert.IsAssignableFrom<IEnumerable<BookAuthor>>(book2Authors);
            Assert.Equal("Book1", book1.Title);
            Assert.Equal("paperback", book1.Format);
            Assert.Equal("english", book1.Language);
            Assert.Equal(workId, book1.WorkId);
            Assert.Equal("Book2", book2.Title);
            Assert.Equal("hardback", book2.Format);
            Assert.Equal("english", book2.Language);
            Assert.Equal(workId, book2.WorkId);
            Assert.Equal("Book3", book3.Title);
            Assert.Equal("audiobook", book3.Format);
            Assert.Equal("english", book3.Language);
            Assert.Null(book3.WorkId);
            Assert.Contains(book1Authors, ba => ba.AuthorId == authorId && ba.AuthorRole == AuthorRole.Author);
            Assert.Contains(book2Authors, ba => ba.AuthorId == authorId && ba.AuthorRole == AuthorRole.Translator);
            Assert.Contains(book2Authors, ba => ba.AuthorId == authorId && ba.AuthorRole == AuthorRole.Narrator);
        }

        [Fact]
        public void Get_validId_ReturnsBook()
        {
            var bookId = CreateBook("BookTitle", "paperback", "english");
            var service = new BookService(DbContext);

            var book = service.Get(bookId);

            Assert.Equal("BookTitle", book.Title);
            Assert.Equal("paperback", book.Format);
            Assert.Equal("english", book.Language);
        }

        [Fact]
        public void Get_invalidId_ReturnsNull()
        {
            var service = new BookService(DbContext);

            var book = service.Get(1);

            Assert.Null(book);
        }

        [Fact]
        public void Exists_validId_ReturnsTrue()
        {
            var bookId = CreateBook("BookTitle", "paperback", "english");
            var service = new BookService(DbContext);

            var exists = service.Exists(bookId);

            Assert.True(exists);
        }

        [Fact]
        public void Exists_invalidId_ReturnsFalse()
        {
            var service = new BookService(DbContext);

            var exists = service.Exists(1);

            Assert.False(exists);
        }

        [Fact]
        public void WorkExists_validId_ReturnsTrue()
        {
            var workId = CreateWork("WorkTitle");
            var service = new BookService(DbContext);

            var exists = service.WorkExists(workId);

            Assert.True(exists);
        }

        [Fact]
        public void WorkExists_invalidId_ReturnsFalse()
        {
            var service = new BookService(DbContext);

            var exists = service.WorkExists(1);

            Assert.False(exists);
        }

        [Fact]
        public void AuthorExists_validId_ReturnsTrue()
        {
            var authorId = CreateAuthor("AuthorName");
            var service = new BookService(DbContext);

            var exists = service.AuthorExists(authorId);

            Assert.True(exists);
        }

        [Fact]
        public void AuthorExists_invalidId_ReturnsFalse()
        {
            var service = new BookService(DbContext);

            var exists = service.AuthorExists(1);

            Assert.False(exists);
        }

        [Fact]
        public void BookAuthorRoleExists_validId_ReturnsTrue()
        {
            var bookId = CreateBook("BookTitle", "paperback", "english");
            var authorId = CreateAuthor("AuthorName");
            AddBookAuthorInRole(bookId, authorId, AuthorRole.Author);
            var service = new BookService(DbContext);

            var exists = service.BookAuthorRoleExists(bookId, authorId, AuthorRole.Author);

            Assert.True(exists);
        }

        [Fact]
        public void BookAuthorRoleExists_AuthorIsNotSavedInThisRole_ReturnsFalse()
        {
            var bookId = CreateBook("BookTitle", "paperback", "english");
            var authorId = CreateAuthor("AuthorName");
            AddBookAuthorInRole(bookId, authorId, AuthorRole.Author);
            var service = new BookService(DbContext);

            var exists = service.BookAuthorRoleExists(bookId, authorId, AuthorRole.Translator);

            Assert.False(exists);
        }


        private int CreateBook(string title, string format, string language, int? workId = null)
        {
            using var dbContext = CreateDbContext();
            var book = new Book() { Title = title, Format = format, Language = language, WorkId = workId };
            dbContext.Books.Add(book);
            dbContext.SaveChanges();

            return book.Id;
        }

        private int CreateWork(string title)
        {
            using var dbContext = CreateDbContext();
            var work = new Work { Title = title };
            dbContext.Works.Add(work);
            dbContext.SaveChanges();

            return work.Id;
        }

        private int CreateAuthor(string name)
        {
            using var dbContext = CreateDbContext();
            var author = new Author { Name = name };
            dbContext.Authors.Add(author);
            dbContext.SaveChanges();

            return author.Id;
        }

        private void AddBookAuthorInRole(int bookId, int authorId, AuthorRole role)
        {
            using var dbContext = CreateDbContext();

            var bookAuthorEntry = new BookAuthor
            {
                BookId = bookId,
                AuthorId = authorId,
                AuthorRole = role
            };

            dbContext.BookAuthor.Add(bookAuthorEntry);
            dbContext.SaveChanges();
        }
    }
}
