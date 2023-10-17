using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryService.Persistence
{
    public class Work
    {
        public int Id { get; set; }
        
        public string Title { get; set; }

        public DateTimeOffset? EarliestPubDate { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        public string SourceIds { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
