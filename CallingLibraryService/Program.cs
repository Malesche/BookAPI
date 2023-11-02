using DataCollectionPrototype.SourceGathering.OpenLibrary.BulkReader;
using LibraryService.Api.Authors.ViewModels;
using LibraryService.Persistence;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Http.Json;

namespace CallingLibraryService;


internal class Program
{
    private const string RootPath = @"C:\Users\praktikum\Downloads\";
    private const string AuthorsPath = $"{RootPath}ol_dump_authors_2023-09-30.txt";
    private const int AuthorsToImport = 10000;
    private const char ItemSeparator = '\t';

    static async Task Main(string[] args)
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:7077");

        var authorsToSave = GetListOfAuthorWriteViewModels(AuthorsToImport);

        var timer2 = new Stopwatch();
        timer2.Start();

        foreach (var author in authorsToSave)
        {
            var jsonAuthor = JsonContent.Create(author);
            await client.PostAsync("/api/Authors", jsonAuthor);
        }

        Console.WriteLine($"Time it took to write {AuthorsToImport} authors to the database with separate Creates: {timer2.Elapsed}");

        var timer = new Stopwatch();
        timer.Start();

        var jsonAuthorsToSave = JsonContent.Create(authorsToSave);
        await client.PostAsync("/api/Authors/CreateSeveral", jsonAuthorsToSave);

        Console.WriteLine($"Time it took to write {AuthorsToImport} authors to the database with CreateSeveral: {timer.Elapsed}");
    }

    private static List<AuthorWriteViewModel> GetListOfAuthorWriteViewModels(int AuthorsToImport)
    {
        var authorsToSave = new List<AuthorWriteViewModel>();
        var authorsReader = new FileReader(AuthorsPath);

        for (var i = AuthorsToImport; i > 0; i--)
        {
            var currentLine = authorsReader.ReadNextLine();
            if (currentLine == null) break;

            var values = currentLine
                .Split(new[] { ItemSeparator }, StringSplitOptions.RemoveEmptyEntries)
                .Where(token => !string.IsNullOrWhiteSpace(token))
                .ToArray();

            var authorKey = values[1];
            var authorModel = AuthorModelFromJsonString(values[4]);
            if (string.IsNullOrWhiteSpace(authorModel.Name))
            {
                currentLine = authorsReader.ReadNextLine();
                continue;
            }

            authorsToSave.Add(authorModel);
        }

        return authorsToSave;
    }
    

    internal static AuthorWriteViewModel AuthorModelFromJsonString(string authorJsonString)
    {
        JObject authorJObject = JObject.Parse(authorJsonString);

        return new AuthorWriteViewModel
        {
            Name = (string)authorJObject["name"],
            Biography = BiographyStringFromObjectOrString(authorJObject["bio"]),
            BirthDate = authorJObject["birth_date"] is not null ? DateTimeOffsetFromString((string)authorJObject["birth_date"]) : null,
            DeathDate = authorJObject["death_date"] is not null ? DateTimeOffsetFromString((string)authorJObject["death_date"]) : null,
            SourceIds = $"OpenLibrary={(string)authorJObject["key"]}"
        };
    }

    internal static string BiographyStringFromObjectOrString(JToken bio)
    {
        if (bio == null)
            return null;
        if (bio.Type == JTokenType.Object)
            return (string)bio["value"];

        return (string)bio;
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
}

