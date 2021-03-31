using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Core.API.Repository;
using Dapper;
using Finance.API.Repository.Mappers;
using Finance.API.Services;
using Finance.Model;

namespace Finance.API.Repository
{
  public class BillRepository : IBillRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public BillRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<IList<Bill>> Query(BillFilter filter)
    {
      var param = new DynamicParameters();
      param.Add("ID", filter.ID);
      param.Add("RequestID", filter.RequestID);
      param.Add("RefNo", filter.RefNo);
      param.Add("RefID", filter.RefID);
      param.Add("CustomerName", filter.CustomerName);
      param.Add("Status", filter.Status);
      param.Add("From", filter.From);
      param.Add("To", filter.To);
      param.Add("InvoiceNo", filter.InvoiceNo);
      param.Add("Type", filter.Type);

      var mapper = new BillMapper();

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "SelectBill",
        new[]
        {
          typeof(Bill),
          typeof(BillLineItem),
          typeof(Payment)
        },
        obj =>
        {
          return mapper.Map(obj[0] as Bill,
            obj[1] as BillLineItem,
            obj[2] as Payment);
        },
        param,
        splitOn: "ID,LineItemID,PaymentID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct().ToList();
    }

    public async Task UpdateBillStatus(long id, BillStatus status)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);
      param.Add("@Status", status);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateBillStatus",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<long> InsertBill(Bill bill)
    {
      var param = new DynamicParameters();
      param.Add("@RefNo", bill.RefNo);
      param.Add("@Status", bill.Status);
      param.Add("@Type", bill.Type);
      param.Add("@RequestType", bill.RequestType);
      param.Add("@AccountID", bill.CustomerID);
      param.Add("@AccountName", bill.CustomerName);
      param.Add("@InvoiceNo", bill.InvoiceNo);
      param.Add("@Amount", bill.Amount);
      param.Add("@GSTAmount", bill.GSTAmount);
      param.Add("@GST", bill.GST);
      param.Add("@IssuedOn", bill.IssuedOn);
      param.Add("@DueOn", bill.DueOn);
      param.Add("@RequestID", bill.RequestID);
      param.Add("@RefID", bill.RefID);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertBill",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      var billID = param.Get<long>("@ID");

      if (bill.LineItems?.Any() ?? false)
      {
        foreach (var lineItem in bill.LineItems)
        {
          lineItem.BillID = billID;
          await InsertBillLineItem(lineItem);
        }
      }

      return billID;
    }

    public async Task<Bill> GetBillByID(long id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      var mapper = new BillMapper();

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "GetBillByID",
        new[]
        {
          typeof(Bill),
          typeof(BillLineItem),
          typeof(Payment)
        },
        obj =>
        {
          return mapper.Map(obj[0] as Bill,
            obj[1] as BillLineItem,
            obj[2] as Payment);
        },
        param,
        splitOn: "ID,LineItemID,PaymentID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task UpdateBill(Bill bill)
    {
      var param = new DynamicParameters();
      param.Add("@ID", bill.ID);
      param.Add("@RefNo", bill.RefNo);
      param.Add("@Status", bill.Status);
      param.Add("@Type", bill.Type);
      param.Add("@RequestType", bill.RequestType);
      param.Add("@AccountID", bill.CustomerID);
      param.Add("@AccountName", bill.CustomerName);
      param.Add("@InvoiceNo", bill.InvoiceNo);
      param.Add("@Amount", bill.Amount);
      param.Add("@GSTAmount", bill.GSTAmount);
      param.Add("@GST", bill.GST);
      param.Add("@IssuedOn", bill.IssuedOn);
      param.Add("@DueOn", bill.DueOn);
      param.Add("@RequestID", bill.RequestID);
      param.Add("@RefID", bill.RefID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateBill",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      var deleteParam = new DynamicParameters();
      deleteParam.Add("@BillID", bill.ID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "DeleteAllBillLineItem",
        deleteParam,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      if (bill.LineItems?.Any() ?? false)
      {
        foreach (var lineItem in bill.LineItems)
        {
          lineItem.BillID = bill.ID;
          await InsertBillLineItem(lineItem);
        }
      }
    }

    public async Task<long> InsertBillLineItem(BillLineItem lineItem)
    {
      var param = new DynamicParameters();
      param.Add("@SectionIndex", lineItem.SectionIndex);
      param.Add("@Section", lineItem.Section);
      param.Add("@Index", lineItem.Index);
      param.Add("@CodeID", lineItem.CodeID);
      param.Add("@Code", lineItem.Code);
      param.Add("@Descr", lineItem.Descr);
      param.Add("@Qty", lineItem.Qty);
      param.Add("@UnitPrice", lineItem.UnitPrice);
      param.Add("@Amount", lineItem.Amount);
      param.Add("@GSTAMount", lineItem.GSTAmount);
      param.Add("@GST", lineItem.GST);
      param.Add("@WillRecord", lineItem.WillRecord);
      param.Add("@BillID", lineItem.BillID);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertBillLineItem",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<long>("@ID");
    }
  }
}
