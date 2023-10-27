namespace LibraryService.Api.Works.ViewModels;

public class WorkReadViewModel
{
    public int Id { get; set; }

    public string Title { get; set; }

    public DateTimeOffset? EarliestPubDate { get; set; }

    public string SourceIds { get; set; }
}