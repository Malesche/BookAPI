using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataCollectionPrototype
{
    public record class Repository(
        [property: JsonPropertyName("name")] string Name, 
        [property: JsonPropertyName("html_url")] string HtmlUrl);
}
