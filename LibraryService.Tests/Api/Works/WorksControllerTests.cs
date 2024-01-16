using LibraryService.Api.Works;
using LibraryService.Api.Works.Models;
using LibraryService.Api.Works.ViewModels;
using LibraryService.Persistence;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace LibraryService.Tests.Api.Works;

public class WorksControllerTests
{
    [Fact]
    public void CreateWork_ReturnsValidViewModel()
    {
        var workService = Substitute.For<IWorkService>();
        var dateDec1871 = new DateTimeOffset(1871, 12, 13, 7, 0, 0, TimeSpan.FromHours(-7));
        workService.Create(Arg.Is<WorkWriteModel>(model =>
                model.Title == "Title1"
                && model.EarliestPubDate == dateDec1871
                && model.SourceIds == "sourceIds"))
            .Returns(new WorkReadViewModel { 
                Id = 1, 
                Title = "Title1", 
                EarliestPubDate = dateDec1871, 
                SourceIds = "sourceIds" });
        var controller = new WorksController(workService);

        var result = controller.CreateWork(new WorkWriteViewModel { 
            Title = "Title1", 
            EarliestPubDate = dateDec1871, 
            SourceIds = "sourceIds" });

        Assert.IsType<OkObjectResult>(result);
        var okObjectResult = (OkObjectResult)result;
        var model = (WorkReadViewModel)okObjectResult.Value;
        Assert.IsAssignableFrom<WorkReadViewModel>(model);
        Assert.Equal(1, model.Id);
        Assert.Equal("Title1", model.Title);
        Assert.Equal(dateDec1871, model.EarliestPubDate);
        Assert.Equal("sourceIds", model.SourceIds);
    }

    [Fact]
    public void CreateWork_CallsService()
    {
        var workService = Substitute.For<IWorkService>();
        var controller = new WorksController(workService);
        var dateDec1871 = new DateTimeOffset(1871, 12, 13, 7, 0, 0, TimeSpan.FromHours(-7));

        controller.CreateWork(new WorkWriteViewModel { Title = "WorkTitle", EarliestPubDate = dateDec1871, SourceIds = "s" });

        workService.Received(1).Create(Arg.Is<WorkWriteModel>(model =>
                model.Title == "WorkTitle"
                && model.EarliestPubDate == dateDec1871
                && model.SourceIds == "s"));
    }

    [Fact]
    public void GetAllWorks_ReturnsValidViewModels()
    {
        var workService = Substitute.For<IWorkService>();
        var dateDec1871 = new DateTimeOffset(1871, 12, 13, 7, 0, 0, TimeSpan.FromHours(-7));
        var dateDec1872 = new DateTimeOffset(1872, 12, 14, 7, 0, 0, TimeSpan.FromHours(-7));
        var dateDec1873 = new DateTimeOffset(1873, 12, 15, 7, 0, 0, TimeSpan.FromHours(-7));
        workService.GetAll().Returns(new List<Work>
        {
            new() { Id = 1, Title = "Title1", EarliestPubDate = dateDec1871},
            new() { Id = 2, Title = "Title2", EarliestPubDate = dateDec1872 },
            new() { Id = 3, Title = "Title3", EarliestPubDate = dateDec1873 }
        });
        var controller = new WorksController(workService);

        var result = controller.GetAllWorks();

        Assert.IsType<OkObjectResult>(result);
        var okObjectResult = (OkObjectResult)result;
        var model = okObjectResult.Value;
        Assert.IsAssignableFrom<IList<WorkReadViewModel>>(model);
        var viewModelList = (IList<WorkReadViewModel>)model;
        Assert.Equal(3, viewModelList.Count);
        Assert.Equal(1, viewModelList[0].Id);
        Assert.Equal("Title1", viewModelList[0].Title);
        Assert.Equal(dateDec1871, viewModelList[0].EarliestPubDate);
        Assert.Equal(2, viewModelList[1].Id);
        Assert.Equal("Title2", viewModelList[1].Title);
        Assert.Equal(dateDec1872, viewModelList[1].EarliestPubDate);
        Assert.Equal(3, viewModelList[2].Id);
        Assert.Equal("Title3", viewModelList[2].Title);
        Assert.Equal(dateDec1873, viewModelList[2].EarliestPubDate);
    }

