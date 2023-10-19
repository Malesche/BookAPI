namespace DataCollectionPrototype.Core.Model
{
    public class AuthorModel
    {
        public string Name { get; set; }

        public string Biography { get; set; }

        public DateTimeOffset? BirthDate { get; set; }

        public DateTimeOffset? DeathDate { get; set; }

        public string SourceIds { get; set; }
        
        public List<BookAuthor> BookAuthor { get; set; } = new();
        
        public List<BookModel> Books { get; set; } = new();
    }
}