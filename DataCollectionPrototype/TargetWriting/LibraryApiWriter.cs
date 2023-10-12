using DataCollectionPrototype.Core;
using DataCollectionPrototype.Core.Model;
using System.Net.Http.Json;
using LibraryService.Api.Books.ViewModels;

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
                //var viewModel = new
                //{
                //    Title = "Peters lustige C# Geschichten",
                //    Language = "Deutsch",
                //    Isbn = "123",
                //    Isbn13 = "123",
                //    Description = "1234",
                //    CoverUrl = "http://google.com",
                //    BookAuthors = Array.Empty<object>(),
                    
                //};
               var response = await client.PostAsync("/api/Books", JsonContent.Create(WriteModelFromBookModel(bookModel)));
            }
        }

        private static BookWriteViewModel WriteModelFromBookModel(BookModel bookModel)
        {
            return new BookWriteViewModel()
            {
                Title = bookModel.Title,
                //Format = bookModel.Format,
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
    }
}
