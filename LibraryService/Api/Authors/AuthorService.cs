using LibraryService.Persistence;

namespace LibraryService.Api.Authors
{
    public interface IAuthorService
    {
        void Create(string name);

        void Update(int id, string name);

        IEnumerable<Author> GetAll();

        Author Get(int id);

        bool Exists(int id);
    }

    public class AuthorService : IAuthorService
    {
        private readonly LibraryDbContext _dbContext;
    
        public AuthorService(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(string name)
        {
            var author = new Author
            {
                Name = name
            };

            _dbContext.Authors.Add(author);
            _dbContext.SaveChanges();
        }

        public void Update(int id, string name)
        {
            var author = _dbContext.Authors.First(a => a.Id == id);
            author.Name = name;

            _dbContext.SaveChanges();
        }

        public IEnumerable<Author> GetAll()
        {
            return _dbContext
                .Authors
                .ToArray();
        }

        public Author Get(int id)
        {
            return _dbContext
                .Authors
                .FirstOrDefault(a => a.Id == id);
        }

        public bool Exists(int id)
        {
            return _dbContext
                .Authors
                .Any(a => a.Id == id);
        }
    }
}
