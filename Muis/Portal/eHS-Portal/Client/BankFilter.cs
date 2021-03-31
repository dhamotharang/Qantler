using eHS.Portal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eHS.Portal.Client
{
  public class BankFilter
  {
    public string AccountNo { get; set; }

    public string AccountName { get; set; }

    public string BankName { get; set; }

    public string BranchCode { get; set; }

    public DDAStatus? DDAStatus { get; set; }

  }
}
