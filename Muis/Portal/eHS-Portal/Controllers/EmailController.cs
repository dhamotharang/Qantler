using System;
using System.Threading.Tasks;
using eHS.Portal.Model;
using eHS.Portal.Services.Emails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eHS.Portal.Controllers
{
  [Authorize]
  public class EmailController : Controller
  {
    readonly IEmailService _emailService;

    public EmailController(IEmailService emailService)
    {
      _emailService = emailService;
    }

    [HttpGet]
    [Route("api/[controller]/request/{id}")]
    public async Task<Email> GetRequestEmail(long id)
    {
      return await _emailService.GetRequestEmail(id);
    }
  }
}
