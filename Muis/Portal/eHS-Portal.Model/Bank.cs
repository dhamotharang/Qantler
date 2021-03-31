using System;
using System.Collections.Generic;
using System.Text;

namespace eHS.Portal.Model
{
  public enum DDAStatus
  {
    Default = 100,
    Approved = 200,
    Rejected = 300
  }

  public class Bank
  {
    public long ID { get; set; }

    public string AccountNo { get; set; }

    public string AccountName { get; set; }

    public string BankName { get; set; }

    public string BranchCode { get; set; }

    public DDAStatus DDAStatus { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

  }
}
