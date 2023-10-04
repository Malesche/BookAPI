namespace LibraryService.Persistence
{
    public class Work
    {
        public int Id { get; set; }
        
        public string Title { get; set; }

        public DateTimeOffset EarliestPubDate { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
