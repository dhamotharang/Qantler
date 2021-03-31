using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Finance.API.Extensions;
using Finance.Model;

namespace Finance.API.Strategies.Billing.Billables
{
  public class ApplicationFee : IBillable
  {
    readonly BillingContext _context;

    public ApplicationFee(BillingContext context)
    {
      _context = context;
    }

    public async Task<IList<BillLineItem>> Generate(BillRequest request)
    {
      var result = new List<BillLineItem>();

      if (  (request.Type == BillType.Stage1 && request.RequestType == BillRequestType.New)
         || (request.Type == BillType.Stage2 && request.RequestType == BillRequestType.Renewal))
      {
        var code = await (request.Expedite
          ? _context.GetTransactionCode(TransactionCodes.ApplicationFeeExpress, request.ReferenceDate)
          : _context.GetTransactionCode(TransactionCodes.ApplicationFeeNormal, request.ReferenceDate));

        var gst = await _context.GST();
        var amount = BillingUtils.CalculateAmount(code, 1M);
        var gstAmount = amount * gst;
        var section = BillingUtils.LineItemSection(request.Type);

        result.Add(new BillLineItem
        {
          SectionIndex = section.Item1,
          Section = section.Item2,
          Qty = 1M,
          CodeID = code.ID,
          Code = code.Code,
          Descr = code.Text,
          UnitPrice = code.GetLatestPriceAmount(),
          Amount = decimal.Round(amount, 2),
          GSTAmount = decimal.Round(gstAmount, 2),
          GST = gst,
          WillRecord = true
        });
      }

      return result;
    }
  }
}
