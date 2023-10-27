namespace DataCollectionPrototype.Core.Model;

public class BookModel
{
    public string Title { get; set; }

    public BookFormat? Format { get; set; }

    public string Language { get; set; }
        
    public string Isbn { get; set; }
        
    public string Isbn13 { get; set; }
        
    public string Description { get; set; }
        
    public DateTimeOffset? PubDate { get; set; }

    public string Publisher { get; set; }

    public string CoverUrl { get; set; }

    public List<SourceId> SourceIds { get; set; } = new();

    public List<BookAuthor> BookAuthors { get; set; } = new();

    public List<AuthorModel> Authors { get; set; } = new();

    public List<BookGenreModel> BookGenres { get; set; } = new();

    public List<GenreModel> Genres { get; set; } = new();

    public WorkModel Work { get; set; }
}