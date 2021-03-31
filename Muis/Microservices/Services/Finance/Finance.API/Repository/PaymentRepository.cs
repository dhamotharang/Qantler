using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Core.API.Repository;
using Core.Model;
using Dapper;
using Finance.API.Repository.Mappers;
using Finance.API.Services;
using Finance.Model;

namespace Finance.API.Repository
{
  public class PaymentRepository : IPaymentRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public PaymentRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<long> Insert(Payment entity)
    {
      var param = new DynamicParameters();
      param.Add("@AltID", entity.AltID);
      param.Add("@RefNo", entity.RefNo);
      param.Add("@Status", entity.Status);
      param.Add("@Mode", entity.Mode);
      param.Add("@Method", entity.Method);
      param.Add("@AccountID", entity.AccountID);
      param.Add("@Name", entity.Name);
      param.Add("@TransactionNo", entity.TransactionNo);
      param.Add("@ReceiptNo", entity.ReceiptNo);
      param.Add("@Amount", entity.Amount);
      param.Add("@GSTAmount", entity.GSTAmount);
      param.Add("@GST", entity.GST);
      param.Add("@PaidOn", entity.PaidOn);
      param.Add("@ProcessedBy", entity.ProcessedBy);
      param.Add("@ProcessedOn", entity.ProcessedOn);
      param.Add("@ContactPersonID", entity.ContactPersonID);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertPayment",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<long>("@ID");
    }

    public async Task<Payment> GetPaymentByID(long id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      var mapper = new PaymentMapper();

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "GetPaymentByID",
        new[]
        {
            typeof(Payment),
            typeof(Bill),
            typeof(BillLineItem),
            typeof(Note),
            typeof(Attachment),
            typeof(Officer),
            typeof(Log),
            typeof(Person),
             typeof(Bank),
            typeof(ContactInfo)
        },
        obj =>
        {
          return mapper.Map(obj[0] as Payment,
            obj[1] as Bill,
            obj[2] as BillLineItem,
            obj[3] as Note,
            obj[4] as Attachment,
            obj[5] as Officer,
            obj[6] as Log,
            obj[7] as Person,
            obj[8] as Bank,
            obj[9] as ContactInfo);
        },
        param: param,
        splitOn: "ID,BillID,BillLineItemID,NoteID,AttachmentID,OfficierID,LogID,PersonID,"+
        "BankID,ContactID",
        transaction: _unitOfWork.Transaction,
        commandType: CommandType.StoredProcedure)).FirstOrDefault();
    }

    public async Task<IList<Payment>> Query(PaymentFilter options)
    {
      var param = new DynamicParameters();
      param.Add("@ID", options.ID);
      param.Add("@AccountID", options.CustomerID);
      param.Add("@Name", options.Name);
      param.Add("@TransactionNo", options.TransactionNo);
      param.Add("@Status", options.Status);
      param.Add("@Mode", options.Mode);
      param.Add("@Method", options.Method);
      param.Add("@From", options.From);
      param.Add("@To", options.To);

      return (await SqlMapper.QueryAsync<Payment>(_unitOfWork.Connection,
        "SelectPayment",
        param,
        transaction: _unitOfWork.Transaction,
        commandType: CommandType.StoredProcedure)).ToList();
    }

    public async Task<IList<Payment>> GetCustomerRecentPayment(Guid ID,
      long? rowFrom = 0,
      long? rowCount = 10)
    {
      var param = new DynamicParameters();
      param.Add("@ID", ID);
      param.Add("@RowFrom", rowFrom);
      param.Add("@RowCount", rowCount);

      return (await SqlMapper.QueryAsync<Payment>(_unitOfWork.Connection,
        "GetCustomerRecentPayment",
        param,
        transaction: _unitOfWork.Transaction,
        commandType: CommandType.StoredProcedure)).ToList();
    }

    public async Task ExecPaymentAction(long id, PaymentStatus status, Officer officer)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);
      param.Add("@Status", status);
      param.Add("@UserID", officer.ID);
      param.Add("@UserName", officer.Name);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "PaymentAction",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task MapNote(long paymentId, long noteId)
    {
      var param = new DynamicParameters();
      param.Add("@PaymentID", paymentId);
      param.Add("@NoteID", noteId);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapPaymentsNotes",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task MapLog(long paymentId, long logId)
    {
      var param = new DynamicParameters();
      param.Add("@PaymentID", paymentId);
      param.Add("@LogID", logId);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapPaymentsLog",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task MapBill(long paymentID, long billID)
    {
      var param = new DynamicParameters();
      param.Add("@PaymentID", paymentID);
      param.Add("@BillID", billID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapPaymentBill",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task MapBank(long paymentID, long bankID)
    {
      var param = new DynamicParameters();
      param.Add("@PaymentID", paymentID);
      param.Add("@BankID", bankID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapPaymentBank",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }    

    public async Task DeleteMapBank(long paymentID, long bankID)
    {
      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        $"DELETE FROM [PaymentBanks] WHERE [PaymentID] = @PaymentID AND [BankID] = @BankID",
        new
        {
          PaymentID = paymentID,
          BankID=bankID
        },
        transaction: _unitOfWork.Transaction);
    }

    public async Task<long?> GetMapBank(long paymentID)
    {      
      return (await SqlMapper.QueryAsync<long>(_unitOfWork.Connection,
        "SELECT BankID FROM [PaymentBanks] WHERE [PaymentID] = @PaymentID",
        new
        {
          PaymentID = paymentID
        },
        commandType: CommandType.Text,
        transaction: _unitOfWork.Transaction)).Distinct().FirstOrDefault();
    }
  }
}
