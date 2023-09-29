using LibraryService.Api.Works;
using LibraryService.Api.Works.ViewModels;
using LibraryService.Persistence;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace LibraryService.Tests.Api.Works
{
    public class WorksControllerTests
    {
        [Fact]
        public void CreateWork_ReturnsNoContent()
        {
            var workService = Substitute.For<IWorkService>();
            var controller = new WorksController(workService);

            var result = controller.CreateWork(new WorkWriteViewModel { Title = "WorkTitle" });

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void CreateWork_CallsService()
        {
            var workService = Substitute.For<IWorkService>();
            var controller = new WorksController(workService);

            controller.CreateWork(new WorkWriteViewModel { Title = "WorkTitle" });

            workService.Received(1).Create("WorkTitle");
        }

        [Fact]
        public void GetAllWorks_ReturnsValidViewModels()
        {
            var workService = Substitute.For<IWorkService>();
            workService.GetAll().Returns(new List<Work>
            {
                new() { Id = 1, Title = "Title1" },
                new() { Id = 2, Title = "Title2" },
                new() { Id = 3, Title = "Title3" }
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
            Assert.Equal(2, viewModelList[1].Id);
            Assert.Equal("Title2", viewModelList[1].Title);
            Assert.Equal(3, viewModelList[2].Id);
            Assert.Equal("Title3", viewModelList[2].Title);
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
            workService.Get(5).Returns(new Work { Id = 5, Title = "Title" });
            var controller = new WorksController(workService);

            var result = controller.GetWorkById(5);

            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;
            var model = (WorkReadViewModel)okObjectResult.Value;
            Assert.IsAssignableFrom<WorkReadViewModel>(model);
            Assert.Equal(5, model.Id);
            Assert.Equal("Title", model.Title);
        }

        [Fact]
        public void GetWorkById_validId_CallsService()
        {
            var workService = Substitute.For<IWorkService>();
            workService.Get(5).Returns(new Work { Id = 5, Title = "Title" });
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

            controller.UpdateWork(5, new WorkWriteViewModel { Title = "NewTitle" });

            workService.Received(1).Update(5, "NewTitle");
        }

        [Fact]
        public void UpdateWork_validId_ReturnsNoContent()
        {
            var workService = Substitute.For<IWorkService>();
            workService.Exists(5).Returns(true);
            var controller = new WorksController(workService);

            var result = controller.UpdateWork(5, new WorkWriteViewModel { Title = "NewTitle" });

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void UpdateWork_invalidId_DoesNotCallServiceUpdate()
        {
            var workService = Substitute.For<IWorkService>();
            workService.Exists(5).Returns(false);
            var controller = new WorksController(workService);

            controller.UpdateWork(5, new WorkWriteViewModel { Title = "NewTitle" });

            workService.DidNotReceive().Update(Arg.Any<int>(), Arg.Any<string>());
        }

        [Fact]
        public void UpdateWork_invalidId_ReturnsNotFound()
        {
            var workService = Substitute.For<IWorkService>();
            workService.Exists(5).Returns(false);
            var controller = new WorksController(workService);

            var result = controller.UpdateWork(5, new WorkWriteViewModel { Title = "NewTitle" });

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
