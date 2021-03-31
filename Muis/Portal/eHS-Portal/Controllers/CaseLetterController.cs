using eHS.Portal.Model;
using eHS.Portal.Services.CaseLetter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eHS.Portal.Controllers
{
  [Authorize]
  public class CaseLetterController : Controller
  {
    readonly ICaseLetterService _caseLetterService;

    public CaseLetterController(ICaseLetterService caseLetterService)
    {
      _caseLetterService = caseLetterService;
    }

    [Route("/api/[controller]/{id}")]
    public async Task<Letter> GetLetterByID(long id)
    {
      return await _caseLetterService.GetLetterByID(id);
    }
  }
}
