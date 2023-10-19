using DataCollectionPrototype.Core;
using DataCollectionPrototype.Core.Model;
using DataCollectionPrototype.SourceGathering.OpenLibrary.Model;
using Newtonsoft.Json.Linq;

namespace DataCollectionPrototype.SourceGathering.OpenLibrary
{
    internal class OpenLibraryGatherer : IDataSourceGatherer
    {
        public async Task<BookModel[]> CollectAsync()
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);
            client.BaseAddress = new Uri("https://openlibrary.org/");

            var books = new List<BookModel>();

            var isbnList = new List<string>
            {
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

                "9783813503708",
                "9780593539569",
                "9781774582664",
                "9781957363622",
                "9780679738046",
                "9781632157096",
                "9783832182915",
                "9783867176958",
                "9780063277014",
                "9781250821553"


            };

            foreach (var isbn in isbnList)
            {
                Console.WriteLine("trying " + isbn);
                try
                {
                    var response = await client.GetAsync("isbn/" + isbn + ".json");
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

            Console.WriteLine(book.title);
            //PrintBookStuff(book);

            var bookModel = new BookModel
            {
                Title = book.title,
                Format = BookFormatFromPhysicalFormat(book.physical_format),
                Language = book.languages?[0].key,
                Isbn13 = book.isbn_13?[0],
                Isbn = book.isbn_10?[0],
                PubDate = DateTimeOffsetFromString(book.publish_date),
                Publisher = book.publishers?[0],
                //CoverUrl = book.covers[0]?.ToString(),
                SourceIds = "OpenLibrary=" + book.key
            };

            if (book.contributions != null)
            {
                foreach (var contribution in book.contributions)
                {
                    var name = contribution.Split('(', ')')[0].Trim();
                    var role = contribution.Split('(', ')').Length > 1 ? contribution.Split('(', ')')[1] : "unknown role";
                    Console.WriteLine($"contributor: {name}, {role}");
                    var currentAuthor = new AuthorModel { Name = name, SourceIds = "OpenLibraryContribution" };
                    authorsList.Add(currentAuthor);
                    bookAuthorsList.Add(new BookAuthor { AuthorRole = StringToAuthorRole(role), Book = bookModel, Author = currentAuthor });
                }
            }

            if (book.authors != null)
            {
                foreach (var a in book.authors)
                {
                    var authorJsonString = await GetAuthorAsync(client, a.key);
                    JObject authorJObject = JObject.Parse(authorJsonString);
                    //Console.WriteLine(authorJObject.ToString());

                    var currentAuthor = new AuthorModel
                    {
                        Name = (string)authorJObject["name"],
                        Biography = BiographyStringFromObjectOrString(authorJObject["bio"]),
                        BirthDate = authorJObject["birth_date"] is not null ? DateTimeOffsetFromString((string)authorJObject["birth_date"]) : null,
                        DeathDate = authorJObject["death_date"] is not null ? DateTimeOffsetFromString((string)authorJObject["death_date"]) : null,
                        SourceIds = "OpenLibrary=" + (string)authorJObject["key"]
                    };
                    authorsList.Add(currentAuthor);
                    bookAuthorsList.Add(new BookAuthor { AuthorRole = AuthorRole.Author, Book = bookModel, Author = currentAuthor });
                }
            }
            bookModel.Authors = authorsList;
            bookModel.BookAuthors = bookAuthorsList;

            return bookModel;
        }

        private async Task<string> GetAuthorAsync(HttpClient client, string authorKey)
        {
            HttpResponseMessage response = await client.GetAsync(authorKey + ".json");
            if (response.IsSuccessStatusCode)
            {
                var authorJsonString = await response.Content.ReadAsStringAsync();

                return authorJsonString;
            }

            return null;
        }

        private string BiographyStringFromObjectOrString(JToken bio)
        {
            if (bio == null)
                return null;
            if (bio.Type == JTokenType.Object)
                return (string)bio["value"];

            return (string)bio;
        }

        private BookFormat BookFormatFromPhysicalFormat(string physical_format)
        {   
            Console.WriteLine("physical_format: " + physical_format);
            switch (physical_format)
            {
                case "Hardcover":
                    return BookFormat.Hardcover;
                case "hardcover":
                    return BookFormat.Hardcover;
                case "Audio CD":
                    return BookFormat.Audiobook;
                case "Paperback":
                    return BookFormat.Paperback;
                case "paperback":
                    return BookFormat.Paperback;
            }
            return BookFormat.UnknownBinding;
        }

        private DateTimeOffset? DateTimeOffsetFromString(string dateString)
        {
            if (DateTimeOffset.TryParse(dateString, out var result))
                return result;
            else if (Int32.TryParse(dateString, out int intResult))
            {
                return new DateTimeOffset(intResult, 1, 1, 0, 0, 0, TimeSpan.Zero);
            }
            return null;
        }

        private AuthorRole? StringToAuthorRole(string role)
        {
            switch (role)
            {
                case "Narrator":
                    return AuthorRole.Narrator;
                case "Translator":
                    return AuthorRole.Translator;
                case "Illustrator":
                    return AuthorRole.Illustrator;
                case "Editor":
                    return AuthorRole.Editor;
                case "Contributor":
                    return AuthorRole.Contributor;
                case "unknown role":
                    return AuthorRole.Contributor;
            }

            Console.WriteLine("Author Role: " + role);
            return null;
        }
            
        private void PrintBookStuff(OpenLibraryBook book)
        {
            Console.WriteLine(book.title);
            Console.WriteLine(book.key);
            Console.WriteLine(BookFormatFromPhysicalFormat(book.physical_format));

            if (book.contributions != null)
            {
                foreach (var contribution in book.contributions)
                {
                    Console.WriteLine(
                        $"contributor: {contribution.Split('(', ')')[0].Trim()}, {contribution.Split('(', ')')[1]}");
                }
            }

            if (book.authors != null)
            {
                foreach (var a in book.authors)
                {
                    Console.WriteLine(a.key);
                }
            }
        }
    }
}
