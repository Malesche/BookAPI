using DataCollectionPrototype.Consolidation;
using DataCollectionPrototype.SourceGathering.OpenLibrary;
using DataCollectionPrototype.TargetWriting;

namespace DataCollectionPrototype.Core
{
    internal sealed class DataCollectionRunner
    {
        private readonly RunnerConfiguration _configuration;

        private readonly IDataSourceGatherer[] _gatherers;
        private readonly IDataConsolidator _consolidator;
        private readonly ITargetWriter _targetWriter;

        public DataCollectionRunner(
            IEnumerable<IDataSourceGatherer> gatherers,
            IDataConsolidator consolidator,
            ITargetWriter targetWriter,
            RunnerConfiguration configuration)
        {
            _gatherers = gatherers.ToArray();
            _consolidator = consolidator;
            _targetWriter = targetWriter;
            _configuration = configuration;
        }

        public async Task RunAsync()
        {
            var booksFromAllSources = new List<object>();

            foreach (var gatherer in _gatherers)
            {
                var books = await gatherer.CollectAsync();
                booksFromAllSources.AddRange(books);
            }

            var consolidatedBooks = await _consolidator.ConsolidateAsync(booksFromAllSources);

            await _targetWriter.WriteAsync(consolidatedBooks);
        }
    }
}
