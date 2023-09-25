namespace LibraryService.Api.Books.ViewModels
{
    public class BookReadViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Format { get; set; }

        public string Language { get; set; }

        public int? WorkId { get; set; }

        public ICollection<BookAuthorReadViewModel> BookAuthors { get; set; }
    }
}
