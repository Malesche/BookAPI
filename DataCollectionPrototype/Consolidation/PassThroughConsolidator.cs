using DataCollectionPrototype.Core;

namespace DataCollectionPrototype.Consolidation
{
    internal class PassThroughConsolidator : IDataConsolidator
    {
        public Task<object[]> ConsolidateAsync(IEnumerable<object> sourceData) 
            => Task.FromResult(sourceData.ToArray());
    }
}
