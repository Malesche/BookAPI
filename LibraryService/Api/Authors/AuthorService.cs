﻿using LibraryService.Api.Authors.Models;
using LibraryService.Api.Authors.ViewModels;
using LibraryService.Persistence;

namespace LibraryService.Api.Authors;

public interface IAuthorService
{
    AuthorReadViewModel Create(AuthorWriteModel model);

    public IEnumerable<AuthorReadViewModel> CreateSeveral(IEnumerable<AuthorWriteModel> models);

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

    public AuthorReadViewModel Create(AuthorWriteModel model)
    {
        var author = new Author
        {
            Name = model.Name,
            Biography = model.Biography,
            BirthDate = model.BirthDate,
            DeathDate = model.DeathDate,
            SourceIds = model.SourceIds
        };
        _dbContext.Authors.Add(author);
        _dbContext.SaveChanges();
        var authorReadViewModel = new AuthorReadViewModel
        {
            Id = author.Id,
            Name = author.Name,
            Biography = author.Biography,
            BirthDate = author.BirthDate,
            DeathDate = author.DeathDate,
            SourceIds = author.SourceIds
        };

        return authorReadViewModel;
    }

    public IEnumerable<AuthorReadViewModel> CreateSeveral(IEnumerable<AuthorWriteModel> models)
    {
        var authorReadViewModels = new List<AuthorReadViewModel>();
        var authors = new List<Author>();
        foreach (AuthorWriteModel model in models)
        {
            if (model is null)
                continue;
            var author = new Author
            {
                Name = model.Name,
                Biography = model.Biography,
                BirthDate = model.BirthDate,
                DeathDate = model.DeathDate,
                SourceIds = model.SourceIds
            };
            authors.Add(author);
            _dbContext.Authors.Add(author);
        }

        _dbContext.SaveChanges();

        foreach (Author author in authors)
        {
            var authorReadViewModel = new AuthorReadViewModel
            {
                Id = author.Id,
                Name = author.Name,
                Biography = author.Biography,
                BirthDate = author.BirthDate,
                DeathDate = author.DeathDate,
                SourceIds = author.SourceIds
            };
            authorReadViewModels.Add(authorReadViewModel);
        }

        return authorReadViewModels;
    }

    public void Update(int id, AuthorWriteModel model)
    {
        var author = _dbContext.Authors.First(a => a.Id == id);
        author.Name = model.Name;
        author.Biography = model.Biography;
        author.BirthDate = model.BirthDate;
        author.DeathDate = model.DeathDate;
        author.SourceIds = model.SourceIds;

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