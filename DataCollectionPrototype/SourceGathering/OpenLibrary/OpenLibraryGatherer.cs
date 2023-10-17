using DataCollectionPrototype.Core;
using DataCollectionPrototype.Core.Model;
using DataCollectionPrototype.SourceGathering.OpenLibrary.Model;
using Newtonsoft.Json.Linq;

namespace DataCollectionPrototype.SourceGathering.OpenLibrary
{
    // trending/daily.json
    // fantastic mr fox:     books/OL7353617M.json
    // der kleine hobbit:    books/OL9032793M.json
    // middlemarch, parse fehler bio:       books/OL7360063M.json
    //                                      books/OL8364844M.json
    // Roald Dahl: /authors/OL34184A
    // George Eliot: /authors/OL24528A

    internal class OpenLibraryGatherer : IDataSourceGatherer
    {
        public async Task<BookModel[]> CollectAsync()
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://openlibrary.org/");
            OpenLibraryBook book;
            BookModel bookModel = null;
            var bookAuthorsList = new List<BookAuthor>();
            var authorsList = new List<AuthorModel>();

            HttpResponseMessage response = await client.GetAsync("books/OL7353617M.json");
            if (response.IsSuccessStatusCode)
            {
                book = await response.Content.ReadAsAsync<OpenLibraryBook>();

                PrintBookStuff(book);

                bookModel = new BookModel
                {
                    Title = book.title,
                    Format = BookFormatFromPhysicalFormat(book.physical_format),
                    Language = book.languages?[0].key,
                    Isbn13 = book.isbn_13?[0],
                    Isbn = book.isbn_10?[0],
                };

                if (book.contributions != null)
                {
                    foreach (var contribution in book.contributions)
                    {
                        var name = contribution.Split('(', ')')[0].Trim();
                        var role = contribution.Split('(', ')')[1];
                        Console.WriteLine($"contributor: {name}, {role}");
                        var currentAuthor = new AuthorModel { Name = name };
                        authorsList.Add( currentAuthor );
                        bookAuthorsList.Add( new BookAuthor {AuthorRole = StringToAuthorRole(role), Book = bookModel, Author = currentAuthor});
                    }
                }

                if (book.authors != null)
                {
                    foreach (var a in book.authors)
                    {
                        var authorJsonString = await GetAuthorAsync(client, a.key);
                        JObject authorJObject = JObject.Parse(authorJsonString);
                        // Console.WriteLine(authorJObject.ToString());

                        var currentAuthor = new AuthorModel
                        {
                            Name = (string)authorJObject["name"],
                            Biography = BiographyStringFromObjectOrString(authorJObject["bio"]),
                            BirthDate = authorJObject["birth_date"] is not null ? DateTimeOffset.Parse((string)authorJObject["birth_date"]) : null,
                            DeathDate = authorJObject["death_date"] is not null ? DateTimeOffset.Parse((string)authorJObject["death_date"]) : null,
                        };
                        authorsList.Add(currentAuthor);
                        bookAuthorsList.Add(new BookAuthor { AuthorRole = AuthorRole.Author, Book = bookModel, Author = currentAuthor });
                    }
                }
            }

            bookModel.Authors = authorsList;
            bookModel.BookAuthors = bookAuthorsList;

            return new BookModel[]
            {
                bookModel
            };
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
            return BookFormat.UnknownBinding;
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
