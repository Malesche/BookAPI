using LibraryService.Api.Works.Models;
using LibraryService.Api.Works.ViewModels;
using LibraryService.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace LibraryService.Api.Works;

[Route("api/[controller]")]
[ApiController]
public class WorksController : ControllerBase
{
    private readonly IWorkService _workService;

    public WorksController(IWorkService workService)
    {
        _workService = workService;
    }

    [HttpPost]
    public IActionResult CreateWork([FromBody]WorkWriteViewModel viewModel)
    {
        var workReadViewModel = _workService
            .Create(WriteModelFromWriteViewModel(viewModel));
        return Ok(workReadViewModel);
    }

    [HttpPost]
    [Route("CreateSeveral")]
    public IActionResult CreateSeveralWorks([FromBody] List<WorkWriteViewModel> viewModels)
    {
        var workReadViewModels = _workService
            .CreateSeveral(viewModels.Select(WriteModelFromWriteViewModel));
        return Ok(workReadViewModels);
    }

    [HttpGet]
    public IActionResult GetAllWorks()
    {
        var allWorks = _workService
            .GetAll()
            .Select(ToViewModel)
            .ToArray();

        return Ok(allWorks);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetWorkById(int id)
    {
        var work = _workService.Get(id);
        if (work == null)
            return NotFound();

        var viewModel = ToViewModel(work);

        return Ok(viewModel);
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateWork(int id, [FromBody] WorkWriteViewModel viewModel)
    {
        if (!_workService.Exists(id))
            return NotFound($"The Work Id {id} does not exist!");
            
        _workService.Update(id, WriteModelFromWriteViewModel(viewModel));

        return NoContent();
    }

    private WorkReadViewModel ToViewModel(Work work)
    {
        return new WorkReadViewModel
        {
            Id = work.Id,
            Title = work.Title,
            EarliestPubDate = work.EarliestPubDate,
            SourceIds = work.SourceIds
        };
    }

    private static WorkWriteModel WriteModelFromWriteViewModel(WorkWriteViewModel viewModel)
    {
        return new WorkWriteModel()
        {
            Title = viewModel.Title,
            EarliestPubDate = viewModel.EarliestPubDate,
            SourceIds = viewModel.SourceIds
        };
    }
}