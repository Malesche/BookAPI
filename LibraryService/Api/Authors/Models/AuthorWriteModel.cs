namespace LibraryService.Api.Authors.Models
{
    public class AuthorWriteModel
    {
        public string Name { get; set; }

        public string Biography { get; set; }

        public DateTimeOffset? BirthDate { get; set; }

        public DateTimeOffset? DeathDate { get; set; }

        public string SourceIds { get; set; }
    }
}
