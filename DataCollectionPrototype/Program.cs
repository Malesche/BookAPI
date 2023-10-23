using DataCollectionPrototype.Consolidation;
using DataCollectionPrototype.Core;
using DataCollectionPrototype.SourceGathering.GoogleBooks;
using DataCollectionPrototype.SourceGathering.OpenLibrary;
using DataCollectionPrototype.TargetWriting;

namespace DataCollectionPrototype
{
    public static class Program
    {
        public static async Task Main()
        {
            try
            {
                var runner = new DataCollectionRunner(
                    new IDataSourceGatherer[]
                    {
                        new GoogleBooksGatherer(),
                        new OpenLibraryGatherer()
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
    
}