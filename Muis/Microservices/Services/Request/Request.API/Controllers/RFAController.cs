using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Request.API.Repository;
using Request.API.Services;
using Request.Model;


namespace Request.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class RFAController : ControllerBase
  {
    readonly IRFAService _rfaservice;

    public RFAController(IRFAService rfaService)
    {
      _rfaservice = rfaService;
    }

    [HttpGet]
    [Route("list")]
    public async Task<IList<RFA>> List(long? id = null,
      long? requestID = null,
      Guid? raisedBy = null,
      string customer = null,
      DateTimeOffset? createdOn = null,
      DateTimeOffset? dueOn = null,
      [FromQuery] IList<RFAStatus> status = null)
    {
      return await _rfaservice.ListOfRFA(new RFAFilter
      {
        ID = id,
        RequestID = requestID,
        Customer = customer,
        Status = status,
        RaisedBy = raisedBy,
        CreatedOn = createdOn,
        DueOn = dueOn
      });
    }

    [HttpPost]
    [Route("")]
    public async Task<RFA> Post([FromBody] RFA rfa)
    {
      return await _rfaservice.SubmitRFA(rfa);
    }

    [HttpPost]
    [Route("response")]
    public async Task<RFA> SubmitRFAResponse([FromBody] RFA rfa)
    {
      return await _rfaservice.SubmitRFAResponse(rfa);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<RFA> Get(long id)
    {
      return await _rfaservice.GetRFAByID(id);
    }

    [HttpPut]
    [Route("{id}/status")]
    public async Task<string> Put(long id, RFAStatus status, Guid userID, string userName)
    {
      await _rfaservice.UpdateRFAStatus(id, status, userID, userName);
      return "success";
    }

    [HttpPost]
    [Route("{id}/extend")]
    public async Task<string> Extend(long id, DateTimeOffset toDate,
      Guid userID, string userName, string notes = null)
    {
      await _rfaservice.ExtendDueDate(id, notes, toDate, userID, userName);
      return "success";
    }

    [HttpDelete("{id}")]
    public async Task<string> Delete(long id)
    {
      await _rfaservice.Delete(id);
      return "success";
    }
  }
}