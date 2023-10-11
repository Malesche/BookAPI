using DataCollectionPrototype.Core.Model;

namespace DataCollectionPrototype.Core;

public interface ITargetWriter
{
    /// <summary>
    /// Write the books to the target rest-api after the data has beeen consolidated.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    Task WriteAsync(BookModel[] data);
}