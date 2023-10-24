using DataCollectionPrototype.Core;
using DataCollectionPrototype.Core.Model;
using System.Net.Http.Json;
using LibraryService.Api.Authors.ViewModels;
using LibraryService.Api.Books.ViewModels;
using LibraryService.Api.Works.ViewModels;

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
                var workResponse = await client.PostAsync(
                    "/api/Works",
                    JsonContent.Create(new WorkWriteViewModel
                    {
                        Title = bookModel.Work.Title,
                        EarliestPubDate = bookModel.Work.EarliestPubDate,
                        SourceIds = bookModel.Work.SourceIds
                    }));
                var work = workResponse.Content.ReadAsAsync<WorkReadViewModel>().Result;
                int? workId = work.Id;
                var bookAuthorList = new List<BookAuthorWriteViewModel>();

                foreach (var bookAuthor in bookModel.BookAuthors)
                {
                    var authorResponse = await client.PostAsync(
                        "/api/Authors",
                        JsonContent.Create(
                            WriteModelFromAuthorModel(bookAuthor.Author)));
                    var author = authorResponse.Content.ReadAsAsync<AuthorReadViewModel>().Result;
                    bookAuthorList.Add(new BookAuthorWriteViewModel{ AuthorId = author.Id, AuthorRole = (LibraryService.Persistence.AuthorRole?)bookAuthor.AuthorRole});
                    Console.WriteLine();
                }
               var response = await client.PostAsync("/api/Books", JsonContent.Create(WriteModelFromBookModel(bookModel, workId, bookAuthorList)));
            }
        }

        private static BookWriteViewModel WriteModelFromBookModel(BookModel bookModel, int? workId, List<BookAuthorWriteViewModel> bookAuthorList)
        {
            return new BookWriteViewModel
            {
                Title = bookModel.Title,
                Format = (LibraryService.Persistence.BookFormat?)bookModel.Format,
                Language = bookModel.Language,
                Isbn = bookModel.Isbn,
                Isbn13 = bookModel.Isbn13,
                Description = bookModel.Description,
                PubDate = bookModel.PubDate,
                Publisher = bookModel.Publisher,
                CoverUrl = bookModel.CoverUrl,
                SourceIds = bookModel.SourceIds,
                WorkId = workId,
                BookAuthors = bookAuthorList
            };
        }

        private static AuthorWriteViewModel WriteModelFromAuthorModel(AuthorModel authorModel)
        {
            return new AuthorWriteViewModel
            {
                Name = authorModel.Name,
                Biography = authorModel.Biography,
                BirthDate = authorModel.BirthDate,
                DeathDate = authorModel.DeathDate,
                SourceIds = authorModel.SourceIds
            };
        }
    }
}
