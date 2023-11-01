using LibraryService.Api.Books.Models;

namespace LibraryService.Api.Works.Models
{
    public class WorkWriteModel
    {
        public string Title { get; set; }

        public DateTimeOffset? EarliestPubDate { get; set; }

        public string SourceIds { get; set; }

        public ICollection<BookWriteModel> Books { get; set; }
    }
}
