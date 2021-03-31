using System;
using System.Threading.Tasks;
using Core.Model;
using Microsoft.AspNetCore.Mvc;
using Request.API.Services;
using Request.Model;

namespace Request.API.Controllers
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

    [HttpGet]
    [Route("{id}")]
    public async Task<Email> GetByID(long id)
    {
      return await _emailService.GetByID(id);
    }

    [HttpPost]
    public async Task<Email> Post([FromBody] Email email)
    {
      return await _emailService.Save(email);
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
