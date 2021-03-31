using System.Collections.Generic;
using System.Threading.Tasks;
using HalalLibrary.API.Services;
using HalalLibrary.Model;
using Microsoft.AspNetCore.Mvc;

namespace HalalLibrary.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CertifyingBodyController : Controller
  {
    readonly ICertifyingBodyService _certifyingbodyservice;

    public CertifyingBodyController(ICertifyingBodyService certifyingbodyservice)
    {
      _certifyingbodyservice = certifyingbodyservice;
    }

    [HttpGet]
    [Route("select")]
    public async Task<IEnumerable<CertifyingBody>> Select()
    {
      return await _certifyingbodyservice.Select();
    }

    [HttpPost]
    [Route("")]
    public async Task<long> Post([FromBody] CertifyingBody data)
    {
      return await _certifyingbodyservice.InsertCertifyingBody(data);
    }
  }
}
