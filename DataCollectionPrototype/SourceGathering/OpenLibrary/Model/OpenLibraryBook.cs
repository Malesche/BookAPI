using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollectionPrototype.SourceGathering.OpenLibrary.Model
{
    public class OpenLibraryBook
    {
        public string title { get; set; }
        public string key { get; set; }
        public Author[] authors { get; set; }
        public string[] contributions { get; set; }


        public string[] publishers { get; set; }
        public string subtitle { get; set; }
        public string weight { get; set; }
        public int[] covers { get; set; }
        public string edition_name { get; set; }
        public string physical_format { get; set; }
        public string[] subjects { get; set; }
        public string[] isbn_13 { get; set; }
        public string[] source_records { get; set; }
        public Identifiers identifiers { get; set; }
        public Language[] languages { get; set; }
        public string[] isbn_10 { get; set; }
        public string publish_date { get; set; }
        public Work[] works { get; set; }
        public Type type { get; set; }
        public string physical_dimensions { get; set; }
        public int latest_revision { get; set; }
        public int revision { get; set; }
        public Created created { get; set; }
        public Last_Modified last_modified { get; set; }
    }

    public class Identifiers
    {
        public string[] goodreads { get; set; }
        public string[] librarything { get; set; }
    }

    public class Type
    {
        public string key { get; set; }
    }

    public class Created
    {
        public string type { get; set; }
        public DateTime value { get; set; }
    }

    public class Last_Modified
    {
        public string type { get; set; }
        public DateTime value { get; set; }
    }

    public class Author
    {
        public string key { get; set; }
    }

    public class Language
    {
        public string key { get; set; }
    }

    public class Work
    {
        public string key { get; set; }
    }




    //internal class OpenLibraryBook
    //{
    //    public int OpenLibraryBookId { get; set; }

    //    public string Title { get; set; }

    //    public string Format { get; set; }

    //    public string Language { get; set; }

    //    public string Isbn { get; set; }

    //    public string Isbn13 { get; set; }

    //    public string Description { get; set; }

    //    public DateTimeOffset? PubDate { get; set; }

    //    public string OpenLibraryCoverId { get; set; }

    //    public ICollection<OpenLibraryBookAuthor> BookAuthors { get; set; }
    //}
}
