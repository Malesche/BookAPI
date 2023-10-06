using LibraryService.Persistence;

namespace LibraryService.Api.Works
{
    public interface IWorkService
    {
        public void Create(string title, DateTimeOffset? earliestPubDate);

        public void Update(int id, string title, DateTimeOffset? earliestPubDate);

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

        public void Create(string title, DateTimeOffset? earliestPubDate)
        {
            var work = new Work
            {
                Title = title,
                EarliestPubDate = earliestPubDate
            };

            _dbContext.Works.Add(work);
            _dbContext.SaveChanges();
        }

        public void Update(int id, string title, DateTimeOffset? earliestPubDate)
        {
            var work = _dbContext.Works.First(a => a.Id == id);
            work.Title = title;
            work.EarliestPubDate = earliestPubDate;

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
}
