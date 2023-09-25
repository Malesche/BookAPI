using LibraryService.Persistence;

namespace LibraryService.Api.Books.ViewModels
{
    public class BookAuthorReadViewModel
    {
        public int AuthorId { get; set; }

        public AuthorRole? AuthorRole { get; set; }
    }
}
