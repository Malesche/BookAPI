using LibraryService.Api.Authors.Models;
using LibraryService.Api.Authors.ViewModels;
using LibraryService.Api.Works.Models;
using LibraryService.Api.Works.ViewModels;
using LibraryService.Persistence;

namespace LibraryService.Api.Works;

public interface IWorkService
{
    public WorkReadViewModel Create(WorkWriteModel model);

    public IEnumerable<WorkReadViewModel> CreateSeveral(IEnumerable<WorkWriteModel> models);

    public void Update(int id, WorkWriteModel model);

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

    public WorkReadViewModel Create(WorkWriteModel model)
    {
        var work = new Work
        {
            Title = model.Title,
            EarliestPubDate = model.EarliestPubDate,
            SourceIds = model.SourceIds
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

    public IEnumerable<WorkReadViewModel> CreateSeveral(IEnumerable<WorkWriteModel> models)
    {
        var workReadViewModels = new List<WorkReadViewModel>();
        var works = new List<Work>();
        foreach (WorkWriteModel model in models)
        {
            if (model is null)
                continue;
            var work = new Work
            {
                Title = model.Title,
                EarliestPubDate = model.EarliestPubDate,
                SourceIds = model.SourceIds
            };
            works.Add(work);
            _dbContext.Works.Add(work);
        }

        _dbContext.SaveChanges();

        foreach (Work work in works)
        {
            var workReadViewModel = new WorkReadViewModel
            {
                Id = work.Id,
                Title = work.Title,
                EarliestPubDate = work.EarliestPubDate,
                SourceIds = work.SourceIds
            };
            workReadViewModels.Add(workReadViewModel);
        }

        return workReadViewModels;
    }

    public void Update(int id, WorkWriteModel model)
    {
        var work = _dbContext.Works.First(a => a.Id == id);
        work.Title = model.Title;
        work.EarliestPubDate = model.EarliestPubDate;
        work.SourceIds = model.SourceIds;

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