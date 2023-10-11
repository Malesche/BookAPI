namespace DataCollectionPrototype.Core.Model
{
    public class WorkModel
    {
        public string Title { get; set; }

        public DateTimeOffset? EarliestPubDate { get; set; }

        public List<BookModel> Books { get; set; } = new();
    }
}
