﻿using LibraryService.Api.Works.ViewModels;
using LibraryService.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace LibraryService.Api.Works
{
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
                .Create(viewModel.Title, viewModel.EarliestPubDate, viewModel.SourceIds);
            return Ok(workReadViewModel);
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
            
            _workService.Update(id, viewModel.Title, viewModel.EarliestPubDate, viewModel.SourceIds);

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
    }
}
