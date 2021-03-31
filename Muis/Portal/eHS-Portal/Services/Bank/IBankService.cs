using eHS.Portal.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eHS.Portal.Services.Bank
{
  public interface IBankService
  {
    public Task<IList<Model.Bank>> Filter(BankFilter filter);
  }
}
