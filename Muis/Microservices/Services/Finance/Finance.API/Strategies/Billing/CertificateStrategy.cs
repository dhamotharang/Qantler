using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Finance.API.Repository;
using Finance.API.Strategies.Billing.Billables;
using Finance.Model;

namespace Finance.API.Strategies.Billing
{
  public class CertificateStrategy : IBillingStrategy
  {
    readonly BillRequest _request;
    readonly BillingContext _context;
    readonly DbContext _dbContext;

    public CertificateStrategy(DbContext dbContext, BillRequest request)
    {
      _request = request;
      _dbContext = dbContext;

      _context = new BillingContext(dbContext);
    }

    public async Task<Bill> Generate()
    {
      var amount = 0M;
      var gstAmount = 0M;
      var gst = await _context.GST();

      var lineItems = new List<BillLineItem>();

      lineItems.AddRange(await new Stage1BillAddOn(_dbContext).Generate(_request));
      lineItems.AddRange(await new ApplicationFee(_context).Generate(_request));
      lineItems.AddRange(await new CertificateFee(_context).Generate(_request));
      lineItems.AddRange(await new ProductFee(_context).Generate(_request));
      lineItems.AddRange(await new DeductionsAddOn(_dbContext).Generate(_request));

      for (int i = 0; i < lineItems.Count; i++)
      {
        BillLineItem lineItem = lineItems[i];

        lineItem.Index = i + 1;
        amount += lineItem.Amount;
        gstAmount += lineItem.GSTAmount;
      }

      return new Bill
      {
        Status = BillStatus.Draft,
        Type = _request.Type,
        RequestType = _request.RequestType,
        RefNo = Guid.NewGuid().ToString(),
        Amount = amount,
        GSTAmount = gstAmount,
        GST = gst,
        RequestID = _request.RequestID,
        RefID = _request.RefID,
        CustomerID = _request.CustomerID,
        CustomerName = _request.CustomerName,
        IssuedOn = _request.ReferenceDate,
        LineItems = lineItems
      };
    }
  }
}
