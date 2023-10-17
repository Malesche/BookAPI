using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryService.Persistence
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public BookFormat? Format { get; set; }

        public string Language { get; set; }
        
        public string Isbn { get; set; }
        
        public string Isbn13 { get; set; }
        
        public string Description { get; set; }
        
        public DateTimeOffset? PubDate { get; set; }

        public string Publisher { get; set; }

        public string CoverUrl { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        public string SourceIds { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; }

        public ICollection<Author> Authors { get; set; }

        public ICollection<BookGenre> BookGenres { get; set; }

        public ICollection<Genre> Genres { get; set; }

        public int? WorkId { get; set; }
        
        public Work Work { get; set; }
    }
}