using System.ComponentModel.DataAnnotations;

namespace LibraryService.Api.Authors.ViewModels
{
    public class AuthorWriteViewModel
    {
        [Required]
        public string Name { get; set; }

        public string Biography { get; set; }

        public DateTimeOffset? BirthDate { get; set; }

        public DateTimeOffset? DeathDate { get; set; }
        
        public string SourceIds { get; set; }
    }
}
