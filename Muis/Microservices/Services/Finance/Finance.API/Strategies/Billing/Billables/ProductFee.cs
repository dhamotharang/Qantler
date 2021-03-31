using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Model;
using Finance.API.Extensions;
using Finance.Model;

namespace Finance.API.Strategies.Billing.Billables
{
  public class ProductFee : IBillable
  {
    readonly BillingContext _context;

    public ProductFee(BillingContext context)
    {
      _context = context;
    }

    public async Task<IList<BillLineItem>> Generate(BillRequest request)
    {
      var result = new List<BillLineItem>();

      if (request.LineItems?.Any() ?? false)
      {
        foreach(var item in request.LineItems)
        {
          if (  item.NoOfProducts > 0
             && (item.Scheme == Scheme.Endorsement
                || item.SubScheme == SubScheme.Product
                || item.SubScheme == SubScheme.WholePlant))
          {
            var duration = BillingUtils.CalculateNoOfMonths(
              item.StartsFrom.Date,
              item.ExpiresOn.Date);

            var code = await GetTransactionCode(item.StartsFrom, item.Scheme, item.SubScheme);

            var unitPrice = code.GetLatestPriceAmount();
            var gst = await _context.GST();
            var qty = Math.Round(duration / 12M, 2);
            var fee = item.NoOfProducts * unitPrice * qty;
            var gstAmount = fee * gst;

            var section = BillingUtils.LineItemSection(request.Type);

            result.Add(new BillLineItem
            {
              SectionIndex = section.Item1,
              Section = section.Item2,
              Qty = qty,
              CodeID = code.ID,
              Code = code.Code,
              Descr = $"{code.Text} x {item.NoOfProducts} prd. ({duration} months)",
              UnitPrice = code.GetLatestPriceAmount(),
              Amount = decimal.Round(fee, 2),
              GSTAmount = decimal.Round(gstAmount, 2),
              GST = gst,
              WillRecord = true
            });
          }
        }
      }

      return result;
    }

    async Task<TransactionCode> GetTransactionCode(DateTimeOffset refDate, Scheme scheme,
      SubScheme? subScheme)
    {
      string code = "";

      if (scheme == Scheme.Endorsement)
      {
        code = TransactionCodes.EndoresementHalalCertMark;
      }
      else if (subScheme == SubScheme.Product)
      {
        code = TransactionCodes.ProductHalalCertMark;
      }
      else if (subScheme == SubScheme.WholePlant)
      {
        code = TransactionCodes.WholePlantHalalCertMark;
      }

      return await _context.GetTransactionCode(code, refDate);
    }
  }
}
