namespace DataCollectionPrototype.Core.Model
{
    public class BookGenreModel
    {
        public BookModel Book { get; set; } = null!;

        public GenreModel Genre { get; set; } = null!;
    }
}
