using System.Text.Json.Serialization;

namespace DataCollectionPrototype.Models
{
    internal class OpenLibraryWorkAuthor
    {
        [property: JsonPropertyName("key")]
        public string[] AuthorKey { get; set; }
    }
}
