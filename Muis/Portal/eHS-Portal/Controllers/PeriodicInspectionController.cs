using System;
using eHS.Portal.Infrastructure.Filters;
using eHS.Portal.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eHS.Portal.Controllers
{
  [Authorize]
  [ServiceFilter(typeof(SessionAwareFilter))]
  [PermissionFilter(Permission.PeriodicInspectionReadWrite)]
  public class PeriodicInspectionController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
  }
}
