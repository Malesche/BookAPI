using LibraryService.Persistence;

namespace LibraryService.Api.Works
{
    public class WorkService
    {
        private readonly LibraryDbContext _dbContext;

        public WorkService(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(string title)
        {
            var work = new Work
            {
                Title = title
            };

            _dbContext.Works.Add(work);
            _dbContext.SaveChanges();
        }

        public void Update(int id, string title)
        {
            var work = _dbContext.Works.First(a => a.Id == id);
            work.Title = title;

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
