using System;
using Microsoft.AspNetCore.Mvc;

namespace Case.API.Controllers
{
  [ApiController]
  public class RootController : Controller
  {
    [Route("api")]
    public string Root()
    {
      return "EnforcementCase.API";
    }
  }
}
