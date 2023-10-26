using DataCollectionPrototype.Core;
using DataCollectionPrototype.SourceGathering.OpenLibrary.Model;
using Newtonsoft.Json;
using DataCollectionPrototype.Core.Model;

namespace DataCollectionPrototype.SourceGathering.OpenLibrary.BulkReader
{
    internal class OpenLibraryBulkReader: IDataSourceGatherer
    {
        public async Task<BookModel[]> CollectAsync()
        {
            var editionsPath = @"C:\Users\praktikum\Downloads\ol_dump_editions_2023-09-30.txt";
            var worksPath = @"C:\Users\praktikum\Downloads\ol_dump_works_2023-09-30.txt";
            var authorsPath = @"C:\Users\praktikum\Downloads\ol_dump_authors_2023-09-30.txt";

            var editionsToSave = new Dictionary<string, BookModel>();
            var authorsToFindForEditions = new Dictionary<string, List<string>>();
            var worksToFindForEditions = new Dictionary<string, List<string>>();

            var editionsReader = new FileReader(editionsPath);

            string currentLine;
            var i = 10;
            while ((currentLine = editionsReader.ReadNextLine()) != null && i > 0)
            {
                i--;
                var values = currentLine.Split('\t');
                var book = JsonConvert.DeserializeObject<OpenLibraryBook>(values[4]);
                Console.WriteLine($"{book.title}");
                var bookModel = OpenLibraryHelper.BookModelFromOpenLibraryBook(book);

                editionsToSave.Add(book.key, bookModel);

                if (book.contributions is not null)
                {
                    foreach (var contribution in book.contributions)
                    {
                        var name = OpenLibraryHelper.AuthorNameFromOpenLibraryContribution(contribution);
                        var role = OpenLibraryHelper.AuthorRoleFromOpenLibraryContribution(contribution);
                        var currentAuthor = new AuthorModel { Name = name, SourceIds = "OpenLibraryContribution" };
                        bookModel.Authors.Add(currentAuthor);
                        bookModel.BookAuthors.Add(new BookAuthor { AuthorRole = OpenLibraryHelper.StringToAuthorRole(role), Book = bookModel, Author = currentAuthor });
                    }
                }

                if (book.authors is not null)
                {
                    foreach (var a in book.authors)
                    {
                        Console.WriteLine($"{a.key}");
                        AddToListDictionary(authorsToFindForEditions, a.key, book.key);
                    }
                }

                if (book.works is not null)
                {
                    Console.WriteLine($"{book.works[0].key}");
                    AddToListDictionary(worksToFindForEditions, book.works[0].key, book.key);
                }
            }

            AddAuthorsToEditions(authorsToFindForEditions, editionsToSave, authorsPath);
            AddWorksToEditions(worksToFindForEditions, editionsToSave, worksPath);

            var bookModelList = editionsToSave.Select(x => x.Value);
            return bookModelList.ToArray();
        }

        private void AddWorksToEditions(Dictionary<string, List<string>> worksToFindForEditions, Dictionary<string, BookModel> editionsToSave, string worksFilePath)
        {
            var worksReader = new FileReader(worksFilePath);

            string currentLine;
            while ((currentLine = worksReader.ReadNextLine()) is not null && worksToFindForEditions.Count > 0)
            {
                var values = currentLine.Split('\t');
                if (worksToFindForEditions.ContainsKey(values[1]))
                {
                    Console.WriteLine(currentLine);
                    var workKey = values[1];
                    var openLibraryWork = JsonConvert.DeserializeObject<OpenLibraryWork>(values[4]);
                    var workModel = OpenLibraryHelper.WorkModelFromOpenLibraryWork(openLibraryWork);

                    foreach (string edition in worksToFindForEditions[workKey])
                    {
                        editionsToSave[edition].Work = workModel;
                    }

                    worksToFindForEditions.Remove(workKey);
                }
            }
        }
    
        private void AddAuthorsToEditions(Dictionary<string, List<string>> authorsToFindForEditions, Dictionary<string, BookModel> editionsToSave, string authorsFilePath)
        {
            var authorsReader = new FileReader(authorsFilePath);

            string currentLine;
            while ((currentLine = authorsReader.ReadNextLine()) is not null && authorsToFindForEditions.Count > 0)
            {
                var values = currentLine.Split('\t');
                if (authorsToFindForEditions.ContainsKey(values[1]))
                {
                    Console.WriteLine(currentLine);
                    var authorKey = values[1];
                    var authorModel = OpenLibraryHelper.AuthorModelFromJsonString(values[4]);

                    foreach (string edition in authorsToFindForEditions[authorKey])
                    {
                        var bookModel = editionsToSave[edition];
                        var bookAuthor = new BookAuthor { Book = bookModel, Author = authorModel, AuthorRole = AuthorRole.Author};
                        bookModel.BookAuthors.Add(bookAuthor);
                    }

                    authorsToFindForEditions.Remove(authorKey);
                }
            }
        }

        private void AddToListDictionary(Dictionary<string, List<string>> ElementsToFindForEditions,string elementKey, string editionKey)
        {
            if (ElementsToFindForEditions.ContainsKey(elementKey))
            {
                ElementsToFindForEditions[elementKey].Add(editionKey);
            }
            else
            {
                ElementsToFindForEditions.Add(elementKey, new List<string> { editionKey });
            }
        }
    }
}
