using DataCollectionPrototype.Core;
using DataCollectionPrototype.SourceGathering.OpenLibrary.Model;

namespace DataCollectionPrototype.SourceGathering.OpenLibrary
{
    internal class OpenLibraryGatherer : IDataSourceGatherer
    {
        public async Task<object[]> CollectAsync()
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://openlibrary.org/");
            OpenLibraryBook book = null;

            HttpResponseMessage response = await client.GetAsync("books/OL8364844M");
            if (response.IsSuccessStatusCode)
            {
                book = await response.Content.ReadAsAsync<OpenLibraryBook>();
            }

            return new object[]
            {
                book
            };
        }
    }
}
