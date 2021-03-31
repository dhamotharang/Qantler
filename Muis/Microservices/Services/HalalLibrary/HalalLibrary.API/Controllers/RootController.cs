using System;
using Microsoft.AspNetCore.Mvc;

namespace HalalLibrary.API.Controllers
{
  [ApiController]
  public class RootController : Controller
  {
    [Route("api")]
    public string Root()
    {
      return "eHS-HalalLibrary API";
    }
  }
}
