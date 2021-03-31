using Core.API;
using Core.API.Exceptions;
using Core.API.Repository;
using Core.Model;
using HalalLibrary.API.Repository;
using System.Threading.Tasks;

namespace HalalLibrary.API.Services.Commands.Supplier
{
  public class InsertSupplierCommand : IUnitOfWorkCommand<long>
  {
    readonly Model.Supplier _data;

    public InsertSupplierCommand(Model.Supplier data)
    {
      _data = data;
    }

    public async Task<long> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();

       var ID = await dbContext.Supplier.InsertSupplier(_data);

      if (ID == 0)
      {
        var logText = await dbContext.Translation.GetTranslation(Locale.EN, "ValidateSupplier");
        throw new BadRequestException(logText);
      }

      uow.Commit();

      return ID;
    }
  }
}
