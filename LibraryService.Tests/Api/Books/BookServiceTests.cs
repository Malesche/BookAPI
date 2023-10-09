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
            var workId = CreateWork("WorkTitle", null);
            var author1Id = CreateAuthor("Author1", null, null, null);
            var author2Id = CreateAuthor("Author2", null, null, null);
            var authorList = new List<BookAuthorWriteModel>
            {
                new(){AuthorId = author1Id, AuthorRole = AuthorRole.Translator},
                new(){AuthorId = author2Id, AuthorRole = AuthorRole.Narrator}
            };
            var bookWriteModel = new BookWriteModel
            {
                Title = "BookTitle",
                Format = BookFormat.Paperback,
                Language = "english",
                Isbn = "3902866063",
                Isbn13 = "9783902866066",
                Description = "describing_describing",
                PubDate = new DateTimeOffset(2013, 3, 4, 7, 0, 0, TimeSpan.FromHours(-7)),
                CoverUrl = "someUrl",
                WorkId = workId,
                BookAuthors = authorList
            };

            service.Create(bookWriteModel);

            var book = DbContext.Books
                .Include(b => b.BookAuthors)
                .Single();
            Assert.Equal("BookTitle", book.Title);
            Assert.Equal(BookFormat.Paperback, book.Format);
            Assert.Equal("english", book.Language);
            Assert.Equal("3902866063", book.Isbn);
            Assert.Equal("9783902866066", book.Isbn13);
            Assert.Equal("describing_describing", book.Description);
            Assert.Equal(new DateTimeOffset(2013, 3, 4, 7, 0, 0, TimeSpan.FromHours(-7)), book.PubDate);
            Assert.Equal("someUrl", book.CoverUrl);
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
            var bookId = CreateBook("TitleBeforeUpdate", BookFormat.Paperback, "german", null, null, null, null, null);
            var workId = CreateWork("WorkTitle", null);
            var author1Id = CreateAuthor("Author1", null, null, null);
            var author2Id = CreateAuthor("Author2", null, null, null);
            var authorList = new List<BookAuthorWriteModel>
            {
                new(){AuthorId = author1Id, AuthorRole = AuthorRole.Translator},
                new(){AuthorId = author2Id, AuthorRole = AuthorRole.Narrator}
            };
            var bookWriteModel = new BookWriteModel
            {
                Title = "BookTitle",
                Format = BookFormat.Paperback,
                Language = "english",
                Isbn = "3902866063",
                Isbn13 = "9783902866066",
                Description = "describing_describing",
                PubDate = new DateTimeOffset(2013, 3, 4, 7, 0, 0, TimeSpan.FromHours(-7)),
                CoverUrl = "someUrl",
                WorkId = workId,
                BookAuthors = authorList
            };

            service.Update(bookId, bookWriteModel);

            var book = DbContext.Books
                .Include(b => b.BookAuthors)
                .Single();
            Assert.Equal("BookTitle", book.Title);
            Assert.Equal(BookFormat.Paperback, book.Format);
            Assert.Equal("english", book.Language);
            Assert.Equal("3902866063", book.Isbn);
            Assert.Equal("9783902866066", book.Isbn13);
            Assert.Equal("describing_describing", book.Description);
            Assert.Equal(new DateTimeOffset(2013, 3, 4, 7, 0, 0, TimeSpan.FromHours(-7)), book.PubDate);
            Assert.Equal("someUrl", book.CoverUrl);
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
            var bookId = CreateBook("BookTitle", BookFormat.Paperback, "english", null, null, null, null, null);
            var authorId = CreateAuthor("AuthorName", null, null, null);

            service.AddBookAuthor(bookId, authorId, AuthorRole.Author);

            var book = DbContext.Books
                .Include(b => b.BookAuthors)
                .Single();
            Assert.Equal("BookTitle", book.Title);
            var bookAuthors = book.BookAuthors as BookAuthor[] ?? book.BookAuthors.ToArray();
            Assert.IsAssignableFrom<IEnumerable<BookAuthor>>(bookAuthors);
            Assert.Equal(AuthorRole.Author, bookAuthors.First(ba => ba.Id == authorId).AuthorRole);
            Assert.Contains(bookAuthors, ba => ba.AuthorId == authorId && ba.AuthorRole == AuthorRole.Author);
        }

        [Fact]
        public void RemoveBookAuthorInRole_SavesToDb()
        {
            var bookId = CreateBook("BookTitle", BookFormat.Paperback, "english", null, null, null, null, null);
            var authorId = CreateAuthor("AuthorName", null, null, null);
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
            var bookId = CreateBook("BookTitle", BookFormat.Paperback, "english", null, null, null, null, null);
            var authorId = CreateAuthor("AuthorName", null, null, null);
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
            var authorId = CreateAuthor("AuthorName", "bio", null, null);
            var workId = CreateWork("WorkTitle", null);
            var pubDate1 = new DateTimeOffset(2001, 1, 1, 7, 0, 0, TimeSpan.FromHours(-7));
            var pubDate2 = new DateTimeOffset(2002, 2, 2, 7, 0, 0, TimeSpan.FromHours(-7));
            var pubDate3 = new DateTimeOffset(2003, 3, 4, 7, 0, 0, TimeSpan.FromHours(-7));
            var bookId1 = CreateBook("Book1", BookFormat.Paperback, "english", "11", "111", "description1", pubDate1, "coverURL1", workId);
            var bookId2 = CreateBook("Book2", BookFormat.Hardcover, "english", "22", "222", "description2", pubDate2, "coverURL2", workId);
            var bookId3 = CreateBook("Book3", BookFormat.Audiobook, "english", "33", "333", "description3", pubDate3, "coverURL3");
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
            Assert.Equal(BookFormat.Paperback, book1.Format);
            Assert.Equal("english", book1.Language);
            Assert.Equal("11", book1.Isbn);
            Assert.Equal("111", book1.Isbn13);
            Assert.Equal("description1", book1.Description);
            Assert.Equal(pubDate1, book1.PubDate);
            Assert.Equal("coverURL1", book1.CoverUrl);
            Assert.Equal(workId, book1.WorkId);
            Assert.Equal("Book2", book2.Title);
            Assert.Equal(BookFormat.Hardcover, book2.Format);
            Assert.Equal("english", book2.Language);
            Assert.Equal("22", book2.Isbn);
            Assert.Equal("222", book2.Isbn13);
            Assert.Equal("description2", book2.Description);
            Assert.Equal(pubDate2, book2.PubDate);
            Assert.Equal("coverURL2", book2.CoverUrl);
            Assert.Equal(workId, book2.WorkId);
            Assert.Equal("Book3", book3.Title);
            Assert.Equal(BookFormat.Audiobook, book3.Format);
            Assert.Equal("english", book3.Language);
            Assert.Equal("33", book3.Isbn);
            Assert.Equal("333", book3.Isbn13);
            Assert.Equal("description3", book3.Description);
            Assert.Equal(pubDate3, book3.PubDate);
            Assert.Equal("coverURL3", book3.CoverUrl);
            Assert.Null(book3.WorkId);
            Assert.Contains(book1Authors, ba => ba.AuthorId == authorId && ba.AuthorRole == AuthorRole.Author);
            Assert.Contains(book2Authors, ba => ba.AuthorId == authorId && ba.AuthorRole == AuthorRole.Translator);
            Assert.Contains(book2Authors, ba => ba.AuthorId == authorId && ba.AuthorRole == AuthorRole.Narrator);
        }

        [Fact]
        public void Get_validId_ReturnsBook()
        {
            var pubDate = new DateTimeOffset(2013, 3, 4, 7, 0, 0, TimeSpan.FromHours(-7));
            var bookId = CreateBook("BookTitle", BookFormat.Paperback, "english", "11", "111", "description", pubDate, "coverURL");
            var service = new BookService(DbContext);

            var book = service.Get(bookId);

            Assert.Equal("BookTitle", book.Title);
            Assert.Equal(BookFormat.Paperback, book.Format);
            Assert.Equal("english", book.Language);
            Assert.Equal("11", book.Isbn);
            Assert.Equal("111", book.Isbn13);
            Assert.Equal("description", book.Description);
            Assert.Equal(pubDate, book.PubDate);
            Assert.Equal("coverURL", book.CoverUrl);
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
            var bookId = CreateBook("BookTitle", BookFormat.Paperback, "english", null, null, null, null, null);
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
            var workId = CreateWork("WorkTitle", null);
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
            var authorId = CreateAuthor("AuthorName", null, null, null);
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
            var bookId = CreateBook("BookTitle", BookFormat.Paperback, "english", null, null, null, null, null);
            var authorId = CreateAuthor("AuthorName", null, null, null);
            AddBookAuthorInRole(bookId, authorId, AuthorRole.Author);
            var service = new BookService(DbContext);

            var exists = service.BookAuthorRoleExists(bookId, authorId, AuthorRole.Author);

            Assert.True(exists);
        }

        [Fact]
        public void BookAuthorRoleExists_AuthorIsNotSavedInThisRole_ReturnsFalse()
        {
            var bookId = CreateBook("BookTitle", BookFormat.Paperback, "english", null, null, null, null, null);
            var authorId = CreateAuthor("AuthorName", null, null, null);
            AddBookAuthorInRole(bookId, authorId, AuthorRole.Author);
            var service = new BookService(DbContext);

            var exists = service.BookAuthorRoleExists(bookId, authorId, AuthorRole.Translator);

            Assert.False(exists);
        }


        private int CreateBook(string title, BookFormat? format, string language, string isbn, string isbn13, string description, DateTimeOffset? pubDate, string coverUrl, int? workId = null)
        {
            using var dbContext = CreateDbContext();
            var book = new Book()
            {
                Title = title, 
                Format = format, 
                Language = language,
                Isbn = isbn,
                Isbn13 = isbn13,
                Description = description,
                PubDate = pubDate,
                CoverUrl = coverUrl,
                WorkId = workId
            };
            dbContext.Books.Add(book);
            dbContext.SaveChanges();

            return book.Id;
        }

        private int CreateWork(string title, DateTimeOffset? earliestPubDate)
        {
            using var dbContext = CreateDbContext();
            var work = new Work
            {
                Title = title,
                EarliestPubDate = earliestPubDate
            };
            dbContext.Works.Add(work);
            dbContext.SaveChanges();

            return work.Id;
        }

        private int CreateAuthor(string name, string bio, DateTimeOffset? birthDate, DateTimeOffset? deathDate)
        {
            using var dbContext = CreateDbContext();
            var author = new Author
            {
                Name = name,
                Biography = bio,
                BirthDate = birthDate,
                DeathDate = deathDate
            };
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
