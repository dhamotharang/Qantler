using Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Finance.API.Services
{
  public interface IBankService
  {
    /// <summary>
    /// Get list of banks
    /// </summary>
    public Task<IList<Bank>> Filter(BankFilter filter);
  }

  public class BankFilter
  {
    public string AccountNo { get; set; }

    public string AccountName { get; set; }

    public string BankName { get; set; }

    public string BranchCode { get; set; }

    public DDAStatus? DDAStatus { get; set; }

  }
}
