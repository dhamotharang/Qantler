using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Request.API.Repository.Mappers;
using Request.Model;
using Core.Model;
using Request.API.Repository.DataTables;
using Core.API.Repository;
using System.Collections.Generic;

namespace Request.API.Repository
{
  public class RFARepository : IRFARepository
  {
    readonly IUnitOfWork _unitOfWork;

    public RFARepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<RFA> GetRFAByID(long id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      var mapper = new RFAMapper();

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "GetRFAByID",
        new[]
        {
          typeof(RFA),
          typeof(RFALineItem),
          typeof(Attachment),
          typeof(RFAReply),
          typeof(Attachment),
          typeof(Log)
        },
        obj =>
        {
          var rfa = obj[0] as RFA;
          var lineItem = obj[1] as RFALineItem;
          var liAtt = obj[2] as Attachment;
          var rep = obj[3] as RFAReply;
          var repAtt = obj[4] as Attachment;
          var rfaLog = obj[5] as Log;

          return mapper.Map(rfa, lineItem, liAtt, rep, repAtt, rfaLog);
        },
        param,
        splitOn: "ID,LineItemID,AttachmentID,ReplyID,AttachmentID,LogID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task<long> InsertRFA(RFA rfa)
    {
      var rfaParam = new DynamicParameters();
      rfaParam.Add("@Status", rfa.Status);
      rfaParam.Add("@RaisedBy", rfa.RaisedBy);
      rfaParam.Add("@RaisedByName", rfa.RaisedByName);
      rfaParam.Add("@DueOn", rfa.DueOn);
      rfaParam.Add("@RequestID", rfa.RequestID);
      rfaParam.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertRFA",
        rfaParam,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      var id = rfaParam.Get<long>("@ID");

      // Insert lineitem section
      if (rfa.LineItems?.Any() ?? false)
      {
        foreach (var li in rfa.LineItems)
        {
          var lineItemID = 0L;

          var lineItemParam = new DynamicParameters();
          lineItemParam.Add("@Scheme", li.Scheme);
          lineItemParam.Add("@Index", li.Index);
          lineItemParam.Add("@ChecklistCategoryID", li.ChecklistCategoryID);
          lineItemParam.Add("@ChecklistCategoryText", li.ChecklistCategoryText);
          lineItemParam.Add("@ChecklistID", li.ChecklistID);
          lineItemParam.Add("@ChecklistText", li.ChecklistText);
          lineItemParam.Add("@Remarks", li.Remarks);
          lineItemParam.Add("@RFAID", id);
          lineItemParam.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

          await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
            "InsertRFALineItem",
            lineItemParam,
            commandType: CommandType.StoredProcedure,
            transaction: _unitOfWork.Transaction);

          lineItemID = lineItemParam.Get<long>("@ID");

          // Insert lineitem attachments section
          if (li.Attachments?.Any() ?? false)
          {
            var attachmentsTable = new DataTable();
            attachmentsTable.Columns.Add("A", typeof(long));
            attachmentsTable.Columns.Add("B", typeof(long));

            foreach (var a in li.Attachments)
            {
              var aParam = DataConverter.Convert(a);

              await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
                "InsertAttachment",
                aParam,
                commandType: CommandType.StoredProcedure,
                transaction: _unitOfWork.Transaction);

              attachmentsTable.Rows.Add(lineItemID, aParam.Get<long>("@ID"));
            }

            // Insert lineitem attachment mapping entities
            var liAttachmentParam = new DynamicParameters();
            liAttachmentParam.Add("@IDMappingType", attachmentsTable.AsTableValuedParameter("dbo.IDMappingType"));

            await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
              "InsertRFALineItemAttachments",
              liAttachmentParam,
              commandType: CommandType.StoredProcedure,
              transaction: _unitOfWork.Transaction);
          }
        }
      }

