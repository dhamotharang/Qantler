using Core.Model;
using Identity.API.Services;
using Identity.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Identity.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class EmailController : ControllerBase
  {
    readonly IEmailService _emailService;

    public EmailController(IEmailService emailService)
    {
      _emailService = emailService;
    }

    [HttpGet("template/{type}")]
    public async Task<Email> GetTemplate(EmailTemplateType type)
    {
      return await _emailService.GetTemplate(type);
    }

    [HttpPut("template")]
    public async Task<string> UpdateTemplate(EmailTemplate emailTemplateDetails, Guid id,
      string userName)
    {
      await _emailService.UpdateTemplate(emailTemplateDetails, id, userName);

      return "Ok";
    }
  }
}
