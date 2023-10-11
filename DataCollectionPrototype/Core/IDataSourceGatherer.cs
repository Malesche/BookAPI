namespace DataCollectionPrototype.Core;

public interface IDataSourceGatherer
{
    /// <summary>
    /// Collections data of a source with books and translate it to a data model
    /// which can be consolidated. The amount of books is decided by the implementation
    /// of this interface and can be of course a subset.
    /// </summary>
    /// <returns></returns>
    Task<object[]> CollectAsync();
}