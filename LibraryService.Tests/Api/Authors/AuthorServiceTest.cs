using LibraryService.Api.Authors;
using LibraryService.Api.Authors.Models;
using LibraryService.Api.Authors.ViewModels;
using LibraryService.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace LibraryService.Tests.Api.Authors;

public class AuthorServiceTest : TestWithSqliteBase
{
    [Fact]
    public void Create_SavesToDb()
    {
        var service = new AuthorService(DbContext);
        var authorWriteModel = new AuthorWriteModel { 
            Name = "George Eliot", 
            Biography = "She was very wonderful!", 
            BirthDate = new DateTimeOffset(1819, 11, 22, 7, 0, 0, TimeSpan.FromHours(-7)), 
            DeathDate = new DateTimeOffset(1880, 12, 22, 7, 0, 0, TimeSpan.FromHours(-7)),
            SourceIds = "OpenLibrary=uuuu/AAAA"
        };

        service.Create(authorWriteModel);

        var author = DbContext.Authors.Single();
        Assert.Equal("George Eliot", author.Name);
        Assert.Equal("She was very wonderful!", author.Biography);
        Assert.Equal(new DateTimeOffset(1819, 11, 22, 7, 0, 0, TimeSpan.FromHours(-7)), author.BirthDate);
        Assert.Equal(new DateTimeOffset(1880, 12, 22, 7, 0, 0, TimeSpan.FromHours(-7)), author.DeathDate);
        Assert.Equal("OpenLibrary=uuuu/AAAA", author.SourceIds);
    }

    [Fact]
    public void Create_ReturnsAuthorReadViewModel()
    {
        var service = new AuthorService(DbContext);
        var birthDate = new DateTimeOffset(1819, 11, 22, 7, 0, 0, TimeSpan.FromHours(-7));
        var deathDate = new DateTimeOffset(1880, 12, 22, 7, 0, 0, TimeSpan.FromHours(-7));
        var authorWriteModel = new AuthorWriteModel
        {
            Name = "George Eliot",
            Biography = "She was very wonderful!",
            BirthDate = birthDate,
            DeathDate = deathDate,
            SourceIds = "s2"
        };

        var model = service.Create(authorWriteModel);

        Assert.IsType<AuthorReadViewModel>(model);
        Assert.IsAssignableFrom<AuthorReadViewModel>(model);
        Assert.Equal("George Eliot", model.Name);
        Assert.Equal("She was very wonderful!", model.Biography);
        Assert.Equal(birthDate, model.BirthDate);
        Assert.Equal(deathDate, model.DeathDate);
        Assert.Equal("s2", model.SourceIds);
    }

    [Fact]
    public void Update_SavesToDb()
    {
        var authorId = CreateAuthor("Mary Ann Evans", null, null, null, "s2");
        var service = new AuthorService(DbContext);
        var birthDate = new DateTimeOffset(1819, 11, 22, 7, 0, 0, TimeSpan.FromHours(-7));
        var deathDate = new DateTimeOffset(1880, 12, 22, 7, 0, 0, TimeSpan.FromHours(-7));
        var authorWriteModel = new AuthorWriteModel
        {
            Name = "Mary Ann Evans Lewes",
            Biography = "She had a family.",
            BirthDate = birthDate,
            DeathDate = deathDate,
            SourceIds = "OpenLibrary=uuuu/AAAA"
        };

        service.Update(authorId, authorWriteModel);

        var author = DbContext.Authors.Single();           
        Assert.Equal("Mary Ann Evans Lewes", author.Name);
        Assert.Equal("She had a family.", author.Biography);
        Assert.Equal(new DateTimeOffset(1819, 11, 22, 7, 0, 0, TimeSpan.FromHours(-7)), author.BirthDate);
        Assert.Equal(new DateTimeOffset(1880, 12, 22, 7, 0, 0, TimeSpan.FromHours(-7)), author.DeathDate);
        Assert.Equal("OpenLibrary=uuuu/AAAA", author.SourceIds);
    }

