﻿using LibraryService.Persistence;

namespace LibraryService.Api.Books.Models;

public record BookWriteModel
{
    public string Title { get; set; }

    public BookFormat? Format { get; set; }

    public string Language { get; set; }

    public string Isbn { get; set; }

    public string Isbn13 { get; set; }

    public string Description { get; set; }

    public DateTimeOffset? PubDate { get; set; }

    public string Publisher { get; set; }

    public string CoverUrl { get; set; }

    public string SourceIds { get; set; }

    public int? WorkId { get; set; }

    public List<BookAuthorWriteModel> BookAuthors { get; set; }
}