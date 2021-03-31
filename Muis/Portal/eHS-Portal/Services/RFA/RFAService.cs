using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eHS.Portal.Client;

namespace eHS.Portal.Services.RFA
{
  public class RFAService : IRFAService
  {
    readonly ApiClient _apiClient;

    public RFAService(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    public async Task<Model.RFA> GetByID(long id)
    {
      var result = await _apiClient.RFASdk.GetRFAByID(id);

      result.Request = await _apiClient.RequestSdk.GetByIDBasic(result.RequestID);

      result.Request.Customer = await _apiClient.CustomerSdk.GetByID(result.Request.CustomerID);
      result.Request.Customer.Code = result.Request.CustomerCode;
      result.Request.Customer.GroupCode = result.Request.GroupCode;

      if (result.Request.AgentID != null)
      {
        result.Request.Agent = await _apiClient.PersonSdk.GetByID(result.Request.AgentID.Value);
        result.Request.Requestor = await _apiClient.PersonSdk.GetByID(result.Request.RequestorID);
      }
      else
      {
        result.Request.Requestor = await _apiClient.PersonSdk.GetByID(result.Request.RequestorID);
      }

      return result;
    }

    public async Task<Model.RFA> Submit(Model.RFA rfa)
    {
      return await _apiClient.RFASdk.Submit(rfa);
    }

    public async Task Discard(long id)
    {
      await _apiClient.RFASdk.Delete(id);
    }

    public async Task Close(long id)
    {
      await _apiClient.RFASdk.UpdateRFAStatus(id, Model.RFAStatus.Closed);
    }

    public async Task Extend(long id, string notes, DateTimeOffset toDate)
    {
      await _apiClient.RFASdk.ExtendRFADueDate(id, notes, toDate);
    }

    public Task<IList<Model.RFA>> ListOfRFA(RFAOptions options)
    {
      return _apiClient.RFASdk.List(options);
    }
  }
}
