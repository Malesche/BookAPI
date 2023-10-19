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
            
            try
            {
                var response = await client.GetAsync("books/OL34982429M.json");
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

            return books.ToArray();
        }

        private async Task<BookModel> ParseBookAsync(HttpClient client, HttpResponseMessage response)
        {
            var book = await response.Content.ReadAsAsync<OpenLibraryBook>();

            var bookAuthorsList = new List<BookAuthor>();
            var authorsList = new List<AuthorModel>();

            PrintBookStuff(book);

            var bookModel = new BookModel
            {
                Title = book.title,
                Format = BookFormatFromPhysicalFormat(book.physical_format),
                Language = book.languages?[0].key,
                Isbn13 = book.isbn_13?[0],
                Isbn = book.isbn_10?[0],
                PubDate = DateTimeOffsetFromPublish_date(book.publish_date),
                Publisher = book.publishers[0],
                //CoverUrl = book.covers[0].ToString(),
                SourceIds = "OpenLibrary=" + book.key
            };

            if (book.contributions != null)
            {
                foreach (var contribution in book.contributions)
                {
                    var name = contribution.Split('(', ')')[0].Trim();
                    var role = contribution.Split('(', ')')[1];
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
                    Console.WriteLine(authorJObject.ToString());

                    var currentAuthor = new AuthorModel
                    {
                        Name = (string)authorJObject["name"],
                        Biography = BiographyStringFromObjectOrString(authorJObject["bio"]),
                        BirthDate = authorJObject["birth_date"] is not null ? DateTimeOffset.Parse((string)authorJObject["birth_date"]) : null,
                        DeathDate = authorJObject["death_date"] is not null ? DateTimeOffset.Parse((string)authorJObject["death_date"]) : null,
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
            switch (physical_format)
            {
                case "hardcover":
                    return BookFormat.Hardcover;
                case "Audio CD":
                    return BookFormat.Audiobook;
            }
            Console.WriteLine("physical_format: " + physical_format);
            return BookFormat.UnknownBinding;
        }

        private DateTimeOffset? DateTimeOffsetFromPublish_date(string publishDate)
        {
            if (DateTimeOffset.TryParse(publishDate, out var result))
                return result;
            else if (Int32.TryParse(publishDate, out int intResult))
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
