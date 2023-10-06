using LibraryService.Api.Books.Models;
using LibraryService.Api.Books.ViewModels;
using LibraryService.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace LibraryService.Api.Books
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost]
        public IActionResult CreateBook([FromBody] BookWriteViewModel viewModel)
        {
            if (viewModel.WorkId != null && !_bookService.WorkExists(viewModel.WorkId))
                return NotFound($"The Work Id {viewModel.WorkId} does not exist!");

            foreach (BookAuthorWriteViewModel bookAuthor in viewModel.BookAuthors)
                if (!_bookService.AuthorExists(bookAuthor.AuthorId))
                    return NotFound($"The Author Id {bookAuthor.AuthorId} does not exist!");

            _bookService.Create(WriteModelFromWriteViewModel(viewModel));

            return NoContent();
        }

        [HttpPost("{bookId:int}/Authors")]
        public IActionResult AddAuthor(int bookId, [FromBody]BookAuthorWriteViewModel viewModel)
        {
            if (!_bookService.Exists(bookId))
                return NotFound($"The Book Id {bookId} does not exist!");
            if (!_bookService.AuthorExists(viewModel.AuthorId))
                return NotFound($"The Author Id {viewModel.AuthorId} does not exist!");
            if (_bookService.BookAuthorRoleExists(bookId, viewModel.AuthorId, viewModel.AuthorRole))
                return Conflict($"Author {viewModel.AuthorId} is already saved in role {viewModel.AuthorRole} for Book {bookId}");

            _bookService.AddBookAuthor(bookId, viewModel.AuthorId, viewModel.AuthorRole);

            return NoContent();
        }

        [HttpDelete("{bookId:int}/Authors/{authorId:int}")]
        public IActionResult RemoveAuthor(int bookId, int authorId, [FromQuery]AuthorRole? role = null)
        {
            if (role is null)
            {
                if (!_bookService.BookAuthorExists(bookId, authorId))
                    return NotFound($"Author {authorId} is not saved for Book {bookId}");
                _bookService.RemoveBookAuthor(bookId, authorId);
            }
            else
            {
                if (!_bookService.BookAuthorRoleExists(bookId, authorId, role))
                    return NotFound($"Author {authorId} is not saved in role {role} for Book {bookId}");
                _bookService.RemoveBookAuthorInRole(bookId, authorId, role);
            }

            return NoContent();
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var allBooks = _bookService
                .GetAll()
                .Select(ToViewModel)
                .ToArray();

            return Ok(allBooks);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetBookById(int id)
        {
            if (!_bookService.Exists(id))
                return NotFound($"The Book Id {id} does not exist!");

            var book = _bookService.Get(id);

            return Ok(ToViewModel(book));
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateBook(int id, [FromBody] BookWriteViewModel viewModel)
        {
            if (!_bookService.Exists(id))
                return NotFound($"The Book Id {id} does not exist!");
            
            if (viewModel.WorkId != null && !_bookService.WorkExists(viewModel.WorkId))
                return NotFound($"The Work Id {viewModel.WorkId} does not exist!");

            var missingBookAuthor = viewModel
                .BookAuthors
                .FirstOrDefault(bookAuthor => !_bookService.AuthorExists(bookAuthor.AuthorId));

            if (missingBookAuthor != null)
            {
                return NotFound($"The Author Id {missingBookAuthor.AuthorId} does not exist!");
            }

            _bookService.Update(id, WriteModelFromWriteViewModel(viewModel));

            return NoContent();
        }

        private static BookReadViewModel ToViewModel(Book book)
        {
            return new BookReadViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Format = book.Format,
                Language = book.Language,
                Isbn = book.Isbn,
                Isbn13 = book.Isbn13,
                Description = book.Description,
                PubDate = book.PubDate,
                CoverUrl = book.CoverUrl,
                WorkId = book.WorkId,
                BookAuthors = book
                    .BookAuthors
                    .Select(ba => new BookAuthorReadViewModel
                    {
                        AuthorId = ba.AuthorId,
                        AuthorRole = ba.AuthorRole
                    }).ToList()
            };
        }

        private static BookWriteModel WriteModelFromWriteViewModel(BookWriteViewModel viewModel)
        {
            return new BookWriteModel()
            {
                Title = viewModel.Title,
                Format = viewModel.Format,
                Language = viewModel.Language,
                Isbn = viewModel.Isbn,
                Isbn13 = viewModel.Isbn13,
                Description = viewModel.Description,
                PubDate = viewModel.PubDate,
                CoverUrl = viewModel.CoverUrl,
                WorkId = viewModel.WorkId,
                BookAuthors = viewModel
                    .BookAuthors
                    .Select(model => new BookAuthorWriteModel
                    {
                        AuthorId = model.AuthorId,
                        AuthorRole = model.AuthorRole
                    }).ToList()
            };
        }
    }
}
