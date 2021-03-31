using Core.API;
using Core.API.Provider;
using HalalLibrary.API.Services;
using HalalLibrary.API.Services.Commands.Supplier;
using HalalLibrary.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HalalLibrary.Service
{
  public class SupplierService : TransactionalService,
                                ISupplierService
  {

    public SupplierService(IDbConnectionProvider connectionProvider)
         : base(connectionProvider)
    {

    }

    public async Task<IEnumerable<Supplier>> Select()
    {
      return await Execute(new QuerySupplierCommand());
    }

    public async Task<long> InsertSupplier(Supplier data)
    {
      return await Execute(new InsertSupplierCommand(data));
    }
  }
}
