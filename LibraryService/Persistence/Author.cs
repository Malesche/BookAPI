namespace LibraryService.Persistence
{
    public class Author
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public ICollection<BookAuthor> BookAuthor { get; set; }
        
        public ICollection<Book> Books { get; set; }
    }
}