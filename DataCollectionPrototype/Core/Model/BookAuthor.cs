namespace DataCollectionPrototype.Core.Model
{
    public class BookAuthor
    {
        public AuthorRole? AuthorRole { get; set; }

        public BookModel Book { get; set; } = null!;

        public AuthorModel Author { get; set; } = null!;
    }
}
