using LibraryService.Api.Authors.Models;
using LibraryService.Api.Authors.ViewModels;
using LibraryService.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace LibraryService.Api.Authors
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpPost]
        public IActionResult CreateAuthor([FromBody]AuthorWriteViewModel viewModel)
        {
            _authorService.Create(WriteModelFromWriteViewModel(viewModel));
            return NoContent();
        }

        [HttpGet]
        public IActionResult GetAllAuthors()
        {
            var allAuthors = _authorService
                .GetAll()
                .Select(ToViewModel)
                .ToArray();

            return Ok(allAuthors);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetAuthorById(int id)
        {
            var author = _authorService.Get(id);
            if (author == null)
            {
                return NotFound();
            }

            var viewModel = ToViewModel(author);

            return Ok(viewModel);
        }

        //[HttpGet("{id:int}")]
        //public IActionResult GetBooksByAuthorId(int id)
        //{
        //    var author = _authorService.Get(id);
        //    if (author == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(author.Books);
        //}

        [HttpPut("{id:int}")]
        public IActionResult UpdateAuthor(int id, [FromBody] AuthorWriteViewModel viewModel)
        {
            if (!_authorService.Exists(id))
                return NotFound($"The Author Id {id} does not exist!");

            _authorService.Update(id, WriteModelFromWriteViewModel(viewModel));

            return NoContent();
        }

        private AuthorReadViewModel ToViewModel(Author author)
        {
            return new AuthorReadViewModel
            {
                Id = author.Id,
                Name = author.Name,
                Biography = author.Biography,
                BirthDate = author.BirthDate,
                DeathDate = author.DeathDate
            };
        }

        private static AuthorWriteModel WriteModelFromWriteViewModel(AuthorWriteViewModel viewModel)
        {
            return new AuthorWriteModel
            {
                Name = viewModel.Name,
                Biography = viewModel.Biography,
                BirthDate = viewModel.BirthDate,
                DeathDate = viewModel.DeathDate
            };
        }
    }
}
