using LibraryService.Api.Works.ViewModels;
using LibraryService.Persistence;

namespace LibraryService.Api.Works;

public interface IWorkService
{
    public WorkReadViewModel Create(string title, DateTimeOffset? earliestPubDate, string sourceIds);

    public void Update(int id, string title, DateTimeOffset? earliestPubDate, string sourceIds);

    public IEnumerable<Work> GetAll();

    public Work Get(int id);

    public bool Exists(int id);
}

public class WorkService : IWorkService
{
    private readonly LibraryDbContext _dbContext;

    public WorkService(LibraryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public WorkReadViewModel Create(string title, DateTimeOffset? earliestPubDate, string sourceIds)
    {
        var work = new Work
        {
            Title = title,
            EarliestPubDate = earliestPubDate,
            SourceIds = sourceIds
        };

        _dbContext.Works.Add(work);
        _dbContext.SaveChanges();

        var workReadViewModel = new WorkReadViewModel
        {
            Id = work.Id,
            Title = work.Title,
            EarliestPubDate = work.EarliestPubDate,
            SourceIds = work.SourceIds
        };

        return workReadViewModel;
    }

    public void Update(int id, string title, DateTimeOffset? earliestPubDate, string sourceIds)
    {
        var work = _dbContext.Works.First(a => a.Id == id);
        work.Title = title;
        work.EarliestPubDate = earliestPubDate;
        work.SourceIds = sourceIds;

        _dbContext.SaveChanges();
    }

    public IEnumerable<Work> GetAll()
    {
        return _dbContext
            .Works
            .ToArray();
    }

    public Work Get(int id)
    {
        return _dbContext
            .Works
            .FirstOrDefault(a => a.Id == id);
    }

    public bool Exists(int id)
    {
        return _dbContext
            .Works
            .Any(a => a.Id == id);
    }
}