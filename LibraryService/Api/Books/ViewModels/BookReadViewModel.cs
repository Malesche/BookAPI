namespace LibraryService.Api.Books.ViewModels
{
    public class BookReadViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Format { get; set; }

        public string Language { get; set; }

        public string Isbn { get; set; }

        public string Isbn13 { get; set; }

        public string Description { get; set; }

        public DateTimeOffset? PubDate { get; set; }

        public string CoverUrl { get; set; }

        public int? WorkId { get; set; }

        public ICollection<BookAuthorReadViewModel> BookAuthors { get; set; }
    }
}
