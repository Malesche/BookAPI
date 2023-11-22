using DataCollectionPrototype.Core;
using DataCollectionPrototype.Core.Model;
using LibraryService.Api.Authors.ViewModels;
using LibraryService.Api.Books.ViewModels;
using LibraryService.Api.Works.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DataCollectionPrototype.TargetWriting
{
    internal class LibraryApiListWriter : ITargetWriter
    {
        private const int BatchSize = 100;

        public async Task WriteAsync(BookModel[] bookModels)
        {
            var timer = new Stopwatch();
            timer.Start();
            
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7077");

            var authorModels = GetAuthorModelsFromBookModels(bookModels);
            var workModels = GetWorkModelsFromBookModels(bookModels);
            var idsForAuthors = new Dictionary<string, int>();
            var idsForWorks = new Dictionary<string, int>();

            int currentBatch = 0;
            while (currentBatch * BatchSize < authorModels.Count())
            {
                AuthorModel[] nextBatch = authorModels.Skip(currentBatch * BatchSize)
                    .Take(BatchSize).ToArray();
                await WriteBatchOfAuthorsAsync
                    (client, nextBatch, idsForAuthors);
                currentBatch++;
            }
            
            currentBatch = 0;
            while (currentBatch * BatchSize < workModels.Count())
            {
                WorkModel[] nextBatch = workModels.Skip(currentBatch * BatchSize)
                    .Take(BatchSize).ToArray();
                await WriteBatchOfWorksAsync(client, nextBatch, idsForWorks);
                currentBatch++;
            }
            
            currentBatch = 0;
            while (currentBatch * BatchSize < bookModels.Count())
            {
                BookModel[] nextBatch = bookModels.Skip(currentBatch * BatchSize)
                    .Take(BatchSize).ToArray();
                await WriteBatchOfBooksAsync(client, nextBatch, idsForAuthors, idsForWorks);
                currentBatch++;
            }

            timer.Stop();
            Console.WriteLine($"Time LibraryApiListWriter WriteAsync took (with batch size{BatchSize}): {timer.Elapsed}");
        }

        private IEnumerable<WorkModel> GetWorkModelsFromBookModels(BookModel[] bookModels)
        {
            var workModels = bookModels.Select(bookModel => bookModel.Work);

            return workModels;
        }

        private IEnumerable<AuthorModel> GetAuthorModelsFromBookModels(BookModel[] bookModels)
        {
            return bookModels.SelectMany(bookModel => bookModel.BookAuthors).Select(bookAuthor => bookAuthor.Author);
        }

        private async Task WriteBatchOfWorksAsync(
            HttpClient client, 
            WorkModel[] nextBatch, 
            Dictionary<string, int> idsForWorks)
        {
            var jsonWorksList = JsonContent.Create(nextBatch.Select(WriteViewModelFromWorkModel));
            var response = await client.PostAsync("/api/Works/CreateSeveral", jsonWorksList);
            var workReadViewModels = await response.Content.ReadAsAsync<List<WorkReadViewModel>>();

            foreach (var workModel in workReadViewModels)
            {
                //var sourceIdStrings = workModel.SourceIds.Split(',');
                //var openLibraryString = sourceIdStrings.Where(idString => idString.StartsWith("OpenLibrary=")).First();
                //var openLibraryId = openLibraryString.Substring(12);
                //var openLibraryIdii = openLibraryString.Substring(openLibraryString.IndexOf("=") + 1);
                //var openLibraryId = workModel.SourceIds.Substring(12);
                idsForWorks[workModel.SourceIds] = workModel.Id;
            }
        }

        private async Task WriteBatchOfAuthorsAsync(
            HttpClient client, 
            AuthorModel[] nextBatch,
            Dictionary<string, int> idsForAuthors)
        {
            var jsonAuthorList = JsonContent.Create(nextBatch.Select(WriteViewModelFromAuthorModel));
            var response = await client.PostAsync("/api/Authors/CreateSeveral", jsonAuthorList);
            var authorReadViewModels = await response.Content.ReadAsAsync<List<AuthorReadViewModel>>();

            foreach (var model in authorReadViewModels)
            {
                //var sourceIdStrings = workModel.SourceIds.Split(',');
                //var openLibraryString = sourceIdStrings.Where(idString => idString.StartsWith("OpenLibrary=")).First();
                //var openLibraryId = openLibraryString.Substring(12);
                //var openLibraryIdii = openLibraryString.Substring(openLibraryString.IndexOf("=") + 1);
                //var openLibraryId = model.SourceIds.Substring(12);
                idsForAuthors[model.SourceIds] = model.Id;
            }
        }

        private async Task WriteBatchOfBooksAsync(
            HttpClient client, 
            BookModel[] nextBatch,
            Dictionary<string, int> idsForAuthors,
            Dictionary<string, int> idsForWorks)
        {
            var jsonBookList = JsonContent.Create(nextBatch.Select(book => WriteViewModelFromBookModel(book, idsForAuthors, idsForWorks)).ToArray());
            await client.PostAsync("/api/Books/CreateSeveral", jsonBookList);
        }

        private static BookWriteViewModel WriteViewModelFromBookModel(
            BookModel bookModel, 
            Dictionary<string, int> idsForAuthors,
            Dictionary<string, int> idsForWorks)
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
                WorkId = bookModel.Work is null? null : idsForWorks[bookModel.Work.SourceIds],
                BookAuthors = BookAuthorWriteViewModelsFromBookAuthors(bookModel.BookAuthors, idsForAuthors)
            };
        }

        private static ICollection<BookAuthorWriteViewModel> BookAuthorWriteViewModelsFromBookAuthors(
            IEnumerable<BookAuthor> bookAuthors,
            Dictionary<string, int> idsForAuthors)
        {
            var writeViewModels = new List<BookAuthorWriteViewModel>();

            foreach (var bookAuthor in bookAuthors)
            {
                var viewModel = new BookAuthorWriteViewModel
                {
                    AuthorId = idsForAuthors[bookAuthor.Author.SourceIds],
                    AuthorRole = (LibraryService.Persistence.AuthorRole?)bookAuthor.AuthorRole
                };
                writeViewModels.Add(viewModel);
            }

            return writeViewModels;
        }

        private static AuthorWriteViewModel WriteViewModelFromAuthorModel(AuthorModel authorModel)
        {
            if (authorModel is null)
                return null;
            return new AuthorWriteViewModel
            {
                Name = authorModel.Name,
                Biography = authorModel.Biography,
                BirthDate = authorModel.BirthDate,
                DeathDate = authorModel.DeathDate,
                SourceIds = authorModel.SourceIds
            };
        }

        private static WorkWriteViewModel WriteViewModelFromWorkModel(WorkModel workModel)
        {
            if (workModel is null)
                return null;
            return new WorkWriteViewModel
            {
                Title = workModel.Title,
                EarliestPubDate = workModel.EarliestPubDate,
                SourceIds = workModel.SourceIds
            };
        }
    }
}