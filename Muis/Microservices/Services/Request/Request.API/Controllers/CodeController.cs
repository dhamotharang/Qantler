using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Request.API.Services;
using Request.Model;

namespace Request.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CodeController : Controller
  {
    readonly ICodeService _codeService;

    public CodeController(ICodeService codeService)
    {
      _codeService = codeService;
    }

    [HttpGet]
    [Route("list")]
    public async Task<IList<Code>> List(CodeType type)
    {
      return await _codeService.List(type);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<Code> GetByID(long id)
    {
      return await _codeService.GetByID(id);
    }

    [HttpPost]
    public async Task<Code> Create([FromBody] Code code)
    {
      return await _codeService.Create(code);
    }

    [HttpGet]
    [Route("generate")]
    public async Task<string> Generate(CodeType type)
    {
      return await _codeService.GenerateCode(type);
    }
  }
}
