﻿using LibraryService.Api.Works;
using LibraryService.Api.Works.Models;
using LibraryService.Persistence;

namespace LibraryService.Tests.Api.Works;

public class WorkServiceTests : TestWithSqliteBase
{
    [Fact]
    public void Create_ReturnsWorkReadViewModel()
    {
        var service = new WorkService(DbContext);
        var dateDec1871 = new DateTimeOffset(1871, 12, 13, 7, 0, 0, TimeSpan.FromHours(-7));

        var model = service.Create(new WorkWriteModel { 
            Title = "WorkTitle", 
            EarliestPubDate = dateDec1871, 
            SourceIds = "s" });

        Assert.Equal("WorkTitle", model.Title);
        Assert.Equal(new DateTimeOffset(1871, 12, 13, 7, 0, 0, TimeSpan.FromHours(-7)), model.EarliestPubDate);
        Assert.Equal("s", model.SourceIds);
    }

    [Fact]
    public void Create_SavesToDb()
    {
        var service = new WorkService(DbContext);
        var dateDec1871 = new DateTimeOffset(1871, 12, 13, 7, 0, 0, TimeSpan.FromHours(-7));

        service.Create(new WorkWriteModel {
            Title = "WorkTitle",
            EarliestPubDate = dateDec1871,
            SourceIds = "s" });

        var work = DbContext.Works.Single();
        Assert.Equal("WorkTitle", work.Title);
        Assert.Equal(new DateTimeOffset(1871, 12, 13, 7, 0, 0, TimeSpan.FromHours(-7)), work.EarliestPubDate);
        Assert.Equal("s", work.SourceIds);
    }

    [Fact]
    public void Update_SavesToDb()
    {
        var dateDec1871 = new DateTimeOffset(1871, 12, 13, 7, 0, 0, TimeSpan.FromHours(-7));            
        var dateDec1872 = new DateTimeOffset(1872, 12, 13, 7, 0, 0, TimeSpan.FromHours(-7));
        var workId = CreateWork("TitleBeforeUpdate", dateDec1871,"s");
        var service = new WorkService(DbContext);

        service.Update(
            workId, 
            new WorkWriteModel {
                Title = "TitleForUpdate",
                EarliestPubDate = dateDec1872,
                SourceIds = "updated" });

        var work = DbContext.Works.Single();
        Assert.Equal("TitleForUpdate", work.Title);
        Assert.Equal(dateDec1872, work.EarliestPubDate);
        Assert.Equal("updated", work.SourceIds);
    }

    [Fact]
    public void GetAll_ReturnsAllWorks()
    {
        var dateDec1871 = new DateTimeOffset(1871, 12, 13, 7, 0, 0, TimeSpan.FromHours(-7));
        var dateDec1872 = new DateTimeOffset(1872, 12, 14, 7, 0, 0, TimeSpan.FromHours(-7));
        var dateDec1873 = new DateTimeOffset(1873, 12, 15, 7, 0, 0, TimeSpan.FromHours(-7));
        var workId1 = CreateWork("Title1", dateDec1871, "s1");
        var workId2 = CreateWork("Title2", dateDec1872, "s2");
        var workId3 = CreateWork("Title3", dateDec1873, "s3");
        var service = new WorkService(DbContext);

        var getAllResult = service.GetAll();
            
        Assert.IsAssignableFrom<IEnumerable<Work>>(getAllResult);
        var works = getAllResult as Work[] ?? getAllResult.ToArray();
        var work1 = works.First(w => w.Id == workId1);
        var work2 = works.First(w => w.Id == workId2);
        var work3 = works.First(w => w.Id == workId3);
        Assert.Equal("Title1", work1.Title);
        Assert.Equal("Title2", work2.Title);
        Assert.Equal("Title3", work3.Title);
        Assert.Equal(dateDec1871, work1.EarliestPubDate);
        Assert.Equal(dateDec1872, work2.EarliestPubDate);
        Assert.Equal(dateDec1873, work3.EarliestPubDate);
        Assert.Equal("s1", work1.SourceIds);
        Assert.Equal("s2", work2.SourceIds);
        Assert.Equal("s3", work3.SourceIds);
    }

    [Fact]
    public void Get_validId_ReturnsWork()
    {
        var dateDec1871 = new DateTimeOffset(1871, 12, 13, 7, 0, 0, TimeSpan.FromHours(-7));
        var workId = CreateWork("WorkTitle", dateDec1871, "s");
        var service = new WorkService(DbContext);

        var work = service.Get(workId);

        Assert.Equal("WorkTitle", work.Title);
        Assert.Equal(dateDec1871, work.EarliestPubDate);
        Assert.Equal("s", work.SourceIds);
    }

    [Fact]
    public void Get_invalidId_ReturnsNull()
    {
        var service = new WorkService(DbContext);

        var work = service.Get(1);

        Assert.Null(work);
    }

    [Fact]
    public void Exists_validId_ReturnsTrue()
    {
        var dateDec1871 = new DateTimeOffset(1871, 12, 13, 7, 0, 0, TimeSpan.FromHours(-7));
        var workId = CreateWork("WorkTitle", dateDec1871, "s");
        var service = new WorkService(DbContext);

        var exists = service.Exists(workId);

        Assert.True(exists);
    }

    [Fact]
    public void Exists_invalidId_ReturnsFalse()
    {
        var service = new WorkService(DbContext);

        var exists = service.Exists(1);

        Assert.False(exists);
    }
        
    private int CreateWork(string title, DateTimeOffset pubDate, string sourceIds)
    {
        using var dbContext = CreateDbContext();
        var work = new Work() { Title = title, EarliestPubDate = pubDate, SourceIds = sourceIds};
        dbContext.Works.Add(work);
        dbContext.SaveChanges();

        return work.Id;
    }
}