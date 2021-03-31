using eHS.Portal.Client;
using eHS.Portal.Infrastructure.Filters;
using eHS.Portal.Model;
using eHS.Portal.Models.HalalLibrary;
using eHS.Portal.Services.HalalLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eHS.Portal.Controllers
{
  [Authorize]
  [ServiceFilter(typeof(SessionAwareFilter))]
  [PermissionFilter(Permission.HalalLibraryRead,
    Permission.HalalLibraryReadWrite)]
  public class HalalLibraryController : Controller
  {
    readonly IHalalLibraryService _halalLibraryService;

    public HalalLibraryController(IHalalLibraryService halalLibraryService)
    {
      _halalLibraryService = halalLibraryService;
    }

    [HttpGet]
    [Route("[controller]")]
    public IActionResult Index()
    {
      return View();
    }

    [HttpGet]
    [Route("api/halallibrary")]
    public async Task<IndexModel> Get(int offsetRows = 0,
      int nextRows = 20,
      string name = null,
      string brand = null,
      string supplier = null,
      string certifyingBody = null,
      RiskCategory? riskCategory = null,
      HLIngredientStatus? status = null,
      string verifiedBy = null)
    {
      var option = new HalalLibraryOptions
      {
        Name = name,
        Brand = brand,
        Supplier = supplier,
        CertifyingBody = certifyingBody,
        RiskCategory = riskCategory,
        Status = status,
        VerifiedBy = verifiedBy
      };
      return await _halalLibraryService.Search(option, offsetRows, nextRows);
    }

    [HttpPost]
    [Route("api/halallibrary")]
    [PermissionFilter(Permission.HalalLibraryReadWrite)]
    public async Task<long> Post([FromBody] HLIngredient data)
    {
      return await _halalLibraryService.InsertHalalLibrary(data);
    }

    [HttpPut]
    [Route("api/halallibrary")]
    [PermissionFilter(Permission.HalalLibraryReadWrite)]
    public async Task<long> Put([FromBody] HLIngredient data)
    {
      return await _halalLibraryService.UpdateHalalLibrary(data);
    }

    [HttpDelete]
    [Route("api/halallibrary/{id}")]
    [PermissionFilter(Permission.HalalLibraryReadWrite)]
    public async Task<bool> DeleteRequest(long id)
    {
      var result = await _halalLibraryService.DeleteHalalLibrary(id);
      return result;
    }

    [HttpGet]
    [Route("api/supplier")]
    public async Task<IEnumerable<Supplier>> GetSupplier()
    {
      return await _halalLibraryService.GetSupplier();
    }

    [HttpPost]
    [Route("api/supplier")]
    public async Task<long> PostSupplier([FromBody] Supplier data)
    {
      return await _halalLibraryService.InsertSupplier(data);
    }

    [HttpGet]
    [Route("api/certifyingbody")]
    public async Task<IEnumerable<CertifyingBody>> GetCertifyingBody()
    {
      return await _halalLibraryService.GetCertifyingBody();
    }

    [HttpPost]
    [Route("api/certifyingbody")]
    [PermissionFilter(Permission.HalalLibraryReadWrite)]
    public async Task<long> PostCertifyingBody([FromBody] CertifyingBody data)
    {
      return await _halalLibraryService.InsertCertifyingBody(data);
    }
  }
}
