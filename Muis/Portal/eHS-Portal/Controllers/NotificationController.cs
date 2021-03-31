using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eHS.Portal.Client;
using eHS.Portal.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eHS.Portal.Controllers
{
  [Authorize]
  public class NotificationController : Controller
  {
    readonly ApiClient _apiClient;
       
    public NotificationController(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    [HttpGet]
    [Route("api/[controller]/list")]
    public async Task<IEnumerable<Notification>> Get(DateTimeOffset? from = null)
    {
      if (from == null)
      {
        from = DateTimeOffset.UtcNow.AddMonths(-1);
      }
      return await _apiClient.NotificationSdk.List(from);
    }

    [HttpPut]
    [Route("api/[controller]/{id}/state")]
    public async Task<string> UpdateState(long id, State state)
    {
      await _apiClient.NotificationSdk.UpdateState(id, state);

      return "Ok";
    }
  }
}
