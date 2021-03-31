using eHS.Portal.Infrastructure.Filters;
using eHS.Portal.Model;
using eHS.Portal.Models.Master;
using eHS.Portal.Services.Master;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eHS.Portal.Controllers
{
  [Authorize]
  [ServiceFilter(typeof(SessionAwareFilter))]

  public class MasterController : Controller
  {
    readonly IMasterService _masterService;

    public MasterController(IMasterService masterService)
    {
      _masterService = masterService;
    }

    [HttpGet]
    [Route("[controller]/email/{type}")]
    [PermissionFilter(Permission.SystemRead, Permission.SystemReadWrite)]
    public async Task<IActionResult> EmailTemplate(EmailTemplateType type)
    {
      var model = await _masterService.GetEmailTemplate(type);
      return View(model);
    }

    [HttpGet]
    [Route("api/[controller]/email/{type}")]
    public async Task<EmailTemplate> EmailTemplateData(EmailTemplateType type)
    {
      return await _masterService.GetEmailTemplate(type);
    }

    [HttpPut]
    [Route("api/[controller]/email/{type}")]
    [PermissionFilter(Permission.SystemReadWrite)]
    public async Task<string> UpdateEmailTemplate(EmailTemplateType type,
      [FromBody] EmailTemplate template)
    {
      template.Type = type;
      await _masterService.UpdateEmailTemplate(template);
      return "Ok";
    }

    [HttpGet]
    [Route("[controller]/letter/{type}")]
    [PermissionFilter(Permission.SystemRead, Permission.SystemReadWrite)]
    public async Task<IActionResult> GetLetterTemplate(LetterType type)
    {
      var model = await _masterService.GetLetterTemplate(type);
      return View("LetterTemplate", model);
    }

    [HttpGet]
    [Route("api/[controller]/letter/{type}")]
    public async Task<LetterTemplate> GetLetterTemplateData(LetterType type)
    {
      return await _masterService.GetLetterTemplate(type);
    }

    [HttpPut]
    [Route("api/[controller]/letter/{type}")]
    [PermissionFilter(Permission.SystemReadWrite)]
    public async Task<string> UpdateLetterTemplate(LetterType type,
      [FromBody] LetterTemplate template)
    {
      template.Type = type;
      await _masterService.UpdateLetterTemplate(template);
      return "Ok";
    }

    [HttpGet]
    [Route("[controller]/{type}")]
    [PermissionFilter(Permission.SystemRead, Permission.SystemReadWrite)]
    public IActionResult Index(MasterType type)
    {
      return View(new IndexModel
      {
        Type = type
      });
    }

    [HttpGet]
    [Route("api/master/{type}")]
    public async Task<IList<Master>> GetRequest(MasterType type)
    {
      return await _masterService.GetMasterList(type);
    }

    [HttpPost]
    [Route("api/master")]
    [PermissionFilter(Permission.SystemReadWrite)]
    public async Task<bool> Post([FromBody] Master data)
    {
      return await _masterService.InsertMaster(data);
    }

    [HttpPut]
    [Route("api/master")]
    [PermissionFilter(Permission.SystemReadWrite)]
    public async Task<bool> Put([FromBody] Master data)
    {
      return await _masterService.UpdateMaster(data);
    }

    [HttpDelete]
    [Route("api/master/{id}/{type}")]
    [PermissionFilter(Permission.SystemReadWrite)]
    public async Task<bool> DeleteRequest(Guid id, MasterType type)
    {
      var result = await _masterService.DeleteMaster(id, type);
      return result;
    }
  }
}
