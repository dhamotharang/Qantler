using eHS.Portal.Infrastructure.Filters;
using eHS.Portal.Model;
using eHS.Portal.Models.Checklist;
using eHS.Portal.Services.Checklist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eHS.Portal.Controllers
{
  [Authorize]
  public class ChecklistController : Controller
  {
    readonly IChecklistService _checklistService;

    public ChecklistController(IChecklistService checklistService)
    {
      _checklistService = checklistService;
    }

    [Route("[controller]/scheme")]
    [ServiceFilter(typeof(SessionAwareFilter))]
    [PermissionFilter(Permission.SystemRead, Permission.SystemReadWrite)]
    public IActionResult Index()
    {
      return View();
    }

    [Route("[controller]/details")]
    [ServiceFilter(typeof(SessionAwareFilter))]
    [PermissionFilter(Permission.SystemRead, Permission.SystemReadWrite)]
    public async Task<IActionResult> Details(int lastVersion, int scheme, int historyID = 0, string userAction = "New")
    {
      var data = new DetailsModel
      {
        Checklist = await _checklistService.GetChecklistHistoryByID(historyID),
        Action = userAction,
        LastVersion = lastVersion,
        SchemeID = scheme,
        SchemeText = Enum.GetName(typeof(Scheme), scheme)
      };

      return View(data);
    }

    [Route("api/[controller]/scheme/{id}")]
    public async Task<IList<ChecklistHistory>> scheme(int id)
    {
      return await _checklistService.GetChecklistHistoryByScheme(id);
    }

    [Route("api/[controller]/")]
    public async Task<IList<ChecklistHistory>> GetChecklistByID(long[] ids)
    {
      var result = new List<ChecklistHistory>();

      foreach (var id in ids)
      {
        result.Add(await _checklistService.GetChecklistHistoryByID(id));
      }

      return result;
    }

    [HttpPost]
    [Route("/api/[controller]")]
    [PermissionFilter(Permission.SystemReadWrite)]
    public async Task<bool> Insert([FromBody] ChecklistHistory data)
    {
      var result = await _checklistService.InsertChecklist(data);
      return result;
    }

    [HttpPut]
    [Route("/api/[controller]")]
    [PermissionFilter(Permission.SystemReadWrite)]
    public async Task<bool> Update([FromBody] ChecklistHistory data)
    {
      var result = await _checklistService.UpdateChecklist(data);
      return result;
    }
  }
}
