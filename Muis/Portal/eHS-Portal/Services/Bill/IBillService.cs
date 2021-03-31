using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eHS.Portal.Client;

namespace eHS.Portal.Services.Bill
{
  public interface IBillService
  {
    /// <summary>
    /// Filter bill based on specified parameters.
    /// </summary>
    Task<IList<Model.Bill>> Filter(BillFilter filter);

    Task<Model.Bill> GetByID(long id);
  }
}
