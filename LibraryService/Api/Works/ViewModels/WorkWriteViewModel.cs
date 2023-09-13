using System.ComponentModel.DataAnnotations;

namespace LibraryService.Api.Works.ViewModels
{
    public class WorkWriteViewModel
    {
        [Required]
        public string Title { get; set; }
    }
}
