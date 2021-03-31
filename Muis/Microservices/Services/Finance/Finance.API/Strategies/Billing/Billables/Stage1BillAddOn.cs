using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Finance.API.Repository;
using Finance.API.Services;
using Finance.Model;

namespace Finance.API.Strategies.Billing.Billables
{
  public class Stage1BillAddOn : IBillable
  {
    readonly DbContext _dbContext;

    public Stage1BillAddOn(DbContext dbContext)
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

      if (  bill != null
         && (bill.LineItems?.Any() ?? false))
      {
        result.AddRange(bill.LineItems.OrderBy(e => e.Index)
          .Select(e =>
          {
            e.WillRecord = false;
            return e;
          }).ToList());
      }

      return result;
    }
  }
}
