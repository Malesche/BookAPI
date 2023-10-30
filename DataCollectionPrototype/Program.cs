using DataCollectionPrototype.Consolidation;
using DataCollectionPrototype.Core;
using DataCollectionPrototype.SourceGathering.GoogleBooks;
using DataCollectionPrototype.SourceGathering.OpenLibrary;
using DataCollectionPrototype.SourceGathering.OpenLibrary.BulkReader;
using DataCollectionPrototype.TargetWriting;

namespace DataCollectionPrototype;

public static class Program
{
    public static async Task Main()
    {
        try
        {
            var runner = new DataCollectionRunner(
                new IDataSourceGatherer[]
                {
                    new OpenLibraryBulkReader()
                    //new OpenLibraryGatherer()
                    //new GoogleBooksGatherer(),
                },
                new PassThroughConsolidator(),
                new LibraryApiWriter(),
                new RunnerConfiguration()
            );

            await runner.RunAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}