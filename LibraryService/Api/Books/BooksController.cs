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
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost]
        public IActionResult CreateBook([FromBody] BookWriteViewModel viewModel)
        {
            if (viewModel.WorkId != null && !_bookService.WorkExists(viewModel.WorkId))
                return NotFound($"The Work Id {viewModel.WorkId} does not exist!");

            foreach (BAWriteViewModel bookAuthor in viewModel.BookAuthors)
                if (!_bookService.AuthorExists(bookAuthor.AuthorId))
                    return NotFound($"The Author Id {bookAuthor.AuthorId} does not exist!");

            _bookService.Create(WriteModelFromWriteViewModel(viewModel));

            return NoContent();
        }

        [HttpPost("{bookId:int}, {authorId:int}")]
        public IActionResult AddAuthor(int bookId, int authorId, AuthorRole authorRole)
        {
            if (!_bookService.Exists(bookId))
                return NotFound($"The Book Id {bookId} does not exist!");
            if (!_bookService.AuthorExists(authorId))
                return NotFound($"The Author Id {authorId} does not exist!");

            _bookService.AddBookAuthor(bookId, authorId, authorRole);

            return NoContent();
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var allBooks = _bookService
                .GetAll()
                .Select(book => ToViewModel(book));

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
            foreach (BAWriteViewModel bookAuthor in viewModel.BookAuthors)
                if (!_bookService.AuthorExists(bookAuthor.AuthorId))
                    return NotFound($"The Author Id {bookAuthor.AuthorId} does not exist!");

            _bookService.Update(id, WriteModelFromWriteViewModel(viewModel));

            return Ok();
        }

        private BookReadViewModel ToViewModel(Book book)
        {
            return new BookReadViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Format = book.Format,
                Language = book.Language,
                WorkId = book.WorkId,
                BookAuthors = book.BookAuthors.ToList()
            };
        }

        private BookWriteModel WriteModelFromWriteViewModel(BookWriteViewModel viewModel)
        {
            return new BookWriteModel()
            {
                Title = viewModel.Title,
                Format = viewModel.Format,
                Language = viewModel.Language,
                WorkId = viewModel.WorkId,
                BookAuthors = viewModel
                    .BookAuthors
                    .Select(model => new BAWriteModel
                    {
                        AuthorId = model.AuthorId,
                        AuthorRole = model.AuthorRole
                    }).ToList()
            };
        }
    }
}
