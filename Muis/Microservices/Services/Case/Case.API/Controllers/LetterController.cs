using Case.API.Service;
using Case.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Case.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class LetterController : ControllerBase
  {
    readonly ILetterService _letterService;

    public LetterController(ILetterService letterService)
    {
      _letterService = letterService;
    }

    [HttpGet("template/{type}")]
    public async Task<LetterTemplate> GetTemplate(LetterType type)
    {
      return await _letterService.GetTemplate(type);
    }

    [HttpPut("template")]
    public async Task<string> UpdateTemplate(LetterTemplate letterTemplate, Guid id,
      string userName)
    {
      await _letterService.UpdateTemplate(letterTemplate, id, userName);

      return "Ok";
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<Letter> GetLetterByID(long id)
    {
      return await _letterService.GetLetterByID(id);
    }
  }
}
