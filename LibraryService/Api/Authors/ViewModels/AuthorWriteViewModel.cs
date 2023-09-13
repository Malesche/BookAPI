using System.ComponentModel.DataAnnotations;

namespace LibraryService.Api.Authors.ViewModels
{
    public class AuthorWriteViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
