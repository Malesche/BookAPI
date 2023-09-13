using LibraryService.Api.Authors.ViewModels;
using LibraryService.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace LibraryService.Api.Authors
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookAuthorController : ControllerBase
    {
        private readonly LibraryDbContext _dbContext;

        public BookAuthorController(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult CreateBookAuthor(int authorId, int bookId)
        {
            var bookAuthor = new BookAuthor
            {
                AuthorId = authorId,
                BookId = bookId
            };

            _dbContext.BookAuthor.Add(bookAuthor);
            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpGet]
        public IActionResult GetAllBookAuthors()
        {
            var allBookAuthors = _dbContext
                .BookAuthor
                .ToArray();

            return Ok(allBookAuthors);
        }
    }
}

