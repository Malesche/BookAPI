using LibraryService.Persistence;

namespace LibraryService.Api.Books.ViewModels;

public class BookAuthorInfoReadViewModel
{
    public int AuthorId { get; set; }

    public AuthorRole? AuthorRole { get; set; }

    public string Name { get; set; }

    public string Biography { get; set; }

    public DateTimeOffset? BirthDate { get; set; }

    public DateTimeOffset? DeathDate { get; set; }
}