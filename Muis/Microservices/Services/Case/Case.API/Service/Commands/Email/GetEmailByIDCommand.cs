using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Core.Model;
using Case.API.Repository;

namespace Case.API.Services.Commands
{
  public class GetEmailByIDCommand : IUnitOfWorkCommand<Email>
  {
    readonly long _id;

    public GetEmailByIDCommand(long id)
    {
      _id = id;
    }

    public async Task<Email> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Email.GetByID(_id);
    }
  }
}
