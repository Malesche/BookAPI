using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollectionPrototype.SourceGathering.OpenLibrary.Model
{
    internal class OpenLibraryAuthor
    {
        public string fuller_name { get; set; }
        public AuthorType type { get; set; }
        public string name { get; set; }
        public Remote_Ids remote_ids { get; set; }
        public string[] alternate_names { get; set; }
        public int[] photos { get; set; }
        public string[] source_records { get; set; }
        public string personal_name { get; set; }
        public string key { get; set; }
        public string birth_date { get; set; }
        //public string bio { get; set; }
        public string death_date { get; set; }
        public string wikipedia { get; set; }
        public Link[] links { get; set; }
        public int latest_revision { get; set; }
        public int revision { get; set; }
        public AuthorCreated created { get; set; }
        public AuthorLast_Modified last_modified { get; set; }
    }

    public class AuthorType
    {
        public string key { get; set; }
    }

    public class Remote_Ids
    {
        public string isni { get; set; }
        public string wikidata { get; set; }
        public string viaf { get; set; }
    }

    public class AuthorCreated
    {
        public string type { get; set; }
        public DateTime value { get; set; }
    }

    public class AuthorLast_Modified
    {
        public string type { get; set; }
        public DateTime value { get; set; }
    }

    public class Link
    {
        public string url { get; set; }
        public string title { get; set; }
        public LinkType1 type { get; set; }
    }

    public class LinkType1
    {
        public string key { get; set; }
    }

}
