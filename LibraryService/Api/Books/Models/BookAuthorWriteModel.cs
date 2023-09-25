using LibraryService.Persistence;

namespace LibraryService.Api.Books.Models
{
    public class BookAuthorWriteModel
    {
        public int AuthorId { get; set; }

        public AuthorRole? AuthorRole { get; set; }
    }
}
