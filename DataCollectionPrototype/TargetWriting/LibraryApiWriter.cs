using DataCollectionPrototype.Core;
using DataCollectionPrototype.Core.Model;
using System.Net.Http.Json;
using LibraryService.Api.Authors.ViewModels;
using LibraryService.Api.Books.ViewModels;
using LibraryService.Api.Works.ViewModels;
using System.Diagnostics;

namespace DataCollectionPrototype.TargetWriting;

internal class LibraryApiWriter : ITargetWriter
{
    public async Task WriteAsync(BookModel[] data)
    {
        var timer = new Stopwatch();
        timer.Start();

        using var client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:7077");

        foreach (var bookModel in data)
        {
            int? workId = null;
            if (bookModel.Work is not null)
            {
                var workPayload = JsonContent.Create(new WorkWriteViewModel
                {
                    Title = bookModel.Work.Title,
                    EarliestPubDate = bookModel.Work.EarliestPubDate,
                    SourceIds = bookModel.Work.SourceIds
                });
                var workResponse = await client.PostAsync("/api/Works", workPayload);
                var work = await workResponse.Content.ReadAsAsync<WorkReadViewModel>();
                workId = work.Id;
            }

            var bookAuthorList = new List<BookAuthorWriteViewModel>();
            foreach (var bookAuthor in bookModel.BookAuthors)
            {
                var authorPayload = JsonContent.Create(WriteModelFromAuthorModel(bookAuthor.Author));
                var authorResponse = await client.PostAsync("/api/Authors", authorPayload);
                var author = await authorResponse.Content.ReadAsAsync<AuthorReadViewModel>();
                var viewModel = new BookAuthorWriteViewModel
                {
                    AuthorId = author.Id, 
                    AuthorRole = (LibraryService.Persistence.AuthorRole?)bookAuthor.AuthorRole
                };
                bookAuthorList.Add(viewModel);
            }

            var jsonBookWriteModel = JsonContent.Create(WriteModelFromBookModel(bookModel, workId, bookAuthorList));
            await client.PostAsync("/api/Books", jsonBookWriteModel);
        }

        timer.Stop();
        Console.WriteLine($"Time WriteAsync took: {timer.Elapsed}");
    }

    private static BookWriteViewModel WriteModelFromBookModel(BookModel bookModel, int? workId, List<BookAuthorWriteViewModel> bookAuthorList)
    {
        return new BookWriteViewModel
        {
            Title = bookModel.Title,
            Format = (LibraryService.Persistence.BookFormat?)bookModel.Format,
            Language = bookModel.Language,
            Isbn = bookModel.Isbn,
            Isbn13 = bookModel.Isbn13,
            Description = bookModel.Description,
            PubDate = bookModel.PubDate,
            Publisher = bookModel.Publisher,
            CoverUrl = bookModel.CoverUrl,
            SourceIds = SourceId.Serialize(bookModel.SourceIds),
            WorkId = workId,
            BookAuthors = bookAuthorList
        };
    }

    private static AuthorWriteViewModel WriteModelFromAuthorModel(AuthorModel authorModel)
    {
        return new AuthorWriteViewModel
        {
            Name = authorModel.Name,
            Biography = authorModel.Biography,
            BirthDate = authorModel.BirthDate,
            DeathDate = authorModel.DeathDate,
            SourceIds = authorModel.SourceIds
        };
    }
}