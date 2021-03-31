using System;
using System.Threading.Tasks;
using Finance.Model;

namespace Finance.API.Strategies.Billing
{
  public interface IBillingStrategy
  {
    public Task<Bill> Generate();
  }
}
