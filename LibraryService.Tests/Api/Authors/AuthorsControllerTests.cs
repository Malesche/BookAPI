using LibraryService.Api.Authors;
using LibraryService.Api.Authors.Models;
using LibraryService.Api.Authors.ViewModels;
using LibraryService.Persistence;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace LibraryService.Tests.Api.Authors
{
    public class AuthorsControllerTests
    {
        [Fact]
        public void CreateAuthor_ReturnsValidViewModel()
        {
            var authorService = Substitute.For<IAuthorService>();
            var birthDate2 = new DateTimeOffset(819, 1, 2, 7, 0, 0, TimeSpan.FromHours(-7));
            var deathDate2 = new DateTimeOffset(880, 2, 2, 7, 0, 0, TimeSpan.FromHours(-7));
            var writeViewModel = new AuthorWriteViewModel
            {
                Name = "Name2",
                Biography = "bio2",
                BirthDate = birthDate2,
                DeathDate = deathDate2
            }; 
            authorService.Create(Arg.Is<AuthorWriteModel>(model =>
                    model.Name == "Name2"
                    && model.Biography == "bio2"
                    && model.BirthDate == birthDate2
                    && model.DeathDate == deathDate2))
                .Returns(new AuthorReadViewModel { Id = 2, Name = "Name2", Biography = "bio2", BirthDate = birthDate2, DeathDate = deathDate2 });
            var controller = new AuthorsController(authorService);

            var result = controller.CreateAuthor(writeViewModel);

            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;
            var model = (AuthorReadViewModel)okObjectResult.Value;
            Assert.IsAssignableFrom<AuthorReadViewModel>(model);
            Assert.Equal(2, model.Id);
            Assert.Equal("Name2", model.Name);
            Assert.Equal("bio2", model.Biography);
            Assert.Equal(birthDate2, model.BirthDate);
            Assert.Equal(deathDate2, model.DeathDate);
        }

        [Fact]
        public void CreateAuthor_CallsService()
        {
            var birthDate = new DateTimeOffset(1900, 3, 4, 7, 0, 0, TimeSpan.FromHours(-7));
            var deathDate = new DateTimeOffset(1980, 3, 4, 7, 0, 0, TimeSpan.FromHours(-7));
            var authorService = Substitute.For<IAuthorService>();
            var controller = new AuthorsController(authorService);
            var writeViewModel = new AuthorWriteViewModel
            {
                Name = "Test",
                Biography = "bio",
                BirthDate = birthDate,
                DeathDate = deathDate
            };

            controller.CreateAuthor(writeViewModel);

            authorService.Received(1).Create(Arg.Is<AuthorWriteModel>(model =>
                model.Name == "Test"
                && model.Biography == "bio"
                && model.BirthDate == birthDate
                && model.DeathDate == deathDate));
        }

        [Fact]
        public void GetAllAuthors_ReturnsValidViewModels()
        {
            var authorService = Substitute.For<IAuthorService>();
            var birthDate1 = new DateTimeOffset(1819, 11, 22, 7, 0, 0, TimeSpan.FromHours(-7));
            var deathDate1 = new DateTimeOffset(1880, 12, 22, 7, 0, 0, TimeSpan.FromHours(-7));
            var birthDate2 = new DateTimeOffset(819, 1, 2, 7, 0, 0, TimeSpan.FromHours(-7));
            var deathDate2 = new DateTimeOffset(880, 2, 2, 7, 0, 0, TimeSpan.FromHours(-7));
            authorService.GetAll().Returns(new List<Author>
                {
                    new() { Id = 1, Name = "Name1", Biography = "bio1", BirthDate = birthDate1, DeathDate = deathDate1},
                    new() { Id = 2, Name = "Name2", Biography = "bio2", BirthDate = birthDate2, DeathDate = deathDate2 },
                    new() { Id = 3, Name = "Name3", Biography = "bio3", BirthDate = birthDate1, DeathDate = deathDate1 }
                });
            var controller = new AuthorsController(authorService);

            var result = controller.GetAllAuthors();

            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;
            var model = okObjectResult.Value;
            Assert.IsAssignableFrom<IList<AuthorReadViewModel>>(model);
            var viewModelCollection = ((IList<AuthorReadViewModel>)model);
            Assert.Equal(3, viewModelCollection.Count);
            Assert.Equal(1, viewModelCollection[0].Id);
            Assert.Equal("Name1", viewModelCollection[0].Name);
            Assert.Equal("bio1", viewModelCollection[0].Biography);
            Assert.Equal(birthDate1, viewModelCollection[0].BirthDate);
            Assert.Equal(deathDate1, viewModelCollection[0].DeathDate);
            Assert.Equal(2, viewModelCollection[1].Id);
            Assert.Equal("Name2", viewModelCollection[1].Name);
            Assert.Equal("bio2", viewModelCollection[1].Biography);
            Assert.Equal(birthDate2, viewModelCollection[1].BirthDate);
            Assert.Equal(deathDate2, viewModelCollection[1].DeathDate);
            Assert.Equal(3, viewModelCollection[2].Id);
            Assert.Equal("Name3", viewModelCollection[2].Name);
            Assert.Equal("bio3", viewModelCollection[2].Biography);
            Assert.Equal(birthDate1, viewModelCollection[2].BirthDate);
            Assert.Equal(deathDate1, viewModelCollection[2].DeathDate);
        }

        [Fact]
        public void GetAllAuthors_CallsService()
        {
            var authorService = Substitute.For<IAuthorService>();
            var controller = new AuthorsController(authorService);

            controller.GetAllAuthors();

            authorService.Received(1).GetAll();
        }

        [Fact]
        public void GetAuthorById_validId_ReturnsValidViewModel()
        {
            var authorService = Substitute.For<IAuthorService>();
            var birthDate2 = new DateTimeOffset(819, 1, 2, 7, 0, 0, TimeSpan.FromHours(-7));
            var deathDate2 = new DateTimeOffset(880, 2, 2, 7, 0, 0, TimeSpan.FromHours(-7));
            authorService.Get(2).Returns(new Author { Id = 2, Name = "Name2", Biography = "bio2", BirthDate = birthDate2, DeathDate = deathDate2 });
            var controller = new AuthorsController(authorService);

            var result = controller.GetAuthorById(2);

            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;
            var model = (AuthorReadViewModel)okObjectResult.Value;
            Assert.IsAssignableFrom<AuthorReadViewModel>(model);
            Assert.Equal(2, model.Id);
            Assert.Equal("Name2", model.Name);
            Assert.Equal("bio2", model.Biography);
            Assert.Equal(birthDate2, model.BirthDate);
            Assert.Equal(deathDate2, model.DeathDate);
        }

        [Fact]
        public void GetAuthorById_validId_CallsService()
        {
            var authorService = Substitute.For<IAuthorService>();
            var birthDate2 = new DateTimeOffset(819, 1, 2, 7, 0, 0, TimeSpan.FromHours(-7));
            var deathDate2 = new DateTimeOffset(880, 2, 2, 7, 0, 0, TimeSpan.FromHours(-7));
            authorService.Get(2).Returns(new Author { Id = 2, Name = "Name2", Biography = "bio2", BirthDate = birthDate2, DeathDate = deathDate2 });
            var controller = new AuthorsController(authorService);

            controller.GetAuthorById(2);

            authorService.Received(1).Get(2);
        }

        [Fact]
        public void GetAuthorById_invalidId_ReturnsNotFound()
        {
            var authorService = Substitute.For<IAuthorService>();
            authorService.Get(2).Returns(_ => null);
            var controller = new AuthorsController(authorService);

            var result = controller.GetAuthorById(2);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void UpdateAuthor_validId_CallsService()
        {
            var authorService = Substitute.For<IAuthorService>();
            authorService.Exists(2).Returns(true);
            var controller = new AuthorsController(authorService);
            var birthDate2 = new DateTimeOffset(819, 1, 2, 7, 0, 0, TimeSpan.FromHours(-7));
            var deathDate2 = new DateTimeOffset(880, 2, 2, 7, 0, 0, TimeSpan.FromHours(-7));

            controller.UpdateAuthor(2, new AuthorWriteViewModel { Name = "Name2", Biography = "bio2", BirthDate = birthDate2, DeathDate = deathDate2 });

            authorService
                .Received(1)
                .Update(2, Arg.Is<AuthorWriteModel>(model =>
                    model.Name == "Name2"
                    && model.Biography == "bio2"
                    && model.BirthDate ==birthDate2
                    && model.DeathDate == deathDate2));
        }

        [Fact]
        public void UpdateAuthor_validId_ReturnsNoContent()
        {
            var authorService = Substitute.For<IAuthorService>();
            authorService.Exists(5).Returns(true);
            var controller = new AuthorsController(authorService);

            var result = controller.UpdateAuthor(5, new AuthorWriteViewModel() { Name = "Name2", Biography = "bio2", BirthDate = null, DeathDate = null });

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void UpdateAuthor_invalidId_ReturnsNotFound()
        {
            var authorService = Substitute.For<IAuthorService>();
            authorService.Exists(2).Returns(false);
            var controller = new AuthorsController(authorService);

            var result = controller.UpdateAuthor(2, new AuthorWriteViewModel() { Name = "Name2", Biography = "bio2", BirthDate = null, DeathDate = null });

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void UpdateAuthor_invalidId_DoesNotCallUpdateService()
        {
            var authorService = Substitute.For<IAuthorService>();
            authorService.Exists(2).Returns(false);
            var controller = new AuthorsController(authorService);

            controller.UpdateAuthor(2, new AuthorWriteViewModel() { Name = "Name2", Biography = "bio2", BirthDate = null, DeathDate = null });

            authorService.DidNotReceive().Update(Arg.Any<int>(), Arg.Any<AuthorWriteModel>());
        }
    }
}
