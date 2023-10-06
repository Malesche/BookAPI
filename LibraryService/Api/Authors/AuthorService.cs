using LibraryService.Api.Authors.Models;
using LibraryService.Persistence;

namespace LibraryService.Api.Authors
{
    public interface IAuthorService
    {
        void Create(AuthorWriteModel model);

        void Update(int id, AuthorWriteModel model);

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

        public void Create(AuthorWriteModel model)
        {
            var author = new Author
            {
                Name = model.Name,
                Biography = model.Biography,
                BirthDate = model.BirthDate,
                DeathDate = model.DeathDate
            };

            _dbContext.Authors.Add(author);
            _dbContext.SaveChanges();
        }

        public void Update(int id, AuthorWriteModel model)
        {
            var author = _dbContext.Authors.First(a => a.Id == id);
            author.Name = model.Name;
            author.Biography = model.Biography;
            author.BirthDate = model.BirthDate;
            author.DeathDate = model.DeathDate;

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
