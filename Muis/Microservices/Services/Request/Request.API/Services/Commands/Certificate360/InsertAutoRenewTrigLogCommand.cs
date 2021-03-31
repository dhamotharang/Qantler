using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using System;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Certificate360
{
  public class InsertAutoRenewTrigLogCommand : IUnitOfWorkCommand<long>
  {
    readonly string _number;
    readonly DateTimeOffset? _expiresOn;

    public InsertAutoRenewTrigLogCommand(string number, DateTimeOffset? expiresOn)
    {
      _number = number;
      _expiresOn = expiresOn;
    }

    public async Task<long> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Certificate360.InsertAutoRenewalTriggerLog
        (_number, _expiresOn);
    }
  }
}
