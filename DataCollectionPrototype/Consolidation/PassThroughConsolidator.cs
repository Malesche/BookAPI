using DataCollectionPrototype.Core;
using DataCollectionPrototype.Core.Model;

namespace DataCollectionPrototype.Consolidation
{
    internal class PassThroughConsolidator : IDataConsolidator
    {
        public Task<BookModel[]> ConsolidateAsync(IEnumerable<BookModel> sourceData) 
            => Task.FromResult(sourceData.ToArray());
    }
}
