using DataCollectionPrototype.Core;
using DataCollectionPrototype.SourceGathering.OpenLibrary.Model;
using Newtonsoft.Json;
using DataCollectionPrototype.Core.Model;
using System.Diagnostics;

namespace DataCollectionPrototype.SourceGathering.OpenLibrary.BulkReader;

internal class OpenLibraryBulkReader: IDataSourceGatherer
{
    private const string RootPath = @"C:\Users\praktikum\Downloads\";
    private const string EditionsPath = $"{RootPath}ol_dump_editions_2023-09-30.txt";
    private const string WorksPath = $"{RootPath}ol_dump_works_2023-09-30.txt";
    private const string AuthorsPath = $"{RootPath}ol_dump_authors_2023-09-30.txt";
    private const int EditionsToImport = 5000000;
    private const char ItemSeparator = '\t';
    private const string SourceTypeIdentifier = "OpenLibraryContribution";

    public Task<BookModel[]> CollectAsync()
    {
        var timer = Stopwatch.StartNew();

        var editionsToSave = new Dictionary<string, BookModel>();
        var authorsToFindForEditions = new Dictionary<string, List<string>>();
        var worksToFindForEditions = new Dictionary<string, List<string>>();

        using var editionsReader = new FileReader(EditionsPath);

        for (var i = EditionsToImport; i > 0; i--)
        {
            var currentLine = editionsReader.ReadNextLine();
            if (currentLine == null) break;

            var values = currentLine
                .Split(new[] { ItemSeparator }, StringSplitOptions.RemoveEmptyEntries)
                .Where(token => !string.IsNullOrWhiteSpace(token))
                .ToArray();

            var book = JsonConvert.DeserializeObject<OpenLibraryBook>(values[4]);
            if (string.IsNullOrWhiteSpace(book.title))
                continue;
            var bookModel = OpenLibraryHelper.BookModelFromOpenLibraryBook(book);

            editionsToSave.Add(book.key, bookModel);

            if (book.contributions is not null)
            {
                foreach (var contribution in book.contributions)
                {
                    var name = OpenLibraryHelper.AuthorNameFromOpenLibraryContribution(contribution);
                    if (string.IsNullOrWhiteSpace(name))
                        continue;
                    var role = OpenLibraryHelper.AuthorRoleFromOpenLibraryContribution(contribution);
                    var currentAuthor = new AuthorModel { 
                        Name = name, 
                        SourceIds = SourceTypeIdentifier };
                    bookModel.Authors.Add(currentAuthor);
                    bookModel.BookAuthors.Add(new BookAuthor { 
                        AuthorRole = OpenLibraryHelper.StringToAuthorRole(role), 
                        Book = bookModel, 
                        Author = currentAuthor });
                }
            }

            if (book.authors is not null)
            {
                foreach (var a in book.authors)
                {
                    //Console.WriteLine($"adding author to list:      {a.key}");
                    AddToListDictionary(authorsToFindForEditions, a.key, book.key);
                }
            }

            if (book.works is not null)
            {
                //Console.WriteLine($"adding work to list:        {book.works[0].key}");
                AddToListDictionary(worksToFindForEditions, book.works[0].key, book.key);
            }
        }
        
        Console.WriteLine($"--------------------------------------");
        Console.WriteLine($"searching for authors in authors file");
        AddAuthorsToEditions(authorsToFindForEditions, editionsToSave, AuthorsPath);
        Console.WriteLine($"--------------------------------------");
        Console.WriteLine($"searching for works in works file");
        AddWorksToEditions(worksToFindForEditions, editionsToSave, WorksPath);

        var bookModelList = editionsToSave.Select(x => x.Value);

        timer.Stop();
        Console.WriteLine($"Time that OpenLibraryBulkReader CollectAsync took for {EditionsToImport} books: {timer.Elapsed}");
        return Task.FromResult(bookModelList.ToArray());
    }

    private static void AddWorksToEditions(
        IDictionary<string, List<string>> worksToFindForEditions, 
        IReadOnlyDictionary<string, BookModel> editionsToSave, 
        string worksFilePath)
    {
        var worksReader = new FileReader(worksFilePath);

        var currentLine = worksReader.ReadNextLine();
        while (currentLine is not null && worksToFindForEditions.Count > 0)
        {
            var values = currentLine.Split(ItemSeparator);
            if (!worksToFindForEditions.ContainsKey(values[1]))
            {
                currentLine = worksReader.ReadNextLine();
                continue;
            }

            //Console.WriteLine($"still looking for {worksToFindForEditions.Count}    works");
            var workKey = values[1];
            var openLibraryWork = JsonConvert.DeserializeObject<OpenLibraryWork>(values[4]);
            if (string.IsNullOrWhiteSpace(openLibraryWork.title))
            {
                currentLine = worksReader.ReadNextLine();
                continue;
            }
            var workModel = OpenLibraryHelper.WorkModelFromOpenLibraryWork(openLibraryWork);

            foreach (var edition in worksToFindForEditions[workKey])
            {
                editionsToSave[edition].Work = workModel;
            }

            worksToFindForEditions.Remove(workKey);

            currentLine = worksReader.ReadNextLine();
        }
        Console.WriteLine("done searching for works.");
    }

    private void AddAuthorsToEditions(
        Dictionary<string, List<string>> authorsToFindForEditions, 
        Dictionary<string, BookModel> editionsToSave, 
        string authorsFilePath)
    {
        var authorsReader = new FileReader(authorsFilePath);

        var currentLine = authorsReader.ReadNextLine();
        while (currentLine is not null && authorsToFindForEditions.Count > 0)
        {
            var values = currentLine.Split(ItemSeparator);
            if (!authorsToFindForEditions.ContainsKey(values[1]))
            {
                currentLine = authorsReader.ReadNextLine();
                continue;
            }

            //Console.WriteLine($"still looking for {authorsToFindForEditions.Count}  authors");
            var authorKey = values[1];
            var authorModel = OpenLibraryHelper.AuthorModelFromJsonString(values[4]);
            if (string.IsNullOrWhiteSpace(authorModel.Name))
            {
                currentLine = authorsReader.ReadNextLine();
                continue;
            }

            foreach (var edition in authorsToFindForEditions[authorKey])
            {
                var bookModel = editionsToSave[edition];
                var bookAuthor = new BookAuthor
                {
                    Book = bookModel,
                    Author = authorModel, 
                    AuthorRole = AuthorRole.Author
                };
                bookModel.BookAuthors.Add(bookAuthor);
            }

            authorsToFindForEditions.Remove(authorKey);
        }
        Console.WriteLine("done searching for authors");
    }

    private static void AddToListDictionary(
        Dictionary<string, List<string>> elementsToFindForEditions, 
        string elementKey, 
        string editionKey)
    {
        if (!elementsToFindForEditions.TryGetValue(elementKey, out var editions))
        {
            editions = new List<string> { editionKey };
            elementsToFindForEditions[elementKey] = editions;
        }

        editions.Add(editionKey);
    }
}