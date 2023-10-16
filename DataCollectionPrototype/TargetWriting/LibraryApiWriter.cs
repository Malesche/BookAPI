using DataCollectionPrototype.Core;
using DataCollectionPrototype.Core.Model;
using System.Net.Http.Json;
using LibraryService.Api.Authors.ViewModels;
using LibraryService.Api.Books.ViewModels;
using LibraryService.Persistence;

namespace DataCollectionPrototype.TargetWriting
{
    internal class LibraryApiWriter : ITargetWriter
    {
        public async Task WriteAsync(BookModel[] data)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7077");

            foreach (var bookModel in data)
            {
                var bookAuthorWriteViewModelList = new List<BookAuthorWriteViewModel>();
                foreach (var bookAuthor in bookModel.BookAuthors)
                {
                    var authorResponse = await client.PostAsync(
                        "/api/Authors",
                        JsonContent.Create(
                            WriteModelFromAuthorModel(bookAuthor.Author)));
                    Console.WriteLine();
                }
               var response = await client.PostAsync("/api/Books", JsonContent.Create(WriteModelFromBookModel(bookModel)));
            }
        }

        private static BookWriteViewModel WriteModelFromBookModel(BookModel bookModel)
        {
            return new BookWriteViewModel()
            {
                Title = bookModel.Title,
                Format = (LibraryService.Persistence.BookFormat?)bookModel.Format,
                Language = bookModel.Language,
                Isbn = bookModel.Isbn,
                Isbn13 = bookModel.Isbn13,
                Description = bookModel.Description,
                PubDate = bookModel.PubDate,
                CoverUrl = bookModel.CoverUrl,
                WorkId = null,
                BookAuthors = new List<BookAuthorWriteViewModel>()
                    //bookModel
                //    .BookAuthors
                //    .Select(model => new BookAuthorWriteModel
                //    {
                //        AuthorId = model.AuthorId,
                //        AuthorRole = model.AuthorRole
                //    }).ToList()
            };
        }

        private static AuthorWriteViewModel WriteModelFromAuthorModel(AuthorModel authorModel)
        {
            return new AuthorWriteViewModel
            {
                Name = authorModel.Name,
                Biography = authorModel.Biography,
                BirthDate = authorModel.BirthDate,
                DeathDate = authorModel.DeathDate
            };
        }

        private async Task SaveAuthorToDbAsync(HttpClient client, AuthorModel authorModel)
        {
            await client.PostAsync(
                "/api/Authors", 
                JsonContent.Create(
                    WriteModelFromAuthorModel(authorModel)
                )
            );
        }
    }
}
