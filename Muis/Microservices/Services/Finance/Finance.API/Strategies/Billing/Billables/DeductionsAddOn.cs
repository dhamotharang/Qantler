using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Finance.API.Repository;
using Finance.API.Services;
using Finance.Model;

namespace Finance.API.Strategies.Billing.Billables
{
  public class DeductionsAddOn : IBillable
  {
    readonly DbContext _dbContext;

    public DeductionsAddOn(DbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public async Task<IList<BillLineItem>> Generate(BillRequest request)
    {
      var result = new List<BillLineItem>();
      if (request.Type == BillType.Stage1)
      {
        return result;
      }

      var bill = (await _dbContext.Bill.Query(new BillFilter
      {
        RequestID = request.RequestID
      })).Where(e => e.Status == BillStatus.Paid && e.Type == BillType.Stage1).FirstOrDefault();

      var payment = bill?.Payments.FirstOrDefault(e => e.Status == PaymentStatus.Processed);
      if (payment != null)
      {
        // Use bill amount instead of payment amount. For the reason, the amount from payment
        // can be total of all bills paid.
        result.Add(new BillLineItem
        {
          SectionIndex = 4,
          Section = "Deductions",
          Descr = $"Stage 1 Payment - paid on {payment.PaidOn.Value.AddHours(8):dd MMM yyyy}",
          Amount = -bill.Amount,
          GSTAmount = -bill.GSTAmount,
          GST = payment.GST,
          WillRecord = false
        });
      }

      return result;
    }
  }
}
