using DataCollectionPrototype.Core;
using DataCollectionPrototype.Core.Model;
using DataCollectionPrototype.SourceGathering.OpenLibrary.Model;

namespace DataCollectionPrototype.SourceGathering.OpenLibrary
{
    internal class OpenLibraryGatherer : IDataSourceGatherer
    {
        public async Task<BookModel[]> CollectAsync()
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://openlibrary.org/");
            OpenLibraryBook book = null;

            HttpResponseMessage response = await client.GetAsync("books/OL8364844M.json");
            if (response.IsSuccessStatusCode)
            {
                book = await response.Content.ReadAsAsync<OpenLibraryBook>();

                Console.WriteLine(book.title);
            }

            return new BookModel[]
            {
                new BookModel
                {
                    Title = book.title,
                    Isbn13 = book.isbn_13[0],
                    Isbn = book.isbn_10[0]
                }
            };
        }
    }
}
