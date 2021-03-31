using System;
using System.Collections.Generic;
using System.Linq;
using Finance.Model;

namespace Finance.API.Extensions
{
  public static class BillExtensions
  {
    public static void AddLineItem(this Bill bill, BillLineItem lineItem)
    {
      if (bill.LineItems == null)
      {
        bill.LineItems = new List<BillLineItem>();
      }
      lineItem.Index = bill.LineItems.Count() + 1;
      bill.LineItems.Add(lineItem);
    }
  }
}