    [Fact]
    public void GetAll_ReturnsAllAuthors()
    {
        var birthDate1 = new DateTimeOffset(1819, 11, 22, 7, 0, 0, TimeSpan.FromHours(-7));
        var deathDate1 = new DateTimeOffset(1880, 12, 22, 7, 0, 0, TimeSpan.FromHours(-7)); 
        var birthDate2 = new DateTimeOffset(819, 1, 2, 7, 0, 0, TimeSpan.FromHours(-7));
        var deathDate2 = new DateTimeOffset(880, 2, 2, 7, 0, 0, TimeSpan.FromHours(-7));
        var authorId1 = CreateAuthor("Name1", "bio1", birthDate1, deathDate1, "s1");
        var authorId2 = CreateAuthor("Name2", "bio2", birthDate2, deathDate2, "s2");
        var authorId3 = CreateAuthor("Name3", "bio3", birthDate1, deathDate1, "s3");
        AuthorService service = new AuthorService(DbContext);

        var getAllResult = service.GetAll();

        Assert.IsAssignableFrom<IEnumerable<Author>>(getAllResult);
        var authors = getAllResult as Author[] ?? getAllResult.ToArray();
        var author1 = authors.First(a => a.Id == authorId1);
        var author2 = authors.First(a => a.Id == authorId2);
        var author3 = authors.First(a => a.Id == authorId3);
        Assert.Equal("Name1", author1.Name);
        Assert.Equal("Name2", author2.Name);
        Assert.Equal("Name3", author3.Name);
        Assert.Equal("bio1", author1.Biography);
        Assert.Equal("bio2", author2.Biography);
        Assert.Equal("bio3", author3.Biography);
        Assert.Equal(birthDate1, author1.BirthDate);
        Assert.Equal(birthDate2, author2.BirthDate);
        Assert.Equal(birthDate1, author3.BirthDate);
        Assert.Equal(deathDate1, author1.DeathDate);
        Assert.Equal(deathDate2, author2.DeathDate);
        Assert.Equal(deathDate1, author3.DeathDate);
        Assert.Equal("s1", author1.SourceIds);
        Assert.Equal("s2", author2.SourceIds);
        Assert.Equal("s3", author3.SourceIds);
    }

    [Fact]
    public void Get_validId_ReturnsAuthor()
    {
        var birthDate = new DateTimeOffset(1819, 11, 22, 7, 0, 0, TimeSpan.FromHours(-7));
        var deathDate = new DateTimeOffset(1880, 12, 22, 7, 0, 0, TimeSpan.FromHours(-7));
        var authorId = CreateAuthor("AuthorName", "someBiography", birthDate, deathDate, "s1");
        AuthorService service = new AuthorService(DbContext);

        var author = service.Get(authorId);

        Assert.Equal("AuthorName", author.Name);
        Assert.Equal("someBiography", author.Biography);
        Assert.Equal(birthDate, author.BirthDate);
        Assert.Equal(deathDate, author.DeathDate);
        Assert.Equal("s1", author.SourceIds);
    }

    [Fact]
    public void Get_invalidId_ReturnsNull()
    {
        AuthorService service = new AuthorService(DbContext);

        var author = service.Get(1);

        Assert.Null(author);
    }

    [Fact]
    public void Exists_validId_ReturnsTrue()
    {
        var authorId = CreateAuthor("AuthorName", "someBiography", null, null, "sourceIds");
        AuthorService service = new AuthorService(DbContext);

        var exists = service.Exists(authorId);

        Assert.True(exists);
    }

    [Fact]
    public void Exists_invalidId_ReturnsFalse()
    {
        AuthorService service = new AuthorService(DbContext);

        var exists = service.Exists(1);

        Assert.False(exists);
    }

    private int CreateAuthor(string name, string bio, DateTimeOffset? birthDate, DateTimeOffset? deathDate, string sourceIds)
    {
        using var dbContext = CreateDbContext();
        var author = new Author()
        {
            Name = name,
            Biography = bio,
            BirthDate = birthDate,
            DeathDate = deathDate,
            SourceIds = sourceIds
        };
        dbContext.Authors.Add(author);
        dbContext.SaveChanges();

        return author.Id;
    }
}