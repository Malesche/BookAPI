using LibraryService.Persistence;
using System.ComponentModel.DataAnnotations;

namespace LibraryService.Api.Books.ViewModels
{
    public class BookWriteViewModel
    {
        [Required]
        public string Title { get; set; }

        public BookFormat? Format { get; set; }
        
        public string Language { get; set; }

        public string Isbn { get; set; }

        public string Isbn13 { get; set; }

        public string Description { get; set; }

        public DateTimeOffset? PubDate { get; set; }

        public string CoverUrl { get; set; }

        public int? WorkId { get; set; }

        public ICollection<BookAuthorWriteViewModel> BookAuthors { get; set; }
    }
}
