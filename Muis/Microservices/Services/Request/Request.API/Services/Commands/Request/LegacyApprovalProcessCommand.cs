using Core.API.Exceptions;
using Core.API.Smtp;
using Core.EventBus;
using Core.Model;
using Request.API.Helpers;
using Request.API.Models;
using Request.API.Repository;
using Request.API.Services.Commands.Request.Validators;
using Request.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Request
{
  public class LegacyApprovalProcessCommand
  {
    readonly Review _review;
    readonly Model.Request _requestBasic;

    readonly IEventBus _eventBus;

    readonly DbContext _dbContext;

    public LegacyApprovalProcessCommand(Model.Request requestBasic, Review review, IEventBus eventBus,
      DbContext dbContext)
    {
      _requestBasic = requestBasic;
      _review = review;

      _eventBus = eventBus;

      _dbContext = dbContext;
    }

    public async Task Invoke()
    {
      await new ApprovalPolicyValidator(_review, _dbContext)
        .Invoke();
    }
  }
}
