using Newtonsoft.Json.Linq;
using DataCollectionPrototype.Core.Model;
using DataCollectionPrototype.SourceGathering.OpenLibrary.Model;

namespace DataCollectionPrototype.SourceGathering.OpenLibrary;

internal static class OpenLibraryHelper
{
    internal static BookModel BookModelFromOpenLibraryBook(OpenLibraryBook book)
    {
        return new BookModel
        {
            Title = book.title,
            Format = BookFormatFromPhysicalFormat(book.physical_format),
            Language = book.languages?.FirstOrDefault()?.key,
            Isbn13 = book.isbn_13?.FirstOrDefault(),
            Isbn = book.isbn_10?.FirstOrDefault(),
            PubDate = DateTimeOffsetFromString(book.publish_date),
            Publisher = book.publishers?.FirstOrDefault(),
            CoverUrl = book.covers is null ? null : $"https://covers.openlibrary.org/b/id/{book.covers.FirstOrDefault()}-M.jpg",
            SourceIds = { SourceId.Create("OpenLibrary", book.key) }
        };
    }

    internal static WorkModel WorkModelFromOpenLibraryWork(OpenLibraryWork work)
    {
        return new WorkModel
        {
            Title = work.title,
            EarliestPubDate = DateTimeOffsetFromString(work.first_publish_date),
            SourceIds = $"OpenLibrary={work.key}"
        };
    }

    internal static AuthorModel AuthorModelFromJsonString(string authorJsonString)
    {
        JObject authorJObject = JObject.Parse(authorJsonString);

        return new AuthorModel
        {
            Name = (string)authorJObject["name"],
            Biography = BiographyStringFromObjectOrString(authorJObject["bio"]),
            BirthDate = authorJObject["birth_date"] is not null ? DateTimeOffsetFromString((string)authorJObject["birth_date"]) : null,
            DeathDate = authorJObject["death_date"] is not null ? DateTimeOffsetFromString((string)authorJObject["death_date"]) : null,
            SourceIds = $"OpenLibrary={(string)authorJObject["key"]}"
        };
    }

    internal static string AuthorNameFromOpenLibraryContribution(string contribution)
    {
        return contribution.Split('(', ')')[0].Trim();
    }

    internal static string AuthorRoleFromOpenLibraryContribution(string contribution)
    {
        return contribution.Split('(', ')').Length > 1 ? contribution.Split('(', ')')[1] : "unknown role";
    }

    internal static string BiographyStringFromObjectOrString(JToken bio)
    {
        if (bio == null)
            return null;
        if (bio.Type == JTokenType.Object)
            return (string)bio["value"];

        return (string)bio;
    }

    internal static BookFormat BookFormatFromPhysicalFormat(string physicalFormat)
    {
        //Console.WriteLine($"physical_format: {physicalFormat}");
        return physicalFormat switch
        {
            "Hardcover" => BookFormat.Hardcover,
            "hardcover" => BookFormat.Hardcover,
            "Audio CD" => BookFormat.Audiobook,
            "Paperback" => BookFormat.Paperback,
            "paperback" => BookFormat.Paperback,
            "Epub" => BookFormat.Ebook,
            _ => BookFormat.UnknownBinding
        };
    }

    internal static DateTimeOffset? DateTimeOffsetFromString(string dateString)
    {
        if (DateTimeOffset.TryParse(dateString, out var result))
            return result;
        else if (Int32.TryParse(dateString, out int intResult))
        {
            if (0 < intResult && intResult < DateTime.Now.Year)
                return new DateTimeOffset(intResult, 1, 1, 0, 0, 0, TimeSpan.Zero);
        }
        return null;
    }

    internal static AuthorRole? StringToAuthorRole(string role)
    {
        return role switch
        {
            "Author" => AuthorRole.Author,
            "Narrator" => AuthorRole.Narrator,
            "Translator" => AuthorRole.Translator,
            "Illustrator" => AuthorRole.Illustrator,
            "Editor" => AuthorRole.Editor,
            "Contributor" => AuthorRole.Contributor,
            _ => null
        };
    }
}