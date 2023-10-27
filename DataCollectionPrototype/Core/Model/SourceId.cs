namespace DataCollectionPrototype.Core.Model;

public class SourceId
{
    /// <summary>
    /// Represents the type of the source like google books.
    /// </summary>
    public string SourceType { get; }

    /// <summary>
    /// Unique id of the entity in the source.
    /// </summary>
    public string Id { get; }

    public static SourceId Create(string type, string id)
        => new(type, id);

    public static string Serialize(IEnumerable<SourceId> ids)
    {
        return string.Join("|", ids.Select(id => $"{id.SourceType}={id.Id}"));
    }

    public static bool ContainsType(IEnumerable<SourceId> ids, string type)
    {
        return ids.Any(id => id.SourceType == type);
    }
        
    private SourceId(string type, string id)
    {
        SourceType = type;
        Id = id;
    }
}