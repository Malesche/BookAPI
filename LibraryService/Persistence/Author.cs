using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryService.Persistence
{
    public class Author
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Biography { get; set; }

        public DateTimeOffset? BirthDate { get; set; }

        public DateTimeOffset? DeathDate { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        public string SourceIds { get; set; }

        public ICollection<BookAuthor> BookAuthor { get; set; }
        
        public ICollection<Book> Books { get; set; }
    }
}