using System.Text.Json.Serialization;

namespace DataCollectionPrototype.Models
{
    internal class OpenLibraryBookAuthor
    {
        [property: JsonPropertyName("key")]
        public string[] AuthorKey { get; set; }
    }
}
