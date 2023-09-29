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
                Format = "format",
                Language = "language",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new(){AuthorId = 1, AuthorRole = AuthorRole.Translator},
                    new(){AuthorId = 2, AuthorRole = AuthorRole.Narrator}
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
                Format = "format",
                Language = "language",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new(){AuthorId = 1, AuthorRole = AuthorRole.Translator},
                    new(){AuthorId = 2, AuthorRole = AuthorRole.Narrator}
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
                Format = "format",
                Language = "language",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new(){AuthorId = 1, AuthorRole = AuthorRole.Translator},
                    new(){AuthorId = 2, AuthorRole = AuthorRole.Narrator}
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
                Format = "format",
                Language = "language",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new(){AuthorId = 1, AuthorRole = AuthorRole.Translator},
                    new(){AuthorId = 2, AuthorRole = AuthorRole.Narrator}
                }
            };

            controller.CreateBook(writeViewModel);

            bookService.Received(1).Create(Arg.Any<BookWriteModel>());
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
                Format = "format",
                Language = "language",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new(){AuthorId = 1, AuthorRole = AuthorRole.Translator},
                    new(){AuthorId = 2, AuthorRole = AuthorRole.Narrator}
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
                Format = "format",
                Language = "language",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new(){AuthorId = 1, AuthorRole = AuthorRole.Translator},
                    new(){AuthorId = 2, AuthorRole = AuthorRole.Narrator}
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

            var result = controller.AddAuthor(1, new BookAuthorWriteViewModel { AuthorId = 1, AuthorRole = AuthorRole.Illustrator });

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

            var result = controller.AddAuthor(1, new BookAuthorWriteViewModel { AuthorId = 1, AuthorRole = AuthorRole.Illustrator });

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

            var result = controller.AddAuthor(1, new BookAuthorWriteViewModel { AuthorId = 1, AuthorRole = AuthorRole.Illustrator });

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

            var result = controller.AddAuthor(1, new BookAuthorWriteViewModel { AuthorId = 1, AuthorRole = AuthorRole.Illustrator });

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
            bookService.GetAll().Returns( new List<Book> 
                {
                    new()
                    {
                        Id = 1,
                        Title = "Title1",
                        Format = "format",
                        Language = "language",
                        WorkId = 3,
                        BookAuthors = new List<BookAuthor>
                        {
                            new(){AuthorId = 1, AuthorRole = AuthorRole.Translator},
                            new(){AuthorId = 2, AuthorRole = AuthorRole.Narrator}
                        }
                    },
                    new()
                    {
                        Id = 2,
                        Title = "Title2",
                        Format = "format",
                        Language = "language",
                        WorkId = 3,
                        BookAuthors = new List<BookAuthor>
                        {
                            new(){AuthorId = 3, AuthorRole = AuthorRole.Editor},
                            new(){AuthorId = 4, AuthorRole = AuthorRole.Contributor}
                        }
                    },
                    new()
                    {
                        Id = 3,
                        Title = "Title3",
                        Format = "format",
                        Language = "language",
                        WorkId = 3,
                        BookAuthors = new List<BookAuthor>
                        {
                            new(){AuthorId = 5, AuthorRole = AuthorRole.Illustrator}
                        }
                    }
                });
            var controller = new BooksController(bookService);

            var result = controller.GetAllBooks();

            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;
            var model = okObjectResult.Value;
            Assert.IsAssignableFrom<IList<BookReadViewModel>>(model);
            var bookList = (IList<BookReadViewModel>)model;
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
            Assert.Equal(2, book1.BookAuthors.Count);
            Assert.Equal(2, book2.BookAuthors.Count);
            Assert.Equal(1, book3.BookAuthors.Count);
            Assert.Contains(book1.BookAuthors, ba => ba.AuthorId == 1 && ba.AuthorRole == AuthorRole.Translator);
            Assert.Contains(book1.BookAuthors, ba => ba.AuthorId == 2 && ba.AuthorRole == AuthorRole.Narrator);
            Assert.Contains(book2.BookAuthors, ba => ba.AuthorId == 3 && ba.AuthorRole == AuthorRole.Editor); 
            Assert.Contains(book2.BookAuthors, ba => ba.AuthorId == 4 && ba.AuthorRole == AuthorRole.Contributor);
            Assert.Contains(book3.BookAuthors, ba => ba.AuthorId == 5 && ba.AuthorRole == AuthorRole.Illustrator);
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
            bookService.Exists(5).Returns(true);
            bookService.Get(5).Returns( new Book
            {
                Id = 5,
                Title = "BookTitle",
                Format = "format",
                Language = "language",
                WorkId = 3,
                BookAuthors = new List<BookAuthor>
                {
                    new(){AuthorId = 1, AuthorRole = AuthorRole.Translator},
                    new(){AuthorId = 2, AuthorRole = AuthorRole.Narrator}
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
            Assert.Equal("format", model.Format);
            Assert.Equal("language", model.Language);
            Assert.Equal(3, model.WorkId);
            Assert.Contains(model.BookAuthors, ba => ba.AuthorId == 1 && ba.AuthorRole == AuthorRole.Translator);
            Assert.Contains(model.BookAuthors, ba => ba.AuthorId == 2 && ba.AuthorRole == AuthorRole.Narrator);
        }

        [Fact]
        public void GetBookById_validId_CallsService()
        {
            var bookService = Substitute.For<IBookService>();
            bookService.Exists(5).Returns(true);
            bookService.Get(5).Returns(new Book
            {
                Id = 5,
                Title = "BookTitle",
                Format = "format",
                Language = "language",
                WorkId = 3,
                BookAuthors = new List<BookAuthor>
                {
                    new(){AuthorId = 1, AuthorRole = AuthorRole.Translator},
                    new(){AuthorId = 2, AuthorRole = AuthorRole.Narrator}
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
                Format = "format",
                Language = "language",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new(){AuthorId = 1, AuthorRole = AuthorRole.Translator},
                    new(){AuthorId = 2, AuthorRole = AuthorRole.Narrator}
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
                Format = "format",
                Language = "language",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new(){AuthorId = 1, AuthorRole = AuthorRole.Translator},
                    new(){AuthorId = 2, AuthorRole = AuthorRole.Narrator}
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
                Format = "format",
                Language = "language",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new(){AuthorId = 1, AuthorRole = AuthorRole.Translator},
                    new(){AuthorId = 2, AuthorRole = AuthorRole.Narrator}
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
                Format = "format",
                Language = "language",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new(){AuthorId = 1, AuthorRole = AuthorRole.Translator},
                    new(){AuthorId = 2, AuthorRole = AuthorRole.Narrator}
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
                Format = "format",
                Language = "language",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new(){AuthorId = 1, AuthorRole = AuthorRole.Translator},
                    new(){AuthorId = 2, AuthorRole = AuthorRole.Narrator}
                }
            };

            controller.UpdateBook(5, writeViewModel);

            bookService.Received(1).Update(5, Arg.Any<BookWriteModel>());
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
                Format = "format",
                Language = "language",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new(){AuthorId = 1, AuthorRole = AuthorRole.Translator},
                    new(){AuthorId = 2, AuthorRole = AuthorRole.Narrator}
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
                Format = "format",
                Language = "language",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new(){AuthorId = 1, AuthorRole = AuthorRole.Translator},
                    new(){AuthorId = 2, AuthorRole = AuthorRole.Narrator}
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
                Format = "format",
                Language = "language",
                WorkId = 3,
                BookAuthors = new List<BookAuthorWriteViewModel>
                {
                    new(){AuthorId = 1, AuthorRole = AuthorRole.Translator},
                    new(){AuthorId = 2, AuthorRole = AuthorRole.Narrator}
                }
            };

            controller.UpdateBook(5, writeViewModel);

            bookService.DidNotReceive().Update(Arg.Any<int>(), Arg.Any<BookWriteModel>());
        }
    }
}
