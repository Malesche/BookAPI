
using LibraryService.Persistence;

namespace LibraryService.Api.Books.ViewModels
{
    public class BAWriteViewModel
    {
        public int AuthorId { get; set; }
        public AuthorRole? AuthorRole { get; set; }
    }
}
