using DataCollectionPrototype.Core;
using DataCollectionPrototype.Core.Model;
using DataCollectionPrototype.SourceGathering.GoogleBooks.Model;

namespace DataCollectionPrototype.SourceGathering.GoogleBooks
{
    internal class GoogleBooksGatherer : IDataSourceGatherer
    {
        public async Task<BookModel[]> CollectAsync()
        {
            using var client = new HttpClient();
            // https://www.googleapis.com/books/v1/volumes?q=isbn:
            client.BaseAddress = new Uri("https://www.googleapis.com/books/");

            var books = new List<BookModel>();
            var isbnList = new List<string>
            {
                "9781797161075",
                "9780231184083",
                "9780593317211",
                "9781786633071",
                "9781797157573",
                "9781250900906",
                "9781797156132",
                "9780156252348",
                "9781642861044",
                "9780593421062",
                "9781617298301",
                "9783839812372",
                "9780745634272",
                "9783596147700"

                //"1400128633",
                //"0374600902",
                //"1250114578",
                //"1690598433",
                //"1324065400",
                //"0374600457",
                //"9798200717064",
                //"9781400128631",
                //"9780374600907",
                //"9781250114570",
                //"9781690598435",
                //"9781324065401",
                //"9780374600457"

                //"9781427268297",
                //"9798212272674",
                //"9780735239661",
                //"9781558328952",
                //"",
                //"",
                //"9781250838001",
                //"",
                //"9781797221410",
                //"9781483015996",
                //"9780593539644",
                //"9781101871515"

                //"9783813503708",
                //"9780593539569",
                //"9781774582664",
                //"9781957363622",
                //"9780679738046",
                //"9781632157096",
                //"9783832182915",
                //"9783867176958",
                //"9780063277014",
                //"9781250821553"
            };

            foreach (var isbn in isbnList)
            {
                Console.WriteLine("trying GoogleBooks:" + isbn);
                try
                {
                    var response = await client.GetAsync("v1/volumes?q=isbn:" + isbn + "&key=XXXXXXXXXXXX");
                    if (response.IsSuccessStatusCode)
                    {
                        var bookModel = await ParseBookAsync(client, response);
                        books.Add(bookModel);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return books.ToArray();
        }

        private async Task<BookModel> ParseBookAsync(HttpClient client, HttpResponseMessage response)
        {
            var searchResult = await response.Content.ReadAsAsync<GoogleSearchResult>();
            var book = searchResult.items?[0];

            if (book == null)
                return new BookModel();

            Console.WriteLine(book.volumeInfo.title);
            var bookModel = new BookModel
            {
                Title = book.volumeInfo.title,
                //Format = BookFormatFromPhysicalFormat(book.physical_format),
                Language = book.volumeInfo.language,
                Isbn13 = PickIsbnFromIndustryIdentifiers(book.volumeInfo.industryIdentifiers, "ISBN_13"),
                Isbn = PickIsbnFromIndustryIdentifiers(book.volumeInfo.industryIdentifiers, "ISBN_10"),
                Description = book.volumeInfo.description,
                PubDate = DateTimeOffsetFromString(book.volumeInfo.publishedDate),
                Publisher = book.volumeInfo.publisher,
                CoverUrl = book.volumeInfo.imageLinks is not null ? book.volumeInfo.imageLinks.thumbnail : null,
                SourceIds = "GoogleBooks=" + book.id
            };

            return bookModel;
        }

        private DateTimeOffset? DateTimeOffsetFromString(string dateString)
        {
            if (DateTimeOffset.TryParse(dateString, out var result))
                return result;
            if (int.TryParse(dateString, out int intResult))
            {
                return new DateTimeOffset(intResult, 1, 1, 0, 0, 0, TimeSpan.Zero);
            }
            return null;
        }

        private string PickIsbnFromIndustryIdentifiers(Industryidentifier[] industryIdentifiers, string isbnType)
        {
            var ii = industryIdentifiers.FirstOrDefault(ii => ii.type == isbnType);
            return ii?.identifier;
        }
    }
}
