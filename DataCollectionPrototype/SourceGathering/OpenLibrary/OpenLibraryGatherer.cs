using DataCollectionPrototype.Core;
using DataCollectionPrototype.Core.Model;
using DataCollectionPrototype.SourceGathering.OpenLibrary.Model;

namespace DataCollectionPrototype.SourceGathering.OpenLibrary
{
    internal class OpenLibraryGatherer : IDataSourceGatherer
    {
        private static readonly string[] Isbns =
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

        public async Task<BookModel[]> CollectAsync()
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);
            client.BaseAddress = new Uri("https://openlibrary.org/");

            var books = new List<BookModel>();

            foreach (var isbn in Isbns)
            {
                Console.WriteLine($"trying OpenLibrary: {isbn}");
                try
                {
                    var response = await client.GetAsync($"isbn/{isbn}.json");
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
            var book = await response.Content.ReadAsAsync<OpenLibraryBook>();

            var bookAuthorsList = new List<BookAuthor>();
            var authorsList = new List<AuthorModel>();
            WorkModel workModel = null;

            Console.WriteLine(book.title);

            var bookModel = OpenLibraryHelper.BookModelFromOpenLibraryBook(book);

            if (book.works is not null)
            {
                var openLibraryWork = await GetWorkAsync(client, book.works[0].key);

                if (openLibraryWork is not null)
                {
                    workModel = OpenLibraryHelper.WorkModelFromOpenLibraryWork(openLibraryWork);
                }
            }

            if (book.contributions is not null)
            {
                foreach (var contribution in book.contributions)
                {
                    var name = OpenLibraryHelper.AuthorNameFromOpenLibraryContribution(contribution);
                    var role = OpenLibraryHelper.AuthorRoleFromOpenLibraryContribution(contribution);
                    var currentAuthor = new AuthorModel { Name = name, SourceIds = "OpenLibraryContribution" };
                    authorsList.Add(currentAuthor);
                    bookAuthorsList.Add(new BookAuthor { AuthorRole = OpenLibraryHelper.StringToAuthorRole(role), Book = bookModel, Author = currentAuthor });
                }
            }

            if (book.authors is not null)
            {
                foreach (var a in book.authors)
                {
                    var authorJsonString = await GetAuthorAsync(client, a.key);
                    var currentAuthor = OpenLibraryHelper.AuthorModelFromJsonString(authorJsonString);

                    authorsList.Add(currentAuthor);
                    bookAuthorsList.Add(new BookAuthor { AuthorRole = AuthorRole.Author, Book = bookModel, Author = currentAuthor });
                }
            }

            bookModel.Work = workModel;
            bookModel.Authors = authorsList;
            bookModel.BookAuthors = bookAuthorsList;

            return bookModel;
        }

        private async Task<OpenLibraryWork> GetWorkAsync(HttpClient client, string workKey)
        {
            HttpResponseMessage response = await client.GetAsync($"{workKey}.json");
            if (response.IsSuccessStatusCode)
            {
                var work = await response.Content.ReadAsAsync<OpenLibraryWork>();
                Console.WriteLine($"work: {work.title}");
                Console.WriteLine(work.first_publish_date);

                return work;
            }

            return null;
        }

        private async Task<string> GetAuthorAsync(HttpClient client, string authorKey)
        {
            HttpResponseMessage response = await client.GetAsync($"{authorKey}.json");
            if (response.IsSuccessStatusCode)
            {
                var authorJsonString = await response.Content.ReadAsStringAsync();

                return authorJsonString;
            }

            return null;
        }
    }
}
