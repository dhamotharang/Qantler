using Core.API;
using Core.API.Repository;
using HalalLibrary.API.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HalalLibrary.API.Services.Commands.Supplier
{
  public class QuerySupplierCommand : IUnitOfWorkCommand<IEnumerable<Model.Supplier>>
  {
    public QuerySupplierCommand()
    {

    }

    public async Task<IEnumerable<Model.Supplier>> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Supplier.Select();
    }
  }
}
