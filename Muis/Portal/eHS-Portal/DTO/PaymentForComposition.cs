using eHS.Portal.Model;
using System;
using System.Collections.Generic;

namespace eHS.Portal.DTO
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

    public PaymentMode Mode { get; set; }

    public PaymentMethod Method { get; set; }

    public IList<Attachment> Attachments { get; set; }

    public Person ContactPerson { get; set; }

    public Officer Officer { get; set; }
  }
}
