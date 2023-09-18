namespace LibraryService.Persistence
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Format { get; set; }

        public string Language { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; }

        public ICollection<Author> Authors { get; set; }

        public ICollection<BookGenre> BookGenres { get; set; }

        public ICollection<Genre> Genres { get; set; }

        public int? WorkId { get; set; }
        
        public Work Work { get; set; }
    }
}