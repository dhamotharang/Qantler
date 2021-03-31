using Core.API;
using Core.API.Exceptions;
using Core.API.Repository;
using Core.Model;
using HalalLibrary.API.Repository;
using System.Threading.Tasks;

namespace HalalLibrary.API.Services.Commands.CertifyingBody
{
  public class InsertCertifyingBodyCommand : IUnitOfWorkCommand<long>
  {
    readonly Model.CertifyingBody _data;

    public InsertCertifyingBodyCommand(Model.CertifyingBody data)
    {
      _data = data;
    }

    public async Task<long> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();

       var ID = await dbContext.CertifyingBody.InsertCertifyingBody(_data);

      if (ID == 0)
      {
        var logText = await dbContext.Translation.GetTranslation(Locale.EN, "ValidateCertifyingBody");
        throw new BadRequestException(logText);
      }

      uow.Commit();

      return ID;
    }
  }
}
