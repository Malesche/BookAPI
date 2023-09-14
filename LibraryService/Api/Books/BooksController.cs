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

        [HttpPost("{bookId:int}/Authors")]
        public IActionResult AddAuthor(int bookId, [FromBody]BAWriteViewModel viewModel)
        {
            if (!_bookService.Exists(bookId))
                return NotFound($"The Book Id {bookId} does not exist!");
            if (!_bookService.AuthorExists(viewModel.AuthorId))
                return NotFound($"The Author Id {viewModel.AuthorId} does not exist!");
            if (_bookService.BABookAuthorRoleExists(bookId, viewModel.AuthorId, viewModel.AuthorRole))
                return Conflict($"Author {viewModel.AuthorId} is already saved in role {viewModel.AuthorRole} for Book {bookId}");

            _bookService.AddBookAuthor(bookId, viewModel.AuthorId, viewModel.AuthorRole);

            return NoContent();
        }

        [HttpDelete("{bookId:int}/Authors/{authorId:int}")]
        public IActionResult RemoveAuthor(int bookId, int authorId, [FromQuery]AuthorRole? role = null)
        {
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
                BookAuthors = book
                    .BookAuthors
                    .Select(ba => new BAReadViewModel
                    {
                        AuthorId = ba.AuthorId,
                        AuthorRole = ba.AuthorRole
                    }).ToList()
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
