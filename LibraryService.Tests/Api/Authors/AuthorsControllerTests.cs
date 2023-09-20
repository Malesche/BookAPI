using LibraryService.Api.Authors;
using LibraryService.Api.Authors.ViewModels;
using LibraryService.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NSubstitute;

namespace LibraryService.Tests.Api.Authors
{
    public class AuthorsControllerTests
    {
        [Fact]
        public void CreateAuthor_ReturnsNoContent()
        {
            var authorService = Substitute.For<IAuthorService>();
            var controller = new AuthorsController(authorService);

            var writeViewModel = new AuthorWriteViewModel
            {
                Name = "Test"
            };
            var result = controller.CreateAuthor(writeViewModel);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void CreateAuthor_CallsService()
        {
            var authorService = Substitute.For<IAuthorService>();
            var controller = new AuthorsController(authorService);

            var writeViewModel = new AuthorWriteViewModel
            {
                Name = "Test"
            };
            controller.CreateAuthor(writeViewModel);

            authorService.Received(1).Create("Test");
        }

        [Fact]
        public void GetAllAuthors_ReturnsValidViewModels()
        {
            var authorService = Substitute.For<IAuthorService>();
            authorService.GetAll().Returns(new List<Author>
            {
                new Author() { Id = 1, Name = "Name1" },
                new Author() { Id = 2, Name = "Name2" },
                new Author() { Id = 3, Name = "Name3" }
            });
            var controller = new AuthorsController(authorService);

            var result = controller.GetAllAuthors();

            var okObjectResult = (OkObjectResult)result;
            var model = okObjectResult.Value;
            Assert.IsAssignableFrom<IList<AuthorReadViewModel>>(model);

            var viewModelCollection = ((IList<AuthorReadViewModel>)model);
            Assert.Equal(3, viewModelCollection.Count);
            Assert.Equal(1, viewModelCollection[0].Id);
            Assert.Equal("Name1", viewModelCollection[0].Name); 
            Assert.Equal(2, viewModelCollection[1].Id);
            Assert.Equal("Name2", viewModelCollection[1].Name); 
            Assert.Equal(3, viewModelCollection[2].Id);
            Assert.Equal("Name3", viewModelCollection[2].Name);
        }
    }
}
