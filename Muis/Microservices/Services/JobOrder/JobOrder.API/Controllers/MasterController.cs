using JobOrder.API.Services;
using JobOrder.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace JobOrder.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class MasterController : ControllerBase
  {
    readonly IMasterService _masterService;

    public MasterController(IMasterService masterService)
    {
      _masterService = masterService;
    }

    [HttpGet]
    [Route("")]
    public async Task<IEnumerable<Master>> Get(MasterType type)
    {
      return await _masterService.GetMaster(type);
    }

    [HttpPost]
    [Route("")]
    public async Task<bool> Post([FromBody] Master data)
    {
      await _masterService.InsertMaster(data);
      return true;
    }

    [HttpPut]
    [Route("")]
    public async Task<bool> Put([FromBody] Master data)
    {
      await _masterService.UpdateMaster(data);
      return true;
    }

    [HttpDelete]
    [Route("")]
    public async Task<bool> Delete(Guid ID)
    {
      await _masterService.DeleteMaster(ID);
      return true;
    }
  }
}