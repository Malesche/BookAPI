using LibraryService.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace LibraryService.Tests
{
    public abstract class TestWithSqliteBase : IDisposable
    {
        private readonly SqliteConnection _connection;

        protected readonly LibraryDbContext DbContext;

        protected TestWithSqliteBase()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            DbContext = CreateDbContext();
            DbContext.Database.EnsureCreated();
        }

        protected LibraryDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseSqlite(_connection)
                .Options;

            return new LibraryDbContext(options);
        }

        public void Dispose()
        {
            DbContext.Dispose();
            _connection.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
