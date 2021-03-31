using System;
using System.Collections.Generic;
using Case.Model;
using Core.Model;

namespace Case.API.Params
{
  public class PaymentForComposition
  {
    public string RefNo { get; set; }

    public Guid PayerID { get; set; }

    public string PayerName { get; set; }

    public string BankAccountName { get; set; }

    public decimal Amount { get; set; }

    public string Notes { get; set; }

    public string RefID { get; set; }

    public IList<Attachment> Attachments { get; set; }

    public Person ContactPerson { get; set; }

    public Officer Officer { get; set; }
  }
}