    [Fact]
    public void GetAllWorks_CallsService()
    {
        var workService = Substitute.For<IWorkService>();
        var controller = new WorksController(workService);

        controller.GetAllWorks();

        workService.Received(1).GetAll();
    }

    [Fact]
    public void GetWorkById_validId_ReturnsValidViewModel()
    {
        var workService = Substitute.For<IWorkService>();
        var dateDec1871 = new DateTimeOffset(1871, 12, 13, 7, 0, 0, TimeSpan.FromHours(-7));
        workService.Get(5).Returns(new Work { Id = 5, Title = "Title", EarliestPubDate = dateDec1871});
        var controller = new WorksController(workService);

        var result = controller.GetWorkById(5);

        Assert.IsType<OkObjectResult>(result);
        var okObjectResult = (OkObjectResult)result;
        var model = (WorkReadViewModel)okObjectResult.Value;
        Assert.IsAssignableFrom<WorkReadViewModel>(model);
        Assert.Equal(5, model.Id);
        Assert.Equal("Title", model.Title);
        Assert.Equal(dateDec1871, model.EarliestPubDate);
    }

    [Fact]
    public void GetWorkById_validId_CallsService()
    {
        var workService = Substitute.For<IWorkService>();
        workService.Get(5).Returns(new Work { Id = 5, Title = "Title", EarliestPubDate = null });
        var controller = new WorksController(workService);

        controller.GetWorkById(5);

        workService.Received(1).Get(5);
    }

    [Fact]
    public void GetWorkById_invalidId_ReturnsNotFound()
    {
        var workService = Substitute.For<IWorkService>();
        workService.Get(5).Returns(_ => null);
        var controller = new WorksController(workService);

        var result = controller.GetWorkById(5);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void UpdateWork_validId_CallsService()
    {
        var workService = Substitute.For<IWorkService>();
        workService.Exists(5).Returns(true);
        var controller = new WorksController(workService);
        var dateDec1871 = new DateTimeOffset(1871, 12, 13, 7, 0, 0, TimeSpan.FromHours(-7));

        controller.UpdateWork(5, new WorkWriteViewModel { Title = "NewTitle", EarliestPubDate = dateDec1871, SourceIds = "s"});

        workService.Received(1).Update(
            5, 
            Arg.Is<WorkWriteModel>(model =>
                model.Title == "NewTitle"
                && model.EarliestPubDate == dateDec1871
                && model.SourceIds == "s"));
    }

    [Fact]
    public void UpdateWork_validId_ReturnsNoContent()
    {
        var workService = Substitute.For<IWorkService>();
        workService.Exists(5).Returns(true);
        var controller = new WorksController(workService);

        var result = controller.UpdateWork(5, new WorkWriteViewModel { Title = "NewTitle", EarliestPubDate = null });

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void UpdateWork_invalidId_DoesNotCallServiceUpdate()
    {
        var workService = Substitute.For<IWorkService>();
        workService.Exists(5).Returns(false);
        var controller = new WorksController(workService);

        controller.UpdateWork(5, new WorkWriteViewModel { Title = "NewTitle", EarliestPubDate = null });

        workService.DidNotReceive().Update(Arg.Any<int>(), Arg.Any<WorkWriteModel>());
    }

    [Fact]
    public void UpdateWork_invalidId_ReturnsNotFound()
    {
        var workService = Substitute.For<IWorkService>();
        workService.Exists(5).Returns(false);
        var controller = new WorksController(workService);

        var result = controller.UpdateWork(5, new WorkWriteViewModel { Title = "NewTitle", EarliestPubDate = null });

        Assert.IsType<NotFoundObjectResult>(result);
    }
}