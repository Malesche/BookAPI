using Microsoft.EntityFrameworkCore;

namespace LibraryService.Persistence
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> dbOptions):
            base(dbOptions)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Work> Works { get; set; }
        public DbSet<BookAuthor> BookAuthor { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<BookGenre> BookGenre { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasMany(e => e.Authors)
                .WithMany(e => e.Books)
                .UsingEntity<BookAuthor>();

            modelBuilder.Entity<Author>()
                .HasMany(e => e.Books)
                .WithMany(e => e.Authors)
                .UsingEntity<BookAuthor>();

            modelBuilder.Entity<BookAuthor>()
                .HasIndex(e => new { e.BookId, e.AuthorId, e.AuthorRole })
                .IsUnique()
                .HasFilter(null);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.Genres)
                .WithMany(e => e.Books)
                .UsingEntity<BookGenre>();

            modelBuilder.Entity<Genre>()
                .HasMany(e => e.Books)
                .WithMany(e => e.Genres)
                .UsingEntity<BookGenre>();
        }
    }
}