using System.ComponentModel.DataAnnotations;

namespace LibraryService.Api.Books.ViewModels
{
    public class BookWriteViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Format { get; set; }
        
        public string Language { get; set; }
        
        public int? WorkId { get; set; }

        public List<BAWriteViewModel> BookAuthors { get; set; }
    }
}
