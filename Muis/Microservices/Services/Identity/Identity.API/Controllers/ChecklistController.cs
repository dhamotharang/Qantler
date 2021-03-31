using Core.Model;
using Identity.API.Services;
using Microsoft.AspNetCore.Mvc;
using Request.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ChecklistController : ControllerBase
  {
    readonly IChecklistService _checklistService;

    public ChecklistController(IChecklistService checklistService)
    {
      _checklistService = checklistService;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ChecklistHistory> Get(long id)
    {
      return await _checklistService.GetChecklistHistoryByID(id);
    }

    [HttpGet]
    [Route("latest")]
    public async Task<ChecklistHistory> GetLatest (Scheme scheme)
    {
      return await _checklistService.GetLatest(scheme);
    }

    [HttpGet]
    [Route("scheme/{id}")]
    public async Task<IEnumerable<ChecklistHistory>> Get(int id)
    {
      return await _checklistService.GetChecklistHistoryByScheme(id);
    }

    [HttpPost]
    [Route("")]
    public async Task<bool> Post([FromBody] ChecklistHistory checklist)
    {
      return await _checklistService.InsertChecklist(checklist);
    }

    [HttpPut]
    [Route("")]
    public async Task<bool> Put([FromBody] ChecklistHistory checklist)
    {
      return await _checklistService.UpdateChecklist(checklist);
    }
  }
}