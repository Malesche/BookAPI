// ReSharper disable InconsistentNaming Because it is generated code.
//#pragma warning disable IDE1006 Because it is generated code.

namespace DataCollectionPrototype.SourceGathering.OpenLibrary.Model;

public class OpenLibraryBook
{
    public string title { get; set; }
    public string physical_format { get; set; }
    public Language[] languages { get; set; }
    public string[] isbn_10 { get; set; }
    public string[] isbn_13 { get; set; }
    public string publish_date { get; set; }
    public string[] publishers { get; set; }
    public int?[] covers { get; set; }
    public string key { get; set; } 
    public Author[] authors { get; set; }
    public string[] contributions { get; set; }
    public Work[] works { get; set; }
}

public class Type
{
    public string key { get; set; }
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