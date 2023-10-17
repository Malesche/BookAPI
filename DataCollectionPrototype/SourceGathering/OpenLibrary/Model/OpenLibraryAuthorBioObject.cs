using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollectionPrototype.SourceGathering.OpenLibrary.Model
{
    internal class OpenLibraryAuthorBioObject
    {
        public string name { get; set; }
        public Bio bio { get; set; }
        public string birth_date { get; set; }
        public string death_date { get; set; }
        public string key { get; set; }

    //    public string title { get; set; }
    //    public Remote_Ids2 remote_ids { get; set; }
    //    public string personal_name { get; set; }
    //    public string[] source_records { get; set; }
    //    public string[] alternate_names { get; set; }
    //    public int[] photos { get; set; }
    //    public Linkk[] links { get; set; }
    //    public Typee type { get; set; }
    //    public int latest_revision { get; set; }
    //    public int revision { get; set; }
    //    public Createdd created { get; set; }
    //    public Last_Modifiedd last_modified { get; set; }
    }

    public class Bio
    {
        public string type { get; set; }
        public string value { get; set; }
    }

    //public class Remote_Ids2
    //{
    //    public string viaf { get; set; }
    //    public string wikidata { get; set; }
    //    public string isni { get; set; }
    //}
    //public class Typee
    //{
    //    public string key { get; set; }
    //}
    //public class Createdd
    //{
    //    public string type { get; set; }
    //    public DateTime value { get; set; }
    //}
    //public class Last_Modifiedd
    //{
    //    public string type { get; set; }
    //    public DateTime value { get; set; }
    //}
    //public class Linkk
    //{
    //    public string url { get; set; }
    //    public Type1 type { get; set; }
    //    public string title { get; set; }
    //}
    //public class Type1
    //{
    //    public string key { get; set; }
    //}

}
