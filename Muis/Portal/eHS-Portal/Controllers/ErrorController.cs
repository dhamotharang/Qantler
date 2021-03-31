using System;
using Microsoft.AspNetCore.Mvc;

namespace eHS.Portal.Controllers
{
  public class ErrorController : Controller
  {
    [Route("[controller]/{code:int}")]
    public IActionResult Index(int code, string message = null)
    {
      ViewData["message"] = message;
      return View($"{code}");
    }
  }
}
