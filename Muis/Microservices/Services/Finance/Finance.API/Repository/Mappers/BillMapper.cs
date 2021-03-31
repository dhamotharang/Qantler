using System;
using System.Collections.Generic;
using Finance.Model;

namespace Finance.API.Repository.Mappers
{
  public class BillMapper
  {
    readonly IDictionary<long, Bill> _cache = new Dictionary<long, Bill>();

    readonly IDictionary<long, BillLineItem> _lineItemCache = new Dictionary<long, BillLineItem>();

    readonly IDictionary<long, Payment> _paymentCache = new Dictionary<long, Payment>();

    public Bill Map(Bill bill, BillLineItem lineItem = null, Payment payment = null)
    {
      if (!_cache.TryGetValue(bill.ID, out Bill result))
      {
        _cache[bill.ID] = bill;
        result = bill;
      }

      if (   (payment?.ID ?? 0) > 0
          && !_paymentCache.ContainsKey(payment.ID))
      {
        _paymentCache[payment.ID] = payment;

        if (result.Payments == null)
        {
          result.Payments = new List<Payment>();
        }
        result.Payments.Add(payment);
      }

      if (   (lineItem?.ID ?? 0) > 0
          && !_lineItemCache.ContainsKey(lineItem.ID))
      {
        _lineItemCache[lineItem.ID] = lineItem;

        if (result.LineItems == null)
        {
          result.LineItems = new List<BillLineItem>();
        }
        result.LineItems.Add(lineItem);
      }

      return result;
    }
  }
}
