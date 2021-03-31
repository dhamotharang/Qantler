using Core.API;
using Request.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Request.API.Repository;
using Core.API.Repository;
using Core.Model;
using Core.EventBus;
using Request.Events;

namespace Request.API.Services.Commands.Request
{
  public class ValidateRequestCommand : IUnitOfWorkCommand<Model.Request>
  {
    readonly Scheme? _scheme;
    readonly SubScheme? _subScheme;
    readonly Premise _premise;  

    public ValidateRequestCommand(Scheme? scheme, SubScheme? subScheme, Premise premise)
    {
      _scheme = scheme;
      _subScheme = subScheme;
      _premise = premise;
    }

    public async Task<Model.Request> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();     
      var result = await dbContext.Request.ValidateRequest(_scheme, _subScheme, _premise);
      unitOfWork.Commit();
      return result;
    }
  }
}