      return id;
    }

    public async Task UpdateRFA(RFA rfa)
    {
      var rfaParam = new DynamicParameters();
      rfaParam.Add("@ID", rfa.ID);
      rfaParam.Add("@Status", rfa.Status);
      rfaParam.Add("@UserID", rfa.RaisedBy);
      rfaParam.Add("@UserName", rfa.RaisedByName);
      rfaParam.Add("@DueOn", rfa.DueOn);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateRFA",
        rfaParam,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      var idParam = new DynamicParameters();
      idParam.Add("@ID", rfa.ID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "CleanRFA",
        idParam,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      // Insert lineitem section
      if (rfa.LineItems?.Any() ?? false)
      {
        foreach (var li in rfa.LineItems)
        {
          var lineItemParam = new DynamicParameters();
          lineItemParam.Add("@Scheme", li.Scheme);
          lineItemParam.Add("@Index", li.Index);
          lineItemParam.Add("@ChecklistCategoryID", li.ChecklistCategoryID);
          lineItemParam.Add("@ChecklistCategoryText", li.ChecklistCategoryText);
          lineItemParam.Add("@ChecklistID", li.ChecklistID);
          lineItemParam.Add("@ChecklistText", li.ChecklistText);
          lineItemParam.Add("@Remarks", li.Remarks);
          lineItemParam.Add("@RFAID", rfa.ID);
          lineItemParam.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

          await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
            "InsertRFALineItem",
            lineItemParam,
            commandType: CommandType.StoredProcedure,
            transaction: _unitOfWork.Transaction);

           var lineItemID = lineItemParam.Get<long>("@ID");

          // Insert lineitem attachments section
          if (li.Attachments?.Any() ?? false)
          {
            var attachmentsTable = new DataTable();
            attachmentsTable.Columns.Add("A", typeof(long));
            attachmentsTable.Columns.Add("B", typeof(long));

            foreach (var a in li.Attachments)
            {
              var aParam = DataConverter.Convert(a);

              await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
                "InsertAttachment",
                aParam,
                commandType: CommandType.StoredProcedure,
                transaction: _unitOfWork.Transaction);

              attachmentsTable.Rows.Add(lineItemID, aParam.Get<long>("@ID"));
            }

            // Insert lineitem attachment mapping entities
            var liAttachmentParam = new DynamicParameters();
            liAttachmentParam.Add("@IDMappingType", attachmentsTable.AsTableValuedParameter("dbo.IDMappingType"));

            await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
              "InsertRFALineItemAttachments",
              liAttachmentParam,
              commandType: CommandType.StoredProcedure,
              transaction: _unitOfWork.Transaction);
          }
        }
      }
    }

    public async Task UpdateRFAStatus(long id, RFAStatus rfaStatus, Guid userID, string username)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);
      param.Add("@Status", rfaStatus);
      param.Add("@UserID", userID);
      param.Add("@UserName", username);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateRFAStatus",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task ExtendDueDate(long id, string notes, DateTimeOffset toDate,
      Guid userID, string userName)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);
      param.Add("@Notes", notes);
      param.Add("@ToDate", toDate);
      param.Add("@UserID", userID);
      param.Add("@UserName", userName);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "ExtendRFADueDate",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task Delete(long id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "DeleteRFA",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<long> InsertRFAResponse(Model.RFA rfa)
    {
      await Task.CompletedTask;
      DataTable dtLineItemAttachments = null;
      DataTable dtLineItemReply = null;
      DataTable dtLineItemReplyAttachments = new DataTable();

      var dr = DataConverter.ToRFAData(rfa);
      //var da = DataConverter.ToAttachmentData(rfa);
      var dl = DataConverter.ToRFALineItemData(rfa, out dtLineItemAttachments,
        out dtLineItemReply, out dtLineItemReplyAttachments);

      var param = new DynamicParameters();
      param.Add("@RFA", dr.AsTableValuedParameter("dbo.RFAType"));
      param.Add("@RFALineItemReply",
        dtLineItemReply.AsTableValuedParameter("dbo.RFALineItemReplyType"));
      param.Add("@LineItemReplyAttachments",
       dtLineItemReplyAttachments.AsTableValuedParameter("dbo.LineItemReplyAttachmentsType"));
      param.Add("@RFAID", rfa.ID, dbType: DbType.Int64, direction: ParameterDirection.Input);

      SqlMapper.Execute(_unitOfWork.Connection,
        "InsertRFAResponse",
          param,
          commandType: CommandType.StoredProcedure,
          transaction: _unitOfWork.Transaction);

      return param.Get<long>("@RFAID");
    }

    public async Task<IList<RFA>> Query(RFAFilter filter)
    {
      var status = new DataTable();
      status.Columns.Add("Val", typeof(int));

      if (filter.Status?.Any() ?? false)
      {
        foreach(var s in filter.Status)
        {
          status.Rows.Add(s);
        }
      }

      var param = new DynamicParameters();
      param.Add("@ID", filter.ID);
      param.Add("@RequestID", filter.RequestID);
      param.Add("@Customer", filter.Customer);
      param.Add("@RaisedBy", filter.RaisedBy);
      param.Add("@CreatedOn", filter.CreatedOn);
      param.Add("@DueOn", filter.DueOn);
      param.Add("@Status", status.AsTableValuedParameter("dbo.SmallIntType"));

      var mapper = new RFAMapper();

      return (await SqlMapper.QueryAsync<RFA>(_unitOfWork.Connection,
        "SelectRFA",
        new []
        {
          typeof(RFA),
          typeof(Model.Request)
        },
        obj =>
        {
          return mapper.Map(obj[0] as RFA,
              request: obj[1] as Model.Request);
        },
        param,
        splitOn: "ID,Request",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct().ToList();
    }
  }
}

