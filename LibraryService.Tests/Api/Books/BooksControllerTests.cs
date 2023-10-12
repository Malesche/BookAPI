using LibraryService.Api.Books;
using LibraryService.Api.Books.Models;
using LibraryService.Api.Books.ViewModels;
using LibraryService.Persistence;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace LibraryService.Tests.Api.Books
{
    public class BooksControllerTests
    {
        [Fact]
        public void CreateBook_IdsAllValid_ReturnsNoContent()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.WorkExists(3).Returns(true);
            bookService.AuthorExists(1).Returns(true);
            bookService.AuthorExists(2).Returns(true);
            var controller = new BooksController(bookService);
            var writeViewModel = new BookWriteViewModel
            {
                Title = "BookTitle",
                Format = BookFormat.Paperback,
                Language = "language",
                Isbn = "3902866063",
                Isbn13 = "9783902866066",
                Description = "describing_describing",
                PubDate = new DateTimeOffset(2013, 3, 4, 7, 0, 0, TimeSpan.FromHours(-7)),
                CoverUrl = "someUrl",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new() { AuthorId = 1, AuthorRole = AuthorRole.Translator },
                    new() { AuthorId = 2, AuthorRole = AuthorRole.Narrator }
                }
            };

            var result = controller.CreateBook(writeViewModel);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void CreateBook_invalidAuthorId_ReturnsNotFound()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.WorkExists(3).Returns(false);
            bookService.AuthorExists(1).Returns(true);
            bookService.AuthorExists(2).Returns(true);
            var controller = new BooksController(bookService);
            var writeViewModel = new BookWriteViewModel
            {
                Title = "BookTitle",
                Format = BookFormat.Paperback,
                Language = "language",
                Isbn = "3902866063",
                Isbn13 = "9783902866066",
                Description = "describing_describing",
                PubDate = new DateTimeOffset(2013, 3, 4, 7, 0, 0, TimeSpan.FromHours(-7)),
                CoverUrl = "someUrl",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new() { AuthorId = 1, AuthorRole = AuthorRole.Translator },
                    new() { AuthorId = 2, AuthorRole = AuthorRole.Narrator }
                }
            };

            var result = controller.CreateBook(writeViewModel);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CreateBook_invalidWorkId_ReturnsNotFound()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.WorkExists(3).Returns(true);
            bookService.AuthorExists(1).Returns(true);
            bookService.AuthorExists(2).Returns(false);
            var controller = new BooksController(bookService);
            var writeViewModel = new BookWriteViewModel
            {
                Title = "BookTitle",
                Format = BookFormat.Paperback,
                Language = "language",
                Isbn = "3902866063",
                Isbn13 = "9783902866066",
                Description = "describing_describing",
                PubDate = new DateTimeOffset(2013, 3, 4, 7, 0, 0, TimeSpan.FromHours(-7)),
                CoverUrl = "someUrl",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new() { AuthorId = 1, AuthorRole = AuthorRole.Translator },
                    new() { AuthorId = 2, AuthorRole = AuthorRole.Narrator }
                }
            };

            var result = controller.CreateBook(writeViewModel);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CreateBook_IdsAllValid_CallsServiceCreate()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.WorkExists(3).Returns(true);
            bookService.AuthorExists(1).Returns(true);
            bookService.AuthorExists(2).Returns(true);
            var controller = new BooksController(bookService);
            var writeViewModel = new BookWriteViewModel
            {
                Title = "BookTitle",
                Format = BookFormat.Paperback,
                Language = "language",
                Isbn = "3902866063",
                Isbn13 = "9783902866066",
                Description = "describing_describing",
                PubDate = new DateTimeOffset(2013, 3, 4, 7, 0, 0, TimeSpan.FromHours(-7)),
                CoverUrl = "someUrl",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new() { AuthorId = 1, AuthorRole = AuthorRole.Translator },
                    new() { AuthorId = 2, AuthorRole = AuthorRole.Narrator }
                }
            };

            controller.CreateBook(writeViewModel);

            bookService
                .Received(1)
                .Create(
                    Arg.Is<BookWriteModel>(model =>
                        model.Title == "BookTitle"
                        && model.Format == BookFormat.Paperback
                        && model.Language == "language"
                        && model.Isbn == "3902866063"
                        && model.Isbn13 == "9783902866066"
                        && model.Description == "describing_describing"
                        && model.PubDate == new DateTimeOffset(2013, 3, 4, 7, 0, 0, TimeSpan.FromHours(-7))
                        && model.CoverUrl == "someUrl"
                        && model.WorkId == 3
                        && model.BookAuthors[0].AuthorId == 1
                        && model.BookAuthors[1].AuthorId == 2
                        && model.BookAuthors[0].AuthorRole == AuthorRole.Translator
                        && model.BookAuthors[1].AuthorRole == AuthorRole.Narrator));
        }

        [Fact]
        public void CreateBook_invalidAuthorId_DoesNotCallServiceCreate()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.WorkExists(3).Returns(false);
            bookService.AuthorExists(1).Returns(true);
            bookService.AuthorExists(2).Returns(true);
            var controller = new BooksController(bookService);
            var writeViewModel = new BookWriteViewModel
            {
                Title = "BookTitle",
                Format = BookFormat.Paperback,
                Language = "language",
                Isbn = "3902866063",
                Isbn13 = "9783902866066",
                Description = "describing_describing",
                PubDate = new DateTimeOffset(2013, 3, 4, 7, 0, 0, TimeSpan.FromHours(-7)),
                CoverUrl = "someUrl",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new() { AuthorId = 1, AuthorRole = AuthorRole.Translator },
                    new() { AuthorId = 2, AuthorRole = AuthorRole.Narrator }
                }
            };

            controller.CreateBook(writeViewModel);

            bookService.DidNotReceive().Create(Arg.Any<BookWriteModel>());
        }

        [Fact]
        public void CreateBook_invalidWorkId_DoesNotCallServiceCreate()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.WorkExists(3).Returns(true);
            bookService.AuthorExists(1).Returns(true);
            bookService.AuthorExists(2).Returns(false);
            var controller = new BooksController(bookService);
            var writeViewModel = new BookWriteViewModel
            {
                Title = "BookTitle",
                Format = BookFormat.Paperback,
                Language = "language",
                Isbn = "3902866063",
                Isbn13 = "9783902866066",
                Description = "describing_describing",
                PubDate = new DateTimeOffset(2013, 3, 4, 7, 0, 0, TimeSpan.FromHours(-7)),
                CoverUrl = "someUrl",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new() { AuthorId = 1, AuthorRole = AuthorRole.Translator },
                    new() { AuthorId = 2, AuthorRole = AuthorRole.Narrator }
                }
            };

            controller.CreateBook(writeViewModel);

            bookService.DidNotReceive().Create(Arg.Any<BookWriteModel>());
        }

        [Fact]
        public void AddAuthor_IdsAndRoleAllValid_ReturnsNoContent()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.Exists(1).Returns(true);
            bookService.AuthorExists(1).Returns(true);
            bookService.BookAuthorRoleExists(1, 1, AuthorRole.Illustrator).Returns(false);
            var controller = new BooksController(bookService);

            var result = controller.AddAuthor(1,
                new BookAuthorWriteViewModel { AuthorId = 1, AuthorRole = AuthorRole.Illustrator });

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void AddAuthor_BookIdInvalid_ReturnsNotFoundObjectResult()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.Exists(1).Returns(false);
            bookService.AuthorExists(1).Returns(true);
            bookService.BookAuthorRoleExists(1, 1, AuthorRole.Illustrator).Returns(false);
            var controller = new BooksController(bookService);

            var result = controller.AddAuthor(1,
                new BookAuthorWriteViewModel { AuthorId = 1, AuthorRole = AuthorRole.Illustrator });

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void AddAuthor_AuthorIdInvalid_ReturnsNotFoundObjectResult()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.Exists(1).Returns(true);
            bookService.AuthorExists(1).Returns(false);
            bookService.BookAuthorRoleExists(1, 1, AuthorRole.Illustrator).Returns(false);
            var controller = new BooksController(bookService);

            var result = controller.AddAuthor(1,
                new BookAuthorWriteViewModel { AuthorId = 1, AuthorRole = AuthorRole.Illustrator });

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void AddAuthor_BookAuthorRoleExistsAlready_ReturnsConflictObjectResult()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.Exists(1).Returns(true);
            bookService.AuthorExists(1).Returns(true);
            bookService.BookAuthorRoleExists(1, 1, AuthorRole.Illustrator).Returns(true);
            var controller = new BooksController(bookService);

            var result = controller.AddAuthor(1,
                new BookAuthorWriteViewModel { AuthorId = 1, AuthorRole = AuthorRole.Illustrator });

            Assert.IsType<ConflictObjectResult>(result);
        }

        [Fact]
        public void AddAuthor_IdsAndRoleAllValid_CallsService()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.Exists(1).Returns(true);
            bookService.AuthorExists(1).Returns(true);
            bookService.BookAuthorRoleExists(1, 1, AuthorRole.Illustrator).Returns(false);
            var controller = new BooksController(bookService);

            controller.AddAuthor(1, new BookAuthorWriteViewModel { AuthorId = 1, AuthorRole = AuthorRole.Illustrator });

            bookService.Received(1).AddBookAuthor(1, 1, AuthorRole.Illustrator);
        }

        [Fact]
        public void AddAuthor_BookIdInvalid_DoesNotCallServiceAddBookAuthor()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.Exists(1).Returns(false);
            bookService.AuthorExists(1).Returns(true);
            bookService.BookAuthorRoleExists(1, 1, AuthorRole.Illustrator).Returns(false);
            var controller = new BooksController(bookService);

            controller.AddAuthor(1, new BookAuthorWriteViewModel { AuthorId = 1, AuthorRole = AuthorRole.Illustrator });

            bookService.DidNotReceive().AddBookAuthor(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<AuthorRole>());
        }

        [Fact]
        public void AddAuthor_AuthorIdInvalid_DoesNotCallServiceAddBookAuthor()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.Exists(1).Returns(true);
            bookService.AuthorExists(1).Returns(false);
            bookService.BookAuthorRoleExists(1, 1, AuthorRole.Illustrator).Returns(false);
            var controller = new BooksController(bookService);

            controller.AddAuthor(1, new BookAuthorWriteViewModel { AuthorId = 1, AuthorRole = AuthorRole.Illustrator });

            bookService.DidNotReceive().AddBookAuthor(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<AuthorRole>());
        }

        [Fact]
        public void AddAuthor_BookAuthorRoleExistsAlready_DoesNotCallServiceAddBookAuthor()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.Exists(1).Returns(true);
            bookService.AuthorExists(1).Returns(true);
            bookService.BookAuthorRoleExists(1, 1, AuthorRole.Illustrator).Returns(true);
            var controller = new BooksController(bookService);

            controller.AddAuthor(1, new BookAuthorWriteViewModel { AuthorId = 1, AuthorRole = AuthorRole.Illustrator });

            bookService.DidNotReceive().AddBookAuthor(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<AuthorRole>());
        }

        [Fact]
        public void RemoveAuthor_RoleNotNullAndBookAuthorRoleExist_ReturnsNoContent()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.BookAuthorRoleExists(1, 1, AuthorRole.Author).Returns(true);
            var controller = new BooksController(bookService);

            var result = controller.RemoveAuthor(1, 1, AuthorRole.Author);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void RemoveAuthor_RoleNullAndBookAuthorExists_ReturnsNoContent()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.BookAuthorExists(1, 1).Returns(true);
            var controller = new BooksController(bookService);

            var result = controller.RemoveAuthor(1, 1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void RemoveAuthor_RoleNullAndBookAuthorInvalid_ReturnsNotFound()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.BookAuthorExists(1, 1).Returns(false);
            var controller = new BooksController(bookService);

            var result = controller.RemoveAuthor(1, 1);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void RemoveAuthor_RoleNotNullAndBookAuthorRoleInvalid_ReturnsNotFound()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.BookAuthorRoleExists(1, 1, AuthorRole.Author).Returns(false);
            var controller = new BooksController(bookService);

            var result = controller.RemoveAuthor(1, 1, AuthorRole.Author);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void RemoveAuthor_RoleNotNullAndBookAuthorRoleExist_CallsServiceRemoveBookAuthorInRole()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.BookAuthorRoleExists(1, 1, AuthorRole.Author).Returns(true);
            var controller = new BooksController(bookService);

            controller.RemoveAuthor(1, 1, AuthorRole.Author);

            bookService.Received(1).RemoveBookAuthorInRole(1, 1, AuthorRole.Author);
        }

        [Fact]
        public void RemoveAuthor_RoleNullAndBookAuthorExists_CallsServiceRemoveBookAuthor()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.BookAuthorExists(1, 1).Returns(true);
            var controller = new BooksController(bookService);

            controller.RemoveAuthor(1, 1);

            bookService.Received(1).RemoveBookAuthor(1, 1);
        }

        [Fact]
        public void RemoveAuthor_RoleNullAndBookAuthorInvalid_DoesNotCallServiceRemoveBookAuthorOrInRole()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.BookAuthorExists(1, 1).Returns(false);
            var controller = new BooksController(bookService);

            controller.RemoveAuthor(1, 1);

            bookService.DidNotReceive().RemoveBookAuthorInRole(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<AuthorRole>());
            bookService.DidNotReceive().RemoveBookAuthor(Arg.Any<int>(), Arg.Any<int>());
        }

        [Fact]
        public void RemoveAuthor_RoleNotNullAndBookAuthorRoleInvalid_DoesNotCallServiceRemoveBookAuthorOrInRole()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.BookAuthorRoleExists(1, 1, AuthorRole.Author).Returns(false);
            var controller = new BooksController(bookService);

            controller.RemoveAuthor(1, 1, AuthorRole.Author);

            bookService.DidNotReceive().RemoveBookAuthorInRole(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<AuthorRole>());
            bookService.DidNotReceive().RemoveBookAuthor(Arg.Any<int>(), Arg.Any<int>());
        }

        [Fact]
        public void GetAllBooks_ReturnsValidViewModels()
        {
            var bookService = Substitute.For<IBookService>();
            var someDate = new DateTimeOffset(2002, 2, 2, 7, 0, 0, TimeSpan.FromHours(-7));
            bookService.GetAll().Returns(new List<Book>
            {
                new()
                {
                    Id = 1,
                    Title = "Title1",
                    Format = BookFormat.Paperback,
                    Language = "language1",
                    WorkId = 1,
                    Isbn = "1",
                    Isbn13 = "111",
                    Description = "describing_describing1",
                    PubDate = new DateTimeOffset(2001, 1, 1, 7, 0, 0, TimeSpan.FromHours(-7)),
                    CoverUrl = "someUrl1",
                    BookAuthors = new List<BookAuthor>
                    {
                        new() { AuthorId = 1, AuthorRole = AuthorRole.Translator },
                        new() { AuthorId = 2, AuthorRole = AuthorRole.Narrator }
                    },
                    Authors = new List<Author>()
                    {
                        new() { Id = 1, Name = "Name1", Biography = "bio1", BirthDate = someDate, DeathDate = someDate },
                        new() { Id = 2, Name = "Name2", Biography = "bio2", BirthDate = someDate, DeathDate = someDate }
                    }
                },
                new()
                {
                    Id = 2,
                    Title = "Title2",
                    Format = BookFormat.Hardcover,
                    Language = "language2",
                    Isbn = "2",
                    Isbn13 = "222",
                    Description = "describing_describing2",
                    PubDate = new DateTimeOffset(2002, 2, 2, 7, 0, 0, TimeSpan.FromHours(-7)),
                    CoverUrl = "someUrl2",
                    WorkId = 2,
                    BookAuthors = new List<BookAuthor>
                    {
                        new() { AuthorId = 3, AuthorRole = AuthorRole.Editor },
                        new() { AuthorId = 4, AuthorRole = AuthorRole.Contributor }
                    },
                    Authors = new List<Author>()
                    {
                        new() { Id = 3, Name = "Name3", Biography = "bio3", BirthDate = someDate, DeathDate = someDate },
                        new() { Id = 4, Name = "Name4", Biography = "bio4", BirthDate = someDate, DeathDate = someDate }
                    }
                },
                new()
                {
                    Id = 3,
                    Title = "Title3",
                    Format = BookFormat.Audiobook,
                    Language = "language3",
                    Isbn = "3",
                    Isbn13 = "333",
                    Description = "describing_describing3",
                    PubDate = new DateTimeOffset(2003, 3, 3, 7, 0, 0, TimeSpan.FromHours(-7)),
                    CoverUrl = "someUrl3",
                    WorkId = 3,
                    BookAuthors = new List<BookAuthor>
                    {
                        new() { AuthorId = 5, AuthorRole = AuthorRole.Illustrator }
                    },
                    Authors = new List<Author>
                    {
                        new() { Id = 5, Name = "Name5", Biography = "bio5", BirthDate = someDate, DeathDate = someDate }
                    }
                }
            });
            var controller = new BooksController(bookService);

            var result = controller.GetAllBooks();

            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;
            var value = okObjectResult.Value;
            Assert.IsAssignableFrom<IList<BookReadViewModel>>(value);
            var bookList = (IList<BookReadViewModel>)value;

            Assert.Contains(bookList, model =>
                model.Title == "Title1"
                && model.Format == BookFormat.Paperback
                && model.Language == "language1"
                && model.Isbn == "1"
                && model.Isbn13 == "111"
                && model.Description == "describing_describing1"
                && model.PubDate == new DateTimeOffset(2001, 1, 1, 7, 0, 0, TimeSpan.FromHours(-7))
                && model.CoverUrl == "someUrl1"
                && model.WorkId == 1);
            Assert.Contains(bookList, model =>
                model.Title == "Title2"
                && model.Format == BookFormat.Hardcover
                && model.Language == "language2"
                && model.Isbn == "2"
                && model.Isbn13 == "222"
                && model.Description == "describing_describing2"
                && model.PubDate == new DateTimeOffset(2002, 2, 2, 7, 0, 0, TimeSpan.FromHours(-7))
                && model.CoverUrl == "someUrl2"
                && model.WorkId == 2);
            Assert.Contains(bookList, model =>
                model.Title == "Title3"
                && model.Format == BookFormat.Audiobook
                && model.Language == "language3"
                && model.Isbn == "3"
                && model.Isbn13 == "333"
                && model.Description == "describing_describing3"
                && model.PubDate == new DateTimeOffset(2003, 3, 3, 7, 0, 0, TimeSpan.FromHours(-7))
                && model.CoverUrl == "someUrl3"
                && model.WorkId == 3);
            var book1 = bookList[0];
            var book2 = bookList[1];
            var book3 = bookList[2];
            Assert.Equal(3, bookList.Count);
            Assert.Equal(1, book1.Id);
            Assert.Equal("Title1", book1.Title);
            Assert.Equal(2, book2.Id);
            Assert.Equal("Title2", book2.Title);
            Assert.Equal(3, book3.Id);
            Assert.Equal("Title3", book3.Title);
            Assert.Equal(2, book1.BookAuthorInfos.Count);
            Assert.Equal(2, book2.BookAuthorInfos.Count);
            Assert.Equal(1, book3.BookAuthorInfos.Count);
            Assert.Contains(book1.BookAuthorInfos, ba => ba.AuthorId == 1 && ba.AuthorRole == AuthorRole.Translator);
            Assert.Contains(book1.BookAuthorInfos, ba => ba.AuthorId == 2 && ba.AuthorRole == AuthorRole.Narrator);
            Assert.Contains(book2.BookAuthorInfos, ba => ba.AuthorId == 3 && ba.AuthorRole == AuthorRole.Editor);
            Assert.Contains(book2.BookAuthorInfos, ba => ba.AuthorId == 4 && ba.AuthorRole == AuthorRole.Contributor);
            Assert.Contains(book3.BookAuthorInfos, ba => ba.AuthorId == 5 && ba.AuthorRole == AuthorRole.Illustrator);
        }

        [Fact]
        public void GetAllBooks_CallsService()
        {
            var bookService = Substitute.For<IBookService>();
            var controller = new BooksController(bookService);

            controller.GetAllBooks();

            bookService.Received(1).GetAll();
        }

        [Fact]
        public void GetBookById_validId_ReturnsValidViewModel()
        {
            var bookService = Substitute.For<IBookService>();
            var someDate = new DateTimeOffset(2002, 2, 2, 7, 0, 0, TimeSpan.FromHours(-7));
            bookService.Exists(5).Returns(true);
            bookService.Get(5).Returns(new Book
            {
                Id = 5,
                Title = "BookTitle",
                Format = BookFormat.Paperback,
                Language = "language",
                Isbn = "3902866063",
                Isbn13 = "9783902866066",
                Description = "describing_describing",
                PubDate = new DateTimeOffset(2013, 3, 4, 7, 0, 0, TimeSpan.FromHours(-7)),
                CoverUrl = "someUrl",
                WorkId = 3,
                BookAuthors = new List<BookAuthor>
                {
                    new() { AuthorId = 1, AuthorRole = AuthorRole.Translator },
                    new() { AuthorId = 2, AuthorRole = AuthorRole.Narrator }
                },
                Authors = new List<Author>()
                {
                    new() { Id = 1, Name = "Name1", Biography = "bio1", BirthDate = someDate, DeathDate = someDate },
                    new() { Id = 2, Name = "Name2", Biography = "bio2", BirthDate = someDate, DeathDate = someDate }
                }
            });
            var controller = new BooksController(bookService);

            var result = controller.GetBookById(5);

            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;
            var model = (BookReadViewModel)okObjectResult.Value;
            Assert.IsAssignableFrom<BookReadViewModel>(model);
            Assert.Equal(5, model.Id);
            Assert.Equal("BookTitle", model.Title);
            Assert.Equal(BookFormat.Paperback, model.Format);
            Assert.Equal("language", model.Language);
            Assert.Equal("3902866063", model.Isbn);
            Assert.Equal("9783902866066", model.Isbn13);
            Assert.Equal("describing_describing", model.Description);
            Assert.Equal(new DateTimeOffset(2013, 3, 4, 7, 0, 0, TimeSpan.FromHours(-7)), model.PubDate);
            Assert.Equal("someUrl", model.CoverUrl);
            Assert.Equal(3, model.WorkId);
            Assert.Contains(model.BookAuthorInfos, ba => ba.AuthorId == 1 && ba.AuthorRole == AuthorRole.Translator);
            Assert.Contains(model.BookAuthorInfos, ba => ba.AuthorId == 2 && ba.AuthorRole == AuthorRole.Narrator);
        }

        [Fact]
        public void GetBookById_validId_CallsService()
        {
            var bookService = Substitute.For<IBookService>();
            var someDate = new DateTimeOffset(2002, 2, 2, 7, 0, 0, TimeSpan.FromHours(-7));
            bookService.Exists(5).Returns(true);
            bookService.Get(5).Returns(new Book
            {
                Id = 5,
                Title = "BookTitle",
                Format = BookFormat.Paperback,
                Language = "language",
                Isbn = "3902866063",
                Isbn13 = "9783902866066",
                Description = "describing_describing",
                PubDate = new DateTimeOffset(2013, 3, 4, 7, 0, 0, TimeSpan.FromHours(-7)),
                CoverUrl = "someUrl",
                WorkId = 3,
                BookAuthors = new List<BookAuthor>
                {
                    new() { AuthorId = 1, AuthorRole = AuthorRole.Translator },
                    new() { AuthorId = 2, AuthorRole = AuthorRole.Narrator }
                },
                Authors = new List<Author>()
                {
                    new() { Id = 1, Name = "Name1", Biography = "bio1", BirthDate = someDate, DeathDate = someDate },
                    new() { Id = 2, Name = "Name2", Biography = "bio2", BirthDate = someDate, DeathDate = someDate }
                }
            });
            var controller = new BooksController(bookService);

            controller.GetBookById(5);

            bookService.Received(1).Get(5);

        }

        [Fact]
        public void GetBookById_invalidId_ReturnsNotFound()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.Exists(5).Returns(false);
            var controller = new BooksController(bookService);

            var result = controller.GetBookById(5);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void GetBookById_invalidId_DoesNotCallServiceGet()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.Exists(5).Returns(false);
            var controller = new BooksController(bookService);

            controller.GetBookById(5);

            bookService.DidNotReceive().Get(5);
        }

        [Fact]
        public void UpdateBook_IdsAllValid_ReturnsNoContent()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.Exists(5).Returns(true);
            bookService.WorkExists(3).Returns(true);
            bookService.AuthorExists(1).Returns(true);
            bookService.AuthorExists(2).Returns(true);
            var controller = new BooksController(bookService);
            var writeViewModel = new BookWriteViewModel
            {
                Title = "BookTitle",
                Format = BookFormat.Paperback,
                Language = "language",
                Isbn = "3902866063",
                Isbn13 = "9783902866066",
                Description = "describing_describing",
                PubDate = new DateTimeOffset(2013, 3, 4, 7, 0, 0, TimeSpan.FromHours(-7)),
                CoverUrl = "someUrl",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new() { AuthorId = 1, AuthorRole = AuthorRole.Translator },
                    new() { AuthorId = 2, AuthorRole = AuthorRole.Narrator }
                }
            };

            var result = controller.UpdateBook(5, writeViewModel);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void UpdateBook_invalidBookId_ReturnsNotFound()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.Exists(5).Returns(false);
            bookService.WorkExists(3).Returns(true);
            bookService.AuthorExists(1).Returns(true);
            bookService.AuthorExists(2).Returns(true);
            var controller = new BooksController(bookService);
            var writeViewModel = new BookWriteViewModel
            {
                Title = "BookTitle",
                Format = BookFormat.Paperback,
                Language = "language",
                Isbn = "3902866063",
                Isbn13 = "9783902866066",
                Description = "describing_describing",
                PubDate = new DateTimeOffset(2013, 3, 4, 7, 0, 0, TimeSpan.FromHours(-7)),
                CoverUrl = "someUrl",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new() { AuthorId = 1, AuthorRole = AuthorRole.Translator },
                    new() { AuthorId = 2, AuthorRole = AuthorRole.Narrator }
                }
            };

            var result = controller.UpdateBook(5, writeViewModel);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void UpdateBook_invalidAuthorId_ReturnsNotFound()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.Exists(5).Returns(true);
            bookService.WorkExists(3).Returns(true);
            bookService.AuthorExists(1).Returns(true);
            bookService.AuthorExists(2).Returns(false);
            var controller = new BooksController(bookService);
            var writeViewModel = new BookWriteViewModel
            {
                Title = "BookTitle",
                Format = BookFormat.Paperback,
                Language = "language",
                Isbn = "3902866063",
                Isbn13 = "9783902866066",
                Description = "describing_describing",
                PubDate = new DateTimeOffset(2013, 3, 4, 7, 0, 0, TimeSpan.FromHours(-7)),
                CoverUrl = "someUrl",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new() { AuthorId = 1, AuthorRole = AuthorRole.Translator },
                    new() { AuthorId = 2, AuthorRole = AuthorRole.Narrator }
                }
            };

            var result = controller.UpdateBook(5, writeViewModel);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void UpdateBook_invalidWorkId_ReturnsNotFound()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.Exists(5).Returns(true);
            bookService.WorkExists(3).Returns(false);
            bookService.AuthorExists(1).Returns(true);
            bookService.AuthorExists(2).Returns(true);
            var controller = new BooksController(bookService);
            var writeViewModel = new BookWriteViewModel
            {
                Title = "BookTitle",
                Format = BookFormat.Paperback,
                Language = "language",
                Isbn = "3902866063",
                Isbn13 = "9783902866066",
                Description = "describing_describing",
                PubDate = new DateTimeOffset(2013, 3, 4, 7, 0, 0, TimeSpan.FromHours(-7)),
                CoverUrl = "someUrl",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new() { AuthorId = 1, AuthorRole = AuthorRole.Translator },
                    new() { AuthorId = 2, AuthorRole = AuthorRole.Narrator }
                }
            };

            var result = controller.UpdateBook(5, writeViewModel);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void UpdateBook_IdsAllValid_CallsServiceCreate()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.Exists(5).Returns(true);
            bookService.WorkExists(3).Returns(true);
            bookService.AuthorExists(1).Returns(true);
            bookService.AuthorExists(2).Returns(true);
            var controller = new BooksController(bookService);
            var writeViewModel = new BookWriteViewModel
            {
                Title = "BookTitle",
                Format = BookFormat.Paperback,
                Language = "language",
                Isbn = "3902866063",
                Isbn13 = "9783902866066",
                Description = "describing_describing",
                PubDate = new DateTimeOffset(2013, 3, 4, 7, 0, 0, TimeSpan.FromHours(-7)),
                CoverUrl = "someUrl",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new() { AuthorId = 1, AuthorRole = AuthorRole.Translator },
                    new() { AuthorId = 2, AuthorRole = AuthorRole.Narrator }
                }
            };

            controller.UpdateBook(5, writeViewModel);

            bookService.Received(1).Update(5, Arg.Is<BookWriteModel>(model =>
                model.Title == "BookTitle"
                && model.Format == BookFormat.Paperback
                && model.Language == "language"
                && model.Isbn == "3902866063"
                && model.Isbn13 == "9783902866066"
                && model.Description == "describing_describing"
                && model.PubDate == new DateTimeOffset(2013, 3, 4, 7, 0, 0, TimeSpan.FromHours(-7))
                && model.CoverUrl == "someUrl"
                && model.WorkId == 3
                && model.BookAuthors[0].AuthorId == 1
                && model.BookAuthors[1].AuthorId == 2
                && model.BookAuthors[0].AuthorRole == AuthorRole.Translator
                && model.BookAuthors[1].AuthorRole == AuthorRole.Narrator));
        }

        [Fact]
        public void UpdateBook_invalidBookId_DoesNotCallServiceCreate()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.Exists(5).Returns(true);
            bookService.WorkExists(3).Returns(false);
            bookService.AuthorExists(1).Returns(true);
            bookService.AuthorExists(2).Returns(true);
            var controller = new BooksController(bookService);
            var writeViewModel = new BookWriteViewModel
            {
                Title = "BookTitle",
                Format = BookFormat.Paperback,
                Language = "language",
                Isbn = "3902866063",
                Isbn13 = "9783902866066",
                Description = "describing_describing",
                PubDate = new DateTimeOffset(2013, 3, 4, 7, 0, 0, TimeSpan.FromHours(-7)),
                CoverUrl = "someUrl",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new() { AuthorId = 1, AuthorRole = AuthorRole.Translator },
                    new() { AuthorId = 2, AuthorRole = AuthorRole.Narrator }
                }
            };

            controller.UpdateBook(5, writeViewModel);

            bookService.DidNotReceive().Update(Arg.Any<int>(), Arg.Any<BookWriteModel>());
        }

        [Fact]
        public void UpdateBook_invalidAuthorId_DoesNotCallServiceCreate()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.Exists(5).Returns(true);
            bookService.WorkExists(3).Returns(false);
            bookService.AuthorExists(1).Returns(true);
            bookService.AuthorExists(2).Returns(true);
            var controller = new BooksController(bookService);
            var writeViewModel = new BookWriteViewModel
            {
                Title = "BookTitle",
                Format = BookFormat.Paperback,
                Language = "language",
                Isbn = "3902866063",
                Isbn13 = "9783902866066",
                Description = "describing_describing",
                PubDate = new DateTimeOffset(2013, 3, 4, 7, 0, 0, TimeSpan.FromHours(-7)),
                CoverUrl = "someUrl",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new() { AuthorId = 1, AuthorRole = AuthorRole.Translator },
                    new() { AuthorId = 2, AuthorRole = AuthorRole.Narrator }
                }
            };

            controller.UpdateBook(5, writeViewModel);

            bookService.DidNotReceive().Update(Arg.Any<int>(), Arg.Any<BookWriteModel>());
        }

        [Fact]
        public void UpdateBook_invalidWorkId_DoesNotCallServiceCreate()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.Exists(5).Returns(true);
            bookService.WorkExists(3).Returns(true);
            bookService.AuthorExists(1).Returns(true);
            bookService.AuthorExists(2).Returns(false);
            var controller = new BooksController(bookService);
            var writeViewModel = new BookWriteViewModel
            {
                Title = "BookTitle",
                Format = BookFormat.Paperback,
                Language = "language",
                Isbn = "3902866063",
                Isbn13 = "9783902866066",
                Description = "describing_describing",
                PubDate = new DateTimeOffset(2013, 3, 4, 7, 0, 0, TimeSpan.FromHours(-7)),
                CoverUrl = "someUrl",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new() { AuthorId = 1, AuthorRole = AuthorRole.Translator },
                    new() { AuthorId = 2, AuthorRole = AuthorRole.Narrator }
                }
            };

            controller.UpdateBook(5, writeViewModel);

            bookService.DidNotReceive().Update(Arg.Any<int>(), Arg.Any<BookWriteModel>());
        }

        [Fact]
        public void DeleteBook_invalidBookId_ReturnsNotFound()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.Exists(5).Returns(false);
            var controller = new BooksController(bookService);

            var result = controller.DeleteBook(5);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void DeleteBook_validBookId_ReturnsNoContent()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.Exists(5).Returns(true);
            var controller = new BooksController(bookService);

            var result = controller.DeleteBook(5);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteBook_validBookId_CallsServiceDelete()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.Exists(5).Returns(true);
            var controller = new BooksController(bookService);

            controller.DeleteBook(5);

            bookService.Received(1).Delete(5);
        }

        [Fact]
        public void DeleteBook_invalidBookId_DoesNotCallServiceDelete()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.Exists(5).Returns(false);
            var controller = new BooksController(bookService);

            controller.DeleteBook(5);

            bookService.DidNotReceive().Delete(Arg.Any<int>());
        }
    }
}
