using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Finance.API.Services;
using Finance.Model;

namespace Finance.API.Repository
{
  public interface IBillRepository
  {
    /// <summary>
    /// Query bill based on specified properties.
    /// </summary>
    Task<IList<Bill>> Query(BillFilter filter);

    /// <summary>
    /// Update bill status with specified ID.
    /// </summary>
    Task UpdateBillStatus(long id, BillStatus status);

    /// <summary>
    /// Insert bill.
    /// </summary>
    Task<long> InsertBill(Bill bill);

    /// <summary>
    /// Get bill instance based on specified id.
    /// </summary>
    Task<Bill> GetBillByID(long id);

    /// <summary>
    /// Update specified bill instance.
    /// </summary>
    Task UpdateBill(Bill bill);

    /// <summary>
    /// Insert bill line item.
    /// </summary>
    /// <param name="lineItem"></param>
    /// <returns></returns>
    Task<long> InsertBillLineItem(BillLineItem lineItem);
  }
}
