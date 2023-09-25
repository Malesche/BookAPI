namespace LibraryService.Api.Books.Models
{
    public record BookWriteModel
    {
        public string Title { get; set; }

        public string Format { get; set; }

        public string Language { get; set; }

        public int? WorkId { get; set; }

        public List<BookAuthorWriteModel> BookAuthors { get; set; }
    }
}

