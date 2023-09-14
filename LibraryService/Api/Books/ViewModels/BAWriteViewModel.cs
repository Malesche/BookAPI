using LibraryService.Persistence;
using System.ComponentModel.DataAnnotations;

namespace LibraryService.Api.Books.ViewModels
{
    public class BAWriteViewModel
    {
        [Required]
        public int AuthorId { get; set; }

        public AuthorRole? AuthorRole { get; set; } = 0;
    }
}
