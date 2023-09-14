using LibraryService.Api.Books.Models;
using LibraryService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LibraryService.Api.Books
{
    public class BookService
    {
        private readonly LibraryDbContext _dbContext;

        public BookService(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(BookWriteModel model)
        {
            var book = new Book();
            FillBook(book, model);

            _dbContext.Books.Add(book);
            _dbContext.SaveChanges();
        }

        public void AddBookAuthor(int bookId, int authorId, AuthorRole? authorRole)
        {
            var bookAuthorEntry = new BookAuthor
            {
                BookId = bookId,
                AuthorId = authorId,
                AuthorRole = authorRole
            };

            _dbContext.BookAuthor.Add(bookAuthorEntry);
            _dbContext.SaveChanges();
        }

        public void Update(int id, BookWriteModel model)
        {
            var book = _dbContext
                .Books
                .Include(b => b.BookAuthors)
                .First(b => b.Id == id);

            book.BookAuthors.Clear();
            FillBook(book, model);

            _dbContext.SaveChanges();
        }

        public IEnumerable<Book> GetAll()
        {
            return _dbContext
                .Books
                .ToArray();
        }

        public Book Get(int id)
        {
            var book = _dbContext
                .Books
                .Include(b => b.BookAuthors)
                .FirstOrDefault(a => a.Id == id);

            return book;
        }

        public bool Exists(int id)
        {
            return _dbContext
                .Books
                .Any(a => a.Id == id);
        }

        public bool WorkExists(int? id)
        {
            return _dbContext
                .Works
                .Any(a => a.Id == id);
        }

        public bool AuthorExists(int? id)
        {
            return _dbContext
                .Authors
                .Any(a => a.Id == id);
        }

        public bool BABookAuthorRoleExists(int bookId, int authorId, AuthorRole? authorRole)
        {
            return _dbContext
                .BookAuthor
                .Any(a => a.BookId == bookId && a.AuthorId == authorId && a.AuthorRole == authorRole);
        }

        private void FillBook(Book book, BookWriteModel model)
        {
            book.Title = model.Title;
            book.Format = model.Format;
            book.Language = model.Language;
            book.WorkId = model.WorkId;
            book.BookAuthors = model
                .BookAuthors
                .Select(item =>
                {
                    return new BookAuthor()
                    {
                        BookId = book.Id,
                        AuthorId = item.AuthorId,
                        AuthorRole = item.AuthorRole
                    };
                })
                .ToList();
        }
    }
}
