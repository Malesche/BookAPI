namespace DataCollectionPrototype.Core.Model;

public class GenreModel
{
    public string Name { get; set; }

    public List<BookGenreModel> BookGenres { get; set; } = new();
}