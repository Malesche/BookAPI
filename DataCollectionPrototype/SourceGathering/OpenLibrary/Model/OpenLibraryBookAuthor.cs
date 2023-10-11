using System.Text.Json.Serialization;

namespace DataCollectionPrototype.SourceGathering.OpenLibrary.Model
{
    internal class OpenLibraryBookAuthor
    {
        [property: JsonPropertyName("key")]
        public string[] AuthorKey { get; set; }
    }
}
