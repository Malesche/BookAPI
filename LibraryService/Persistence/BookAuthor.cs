namespace LibraryService.Persistence;

public class BookAuthor
{
    public int Id { get; set; }

    public int AuthorId { get; set; }

    public int BookId { get; set; }

    public AuthorRole? AuthorRole { get; set; }

    public Book Book { get; set; } = null!;

    public Author Author { get; set; } = null!;
}