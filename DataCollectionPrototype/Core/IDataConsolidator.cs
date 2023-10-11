using DataCollectionPrototype.Core.Model;

namespace DataCollectionPrototype.Core;

public interface IDataConsolidator
{
    /// <summary>
    /// Takes the complete list of all books of all sources and consolidates the data.
    /// </summary>
    /// <param name="sourceData"></param>
    /// <returns></returns>
    Task<BookModel[]> ConsolidateAsync(IEnumerable<BookModel> sourceData);
}