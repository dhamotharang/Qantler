using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eHS.Portal.Model;
using eHS.Portal.Services.Code;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eHS.Portal.Controllers
{
  [Authorize]
  public class CodeController : ControllerBase
  {
    readonly ICodeService _codeService;

    public CodeController(ICodeService codeService)
    {
      _codeService = codeService;
    }

    [HttpPost]
    [Route("api/[controller]")]
    public async Task<Code> Post([FromBody] Code code)
    {
      return await _codeService.Create(code);
    }

    [HttpGet]
    [Route("api/[controller]/generate")]
    public async Task<string> Generate(CodeType type)
    {
      return await _codeService.Generate(type);
    }

    [HttpGet]
    [Route("api/[controller]/list")]
    public async Task<IList<Code>> List(CodeType type)
    {
      return await _codeService.List(type);
    }
  }
}
