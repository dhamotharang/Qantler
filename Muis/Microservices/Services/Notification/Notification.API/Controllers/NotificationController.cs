using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Notification.API.DTO;
using Notification.API.Repository;
using Notification.API.Services;
using Notification.Model;

namespace Notification.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class NotificationController : ControllerBase
  {
    readonly INotificationService _notifservice;

    public NotificationController(INotificationService notifService)
    {
      _notifservice = notifService;
    }

    [HttpGet]
    [Route("list")]
    public async Task<IEnumerable<Model.Notification>> Get(Guid? userID = null,
      DateTimeOffset? from = null, DateTimeOffset? lastModified = null)
    {
      return await _notifservice.Query(new NotificationFilter
      {
        UserID = userID,
        From = from,
        LastModified = lastModified
      });
    }

    [HttpPost]
    [Route("")]
    public async Task<Model.Notification> Post([FromBody] SendNotificationParam param)
    {
      return await _notifservice.Send(param.Notification, param.To);
    }

    [HttpPut]
    [Route("{id}/state")]
    public async Task<string> Put(long id, Guid userID, State state)
    {
      await _notifservice.UpdateState(id, userID, state);

      return "Ok";
    }

    [HttpDelete]
    [Route("clear")]
    public async Task<string> Delete(Guid userID)
    {
      await _notifservice.Clear(userID);

      return "Ok";
    }
  }
}