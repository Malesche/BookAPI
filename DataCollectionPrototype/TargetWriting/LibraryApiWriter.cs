using DataCollectionPrototype.Core;
using DataCollectionPrototype.Core.Model;
using System.Net.Http.Json;

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
                var viewModel = new
                {
                    Title = "Peters lustige C# Geschichten",
                    Language = "Deutsch",
                    Isbn = "123",
                    Isbn13 = "123",
                    Description = "1234",
                    CoverUrl = "http://google.com",
                    BookAuthors = Array.Empty<object>(),
                    
                };

               var response = await client.PostAsync("/api/Books", JsonContent.Create(viewModel));
            }
        }
    }
}
