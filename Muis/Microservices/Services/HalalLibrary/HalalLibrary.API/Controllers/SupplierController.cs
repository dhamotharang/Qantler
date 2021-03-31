using HalalLibrary.API.Services;
using HalalLibrary.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HalalLibrary.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SupplierController : Controller
  {
    readonly ISupplierService _supplierservice;

    public SupplierController(ISupplierService supplierservice)
    {
      _supplierservice = supplierservice;
    }

    [HttpGet]
    [Route("select")]
    public async Task<IEnumerable<Supplier>> Select()
    {
      return await _supplierservice.Select();
    }

    [HttpPost]
    [Route("")]
    public async Task<long> Post([FromBody] Supplier data)
    {
      return await _supplierservice.InsertSupplier(data);
    }
  }
}
