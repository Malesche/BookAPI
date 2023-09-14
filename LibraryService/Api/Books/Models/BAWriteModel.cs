using LibraryService.Persistence;

namespace LibraryService.Api.Books.Models
{
    public class BAWriteModel
    {
        public int AuthorId { get; set; }

        public AuthorRole? AuthorRole { get; set; }
    }
}
