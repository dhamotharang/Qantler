using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Request.API.Params;
using Request.Model;
using Dapper;
using System.Data;
using Core.API;
using Request.API.Repository.Mappers;
using System.Linq;
using Request.API.Repository.DataTables;
using Core.Model;
using Core.API.Repository;

namespace Request.API.Repository
{
  public class RequestRepository : IRequestRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public RequestRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Model.Request>> Select(
      RequestOptions options)
    {
      var statusTable = new DataTable();
      statusTable.Columns.Add("Val", typeof(int));

      if (options.Status?.Any() ?? false)
      {
        foreach (var status in options.Status)
        {
          statusTable.Rows.Add(status);
        }
      }

      var typeTable = new DataTable();
      typeTable.Columns.Add("Val", typeof(int));

      if (options.Type?.Any() ?? false)
      {
        foreach (var type in options.Type)
        {
          typeTable.Rows.Add(type);
        }
      }

      var assignedToTable = new DataTable();
      assignedToTable.Columns.Add("Val", typeof(Guid));

      if (options.AssignedTo?.Any() ?? false)
      {
        foreach (var assignedTo in options.AssignedTo)
        {
          assignedToTable.Rows.Add(assignedTo);
        }
      }

      var statusMinorTable = new DataTable();
      statusMinorTable.Columns.Add("Val", typeof(int));

      if (options.StatusMinor?.Any() ?? false)
      {
        foreach (var status in options.StatusMinor)
        {
          statusMinorTable.Rows.Add(status);
        }
      }

      var mapper = new RequestMapper();

      var param = new DynamicParameters();
      param.Add("@ID", options.ID);
      param.Add("@CustomerID", options.CustomerID);
      param.Add("@CustomerCode", StringUtils.NullIfEmptyOrNull(options.CustomerCode));
      param.Add("@CustomerName", StringUtils.NullIfEmptyOrNull(options.Customer));
      param.Add("@RFAStatus", options.RFAStatus);
      param.Add("@EscalateStatus", options.EscalateStatus);
      param.Add("@From", options.From);
      param.Add("@To", options.To);
      param.Add("@Premise", StringUtils.NullIfEmptyOrNull(options.Premise));
      param.Add("@PremiseID", options.PremiseID);
      param.Add("@Type", typeTable.AsTableValuedParameter("dbo.SmallIntType"));
      param.Add("@Status", statusTable.AsTableValuedParameter("dbo.SmallIntType"));
      param.Add("@AssignedTo", assignedToTable.AsTableValuedParameter("dbo.UniqueIdentifierType"));
      param.Add("@StatusMinor", statusMinorTable.AsTableValuedParameter("dbo.SmallIntType"));

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "SelectRequest",
        new[]
        {
          typeof(Model.Request),
          typeof(Code),
          typeof(RequestLineItem),
          typeof(Premise),
          typeof(RFA)
        },
        obj =>
        {
          return mapper.Map(obj[0] as Model.Request,
            obj[1] as Code,
            obj[2] as RequestLineItem,
            obj[3] as Premise,
            obj[4] as RFA);
        },
        param,
        splitOn: "ID,CCID,LineItemID,PremiseID,RFAID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct();
    }

    public async Task<IEnumerable<Model.Request>> SelectRelatedRequests(long id)
    {
      var mapper = new RequestMapper();

      var param = new DynamicParameters();
      param.Add("@ID", id);

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "SelectRelatedRequest",
        new[]
        {
          typeof(Model.Request),
          typeof(RequestLineItem),
          typeof(Premise)
        },
        obj =>
        {
          return mapper.Map(obj[0] as Model.Request,
            obj[1] as RequestLineItem,
            obj[2] as Premise);
        },
        param,
        splitOn: "ID,LineItemID,PremiseID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct();
    }

    public async Task<Model.Request> GetRequestInfoByID(long id)
    {
      var selectRequest = $"SELECT * FROM [dbo].[Request] WHERE ID = {id}";
      return (await _unitOfWork.Connection.QueryAsync<Model.Request>(selectRequest,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task<Model.Request> GetRequestByIDBasic(long id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      var mapper = new RequestMapper();

      return (await SqlMapper.QueryAsync<Model.Request>(_unitOfWork.Connection,
        "GetRequestByIDBasic",
        new[]
        {
          typeof(Model.Request),
          typeof(RequestLineItem),
          typeof(Characteristic)
        },
        obj =>
        {
          return mapper.Map(obj[0] as Model.Request,
            lineItem: obj[1] as RequestLineItem,
            lineItemChar: obj[2] as Characteristic);
        },
        param,
        splitOn: "ID,LineItemID,LineItemCharID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task<Model.Request> GetRequestByID(long id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      var mapper = new RequestMapper();

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "GetRequestByID",
        new[]
        {
          typeof(Model.Request),
          typeof(Code),
          typeof(Code),
          typeof(HalalTeam),
          typeof(Premise),
          typeof(Characteristic),
          typeof(RFA),
          typeof(Log),
          typeof(Attachment),
          typeof(RequestLineItem),
          typeof(Review),
          typeof(ReviewLineItem),
          typeof(Characteristic)
        },
        obj =>
        {
          return mapper.Map(obj[0] as Model.Request,
            obj[1] as Code,
            obj[2] as Code,
            obj[3] as HalalTeam,
            obj[4] as Premise,
            obj[5] as Characteristic,
            obj[6] as RFA,
            obj[7] as Log,
            obj[8] as Attachment,
            obj[9] as RequestLineItem,
            obj[10] as Review,
            obj[11] as ReviewLineItem,
            obj[12] as Characteristic);
        },
        param,
        splitOn: "ID,CCID,GCID,HalalTeamID,PremiseID,CharID,RFAID,LogID,AttID,LineItemID,ReviewID," +
          "ReviewLineItemID,LiCharID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task<long> InsertRequest(Model.Request req)
    {
      DataTable dlc = null;
      var dt = DataConverter.ToRequestData(req);
      var dh = DataConverter.ToHalalTeamData(req);
      var dp = DataConverter.ToPremiseData(req);
      var dm = DataConverter.ToMenuData(req);
      var di = DataConverter.ToIngredientData(req);
      var dc = DataConverter.ToCharacteristicData(req.Characteristics?.ToArray());
      var da = DataConverter.ToAttachmentData(req);
      var dl = DataConverter.ToRequestLineItemData(req, out dlc);

      var param = new DynamicParameters();
      param.Add("@Request",
        dt.AsTableValuedParameter("dbo.ApplicationRequestType"));
      param.Add("@HalalTeam", dh.AsTableValuedParameter("dbo.HalalTeamType"));
      param.Add("@Menu", dm.AsTableValuedParameter("dbo.MenuType"));
      param.Add("@Ingredient", di.AsTableValuedParameter("dbo.IngredientType"));
      param.Add("@Premise", dp.AsTableValuedParameter("dbo.PremiseType"));
      param.Add("@Characteristics",
        dc.AsTableValuedParameter("dbo.CharacteristicsType"));
      param.Add("@Attachments",
        da.AsTableValuedParameter("dbo.AttachmentsType"));
      param.Add("@RequestLineItems",
        dl.AsTableValuedParameter("dbo.RequestLineItemType"));
      param.Add("@RequestLineItemCharacteristics",
        dlc.AsTableValuedParameter("dbo.RequestLineItemCharacteristicsType"));
      param.Add("@ID",
        dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertRequest",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<long>("@ID");
    }

    public async Task<long> UpdateRequest(Model.Request req)
    {
      DataTable dlc = null;
      var dt = DataConverter.ToRequestData(req);
      var ht = DataConverter.ToHalalTeamData(req);
      var dp = DataConverter.ToPremiseData(req);
      var dm = DataConverter.ToMenuData(req);
      var di = DataConverter.ToIngredientData(req);
      var dc = DataConverter.ToCharacteristicData(req.Characteristics?.ToArray());
      var da = DataConverter.ToAttachmentData(req);
      var dl = DataConverter.ToRequestLineItemData(req, out dlc);

      var param = new DynamicParameters();
      param.Add("@Request",
        dt.AsTableValuedParameter("dbo.ApplicationRequestType"));
      param.Add("@HalalTeam",
        ht.AsTableValuedParameter("dbo.HalalTeamType"));
      param.Add("@Menu", dm.AsTableValuedParameter("dbo.MenuType"));
      param.Add("@Ingredient", di.AsTableValuedParameter("dbo.IngredientType"));
      param.Add("@Premise", dp.AsTableValuedParameter("dbo.PremiseType"));
      param.Add("@Characteristics",
        dc.AsTableValuedParameter("dbo.CharacteristicsType"));
      param.Add("@Attachments",
        da.AsTableValuedParameter("dbo.AttachmentsType"));
      param.Add("@RequestLineItems",
        dl.AsTableValuedParameter("dbo.RequestLineItemType"));
      param.Add("@RequestLineItemCharacteristics",
        dlc.AsTableValuedParameter("dbo.RequestLineItemCharacteristicsType"));
      param.Add("@ID", req.ID,
        dbType: DbType.Int64, direction: ParameterDirection.Input);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateRequests",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return req.ID;
    }

    public async Task<long> GetRequestIDFromCharacteristic(
      Characteristic characteristic)
    {
      var dc = DataConverter.ToCharacteristicData(characteristic);

      var param = new DynamicParameters();
      param.Add("@Characteristics",
        dc.AsTableValuedParameter("dbo.CharacteristicsType"));
      param.Add("@ID",
        dbType: DbType.Int64,
        direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "GetRequestIDFromCharacteristic",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<long>("@ID");
    }

    public async Task<string> GetMessageByKeyAndLocale(int locale, string key)
    {
      await Task.CompletedTask;

      var param = new DynamicParameters();

      param.Add("@Locale", locale);
      param.Add("@Key", key);
      param.Add("@Actiontext",
        dbType: DbType.String,
        direction: ParameterDirection.Output, size: 2000);

      SqlMapper.Execute(_unitOfWork.Connection,
        "GetTranslationText",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<string>("@Actiontext");

    }

    public async Task EscalateAction(long requestID, EscalateStatus status, string remarks,
      Guid userID, string userName)
    {
      var param = new DynamicParameters();
      param.Add("@RequestID", requestID);
      param.Add("@Status", status);
      param.Add("@remarks", remarks);
      param.Add("@UserID", userID);
      param.Add("@UserName", userName);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "RequestEscalateAction",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task KIV(long id, string notes, DateTimeOffset remindOn, Guid userID,
      string userName)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);
      param.Add("@RemindOn", remindOn);
      param.Add("@Notes", notes);
      param.Add("@UserID", userID);
      param.Add("@UserName", userName);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "KIVRequest",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task RevertKIV(long id, Guid userID, string userName)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);
      param.Add("@UserID", userID);
      param.Add("@UserName", userName);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "RevertKIV",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task UpdateStatus(long id, RequestStatus status, RequestStatusMinor? statusMinor,
      RequestStatus? oldStatus = null)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);
      param.Add("@Status", status);
      param.Add("@StatusMinor", statusMinor);
      param.Add("@OldStatus", oldStatus);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateRequestStatus",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task MapLog(long requestID, long logID)
    {
      var param = new DynamicParameters();
      param.Add("@RequestID", requestID);
      param.Add("@LogID", logID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapLogToRequest",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<long> InsertReview(Review review)
    {
      var reviewParam = new DynamicParameters();
      reviewParam.Add("@Step", review.Step);
      reviewParam.Add("@ReviewerID", review.ReviewerID);
      reviewParam.Add("@ReviewerName", review.ReviewerName);
      reviewParam.Add("@Grade", review.Grade);
      reviewParam.Add("@RefID", review.RefID);
      reviewParam.Add("@RequestID", review.RequestID);
      reviewParam.Add("@EmailID", review.EmailID);
      reviewParam.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertReview",
        reviewParam,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      var reviewID = reviewParam.Get<long>("ID");

      if (review.LineItems?.Any() ?? false)
      {
        foreach (var li in review.LineItems)
        {
          var liParam = new DynamicParameters();
          liParam.Add("@Scheme", li.Scheme);
          liParam.Add("@SubScheme", li.SubScheme);
          liParam.Add("@Approved", li.Approved);
          liParam.Add("@Remarks", li.Remarks);
          liParam.Add("@ReviewID", reviewID);
          liParam.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

          await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
            "InsertReviewLineItem",
            liParam,
            commandType: CommandType.StoredProcedure,
            transaction: _unitOfWork.Transaction);
        }
      }

      return reviewID;
    }

    public async Task<long> InsertActionHistory(RequestActionHistory entity)
    {
      var param = new DynamicParameters();
      param.Add("@Action", entity.Action);
      param.Add("@OfficerID", entity.Officer.ID);
      param.Add("@OfficerName", entity.Officer.Name);
      param.Add("@RequestID", entity.RequestID);
      param.Add("@RefID", entity.RefID);
      param.Add("@Remarks", entity.Remarks);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertRequestActionHistory",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<long>("@ID");
    }

    public async Task<IList<RequestActionHistory>> GetRequestActionHistories(long requestID)
    {
      var param = new DynamicParameters();
      param.Add("@ID", requestID);

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "GetRequestActionHistories",
        new[]
        {
          typeof(RequestActionHistory),
          typeof(Officer)
        },
        obj =>
        {
          var result = obj[0] as RequestActionHistory;
          result.Officer = obj[1] as Officer;

          return result;
        },
        param,
        splitOn: "ID,OfficerID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct().ToList();
    }

    public async Task SetAssignedOfficer(long requestID, Guid? officerID, string officerName)
    {
      var param = new DynamicParameters();
      param.Add("@ID", requestID);
      param.Add("@OfficerID", officerID);
      param.Add("@OfficerName", officerName);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "SetRequestOfficer",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<IList<Review>> QueryReview(ReviewFilter filter)
    {
      var requestIDs = new DataTable();
      requestIDs.Columns.Add("Val");

      if (filter.RequestIDs?.Any() ?? false)
      {
        foreach (var id in filter.RequestIDs)
        {
          requestIDs.Rows.Add(id);
        }
      }

      var param = new DynamicParameters();
      param.Add("RequestIDs", requestIDs.AsTableValuedParameter("dbo.BigIntType"));

      var mapper = new ReviewMapper();

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "QueryReview",
        new[]
        {
          typeof(Review),
          typeof(ReviewLineItem)
        },
        obj =>
        {
          var review = obj[0] as Review;
          var lineItem = obj[1] as ReviewLineItem;

          return mapper.Map(review, lineItem);
        },
        param,
        splitOn: "ID,LineItemID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct().ToList();
    }

    public async Task MapJobOrderToRequest(long requestID, long jobID)
    {
      var param = new DynamicParameters();
      param.Add("@RequestID", requestID);
      param.Add("@JobID", jobID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapJobOrderToRequest",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<Model.Request> GetRequestByRefID(string refID)
    {
      var param = new DynamicParameters();
      param.Add("@RefID", refID);

      var mapper = new RequestMapper();

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "GetRequestByRefID",
        new[]
        {
          typeof(Model.Request),
          typeof(HalalTeam),
          typeof(Premise),
          typeof(Characteristic),
          typeof(RFA),
          typeof(Log),
          typeof(Attachment),
          typeof(RequestLineItem),
          typeof(Review),
          typeof(ReviewLineItem),
          typeof(Characteristic)
        },
        obj =>
        {
          Model.Request re = obj[0] as Model.Request;
          HalalTeam ha = obj[1] as HalalTeam;
          Premise pr = obj[2] as Premise;
          Characteristic ch = obj[3] as Characteristic;
          RFA r = obj[4] as RFA;
          Log l = obj[5] as Log;
          Attachment att = obj[6] as Attachment;
          RequestLineItem rli = obj[7] as RequestLineItem;
          Review rev = obj[8] as Review;
          ReviewLineItem revli = obj[9] as ReviewLineItem;
          Characteristic licha = obj[10] as Characteristic;

          return mapper.Map(re, null, null, ha, pr, ch, r, l, att, rli, rev, revli, licha);
        },
        param,
        splitOn: "ID,HalalTeamID,PremiseID,CharID,RFAID,LogID,AttID,LineItemID,ReviewID," +
          "ReviewLineItemID,LiCharID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task<long> UpdateRequestLineItem(RequestLineItem requestLineItem, long RequestID)
    {
      var param = new DynamicParameters();
      param.Add("@Scheme", requestLineItem.Scheme);
      param.Add("@SubScheme", requestLineItem.SubScheme);
      param.Add("@RequestID", RequestID);
      param.Add("@IsDeleted", requestLineItem.IsDeleted);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateReqLineItem",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      var lineItemID = param.Get<long>("ID");
      return lineItemID;
    }

    public async Task UpdateRequestLineItemCharacteristics(Characteristic chars, long LineItemID)
    {
      var param = new DynamicParameters();
      param.Add("@ID", chars.ID);
      param.Add("@Type", chars.Type);
      param.Add("@Value", chars.Value);
      param.Add("@IsDeleted", chars.IsDeleted);
      param.Add("@LineItemID", LineItemID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateReqLineItemChars",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<long> InsertRequestAttachments(Attachment attachment, long RequestID)
    {
      var param = new DynamicParameters();
      param.Add("@ReqID", RequestID);
      param.Add("@FileID", attachment.FileID);
      param.Add("@FileName", attachment.FileName);
      param.Add("@Extension", attachment.Extension);
      param.Add("@Size", attachment.Size);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertReqAttachments",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      var attachmentID = param.Get<long>("ID");
      return attachmentID;
    }

    public async Task UpdateRequestPremise(Premise premise, long RequestID)
    {
      var param = new DynamicParameters();
      param.Add("@ID", premise.ID);
      param.Add("@IsLocal", premise.Type);
      param.Add("@Name", premise.Name);
      param.Add("@Type", premise.Type);
      param.Add("@Area", premise.Area);
      param.Add("@Schedule", premise.Schedule);
      param.Add("@BlockNo", premise.BlockNo);
      param.Add("@UnitNo", premise.UnitNo);
      param.Add("@FloorNo", premise.FloorNo);
      param.Add("@BuildingName", premise.BuildingName);
      param.Add("@Address1", premise.Address1);
      param.Add("@Address2", premise.Address2);
      param.Add("@City", premise.City);
      param.Add("@Province", premise.Province);
      param.Add("@Country", premise.Country);
      param.Add("@Postal", premise.Postal);
      param.Add("@Longitude", premise.Longitude);
      param.Add("@Latitude", premise.Latitude);
      param.Add("@CreatedOn", premise.CreatedOn);
      param.Add("@ModifiedOn", premise.ModifiedOn);
      param.Add("@IsDeleted", premise.IsDeleted);
      param.Add("@ReqID", RequestID);
      param.Add("@IsPrimary", premise.IsPrimary);
      param.Add("@ChangeType", premise.ChangeType);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateReqPremise",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task UpdateRequestHalalTeam(HalalTeam halalTeam, long RequestID)
    {
      var param = new DynamicParameters();
      param.Add("@ID", halalTeam.ID);
      param.Add("@AltID", halalTeam.AltID);
      param.Add("@Name", halalTeam.Name);
      param.Add("@Designation", halalTeam.Designation);
      param.Add("@Role", halalTeam.Role);
      param.Add("@IsCertified", halalTeam.IsCertified);
      param.Add("@JoinedOn", halalTeam.JoinedOn);
      param.Add("@ChangeType", halalTeam.ChangeType);
      param.Add("@RequestID", RequestID);
      param.Add("@CreatedOn", halalTeam.CreatedOn);
      param.Add("@ModifiedOn", halalTeam.ModifiedOn);
      param.Add("@IsDeleted", halalTeam.IsDeleted);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateReqHalalTeam",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task UpdateRequestMenu(Menu menu, long RequestID)
    {
      var param = new DynamicParameters();
      param.Add("@ID", menu.ID);
      param.Add("@Scheme", menu.Scheme);
      param.Add("@Text", menu.Text);
      param.Add("@SubText", menu.SubText);
      param.Add("@Approved", menu.Approved);
      param.Add("@Remarks", menu.Remarks);
      param.Add("@ReviewedOn", menu.ReviewedOn);
      param.Add("@ChangeType", menu.ChangeType);
      param.Add("@RequestID", RequestID);
      param.Add("@CreatedOn", menu.CreatedOn);
      param.Add("@ModifiedOn", menu.ModifiedOn);
      param.Add("@IsDeleted", menu.IsDeleted);
      param.Add("@ReviewedBy", menu.ReviewedBy);
      param.Add("@Group", menu.Group);
      param.Add("@Index", menu.Index);
      param.Add("@Undeclared", menu.Undeclared);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateReqMenu",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task UpdateRequestIngredient(Ingredient ingredient, long RequestID)
    {
      var param = new DynamicParameters();
      param.Add("@ID", ingredient.ID);
      param.Add("@RiskCategory", ingredient.RiskCategory);
      param.Add("@Text", ingredient.Text);
      param.Add("@SubText", ingredient.SubText);
      param.Add("@Approved", ingredient.Approved);
      param.Add("@Remarks", ingredient.Remarks);
      param.Add("@ReviewedOn", ingredient.ReviewedOn);
      param.Add("@ChangeType", ingredient.ChangeType);
      param.Add("@RequestID", RequestID);
      param.Add("@CreatedOn", ingredient.CreatedOn);
      param.Add("@ModifiedOn", ingredient.ModifiedOn);
      param.Add("@IsDeleted", ingredient.IsDeleted);
      param.Add("@ReviewedBy", ingredient.ReviewedBy);
      param.Add("@Undeclared", ingredient.Undeclared);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateReqIngredient",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task UpdateRequestCharacteristics(Characteristic chars, long RequestID)
    {
      var param = new DynamicParameters();
      param.Add("@ID", chars.ID);
      param.Add("@Type", chars.Type);
      param.Add("@Value", chars.Value);
      param.Add("@IsDeleted", chars.IsDeleted);
      param.Add("@RequestID", RequestID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateReqChars",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<Model.Request> ValidateRequest
      (Scheme? Scheme, SubScheme? SubScheme, Premise premise)
    {
      var dp = DataConverter.ToPremiseData(premise);

      var param = new DynamicParameters();
      param.Add("@Scheme", (int)Scheme);
      if (SubScheme != null) { param.Add("@SubScheme", (int)SubScheme); }
      else { param.Add("@SubScheme", SubScheme); }
      param.Add("@Premise", dp.AsTableValuedParameter("dbo.PremiseType"));

      var mapper = new RequestMapper();

      return (await SqlMapper.QueryAsync<Model.Request>(_unitOfWork.Connection,
        "ValidateRequest",
        new[]
        {
          typeof(Model.Request),
          typeof(Model.RequestLineItem),
          typeof(Model.Premise)
        },
        obj =>
        {
          return mapper.Map(obj[0] as Model.Request,
             obj[1] as RequestLineItem,
             obj[2] as Premise);
        },
        param,
        splitOn: "ID,LineItemID,PremiseID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task<Model.Request> GetParentRequest
     (Scheme? Scheme, SubScheme? SubScheme, Premise premise,
      RequestStatus[] statuses, RequestType[] requestTypes)
    {
      var statusTable = new DataTable();
      statusTable.Columns.Add("Val", typeof(int));

      if (statuses?.Any() ?? false)
      {
        foreach (var status in statuses)
        {
          statusTable.Rows.Add(status);
        }
      }

      var typeTable = new DataTable();
      typeTable.Columns.Add("Val", typeof(int));

      if (requestTypes?.Any() ?? false)
      {
        foreach (var rtype in requestTypes)
        {
          typeTable.Rows.Add(rtype);
        }
      }

      var param = new DynamicParameters();
      param.Add("@Scheme", (int)Scheme);
      if (SubScheme != null) { param.Add("@SubScheme", (int)SubScheme); }
      else { param.Add("@SubScheme", SubScheme); }
      param.Add("@BlockNo", premise.BlockNo);
      param.Add("@UnitNo", premise.UnitNo);
      param.Add("@FloorNo", premise.FloorNo);
      param.Add("@BuildingName", premise.BuildingName);
      param.Add("@Address1", premise.Address1);
      param.Add("@Postal", premise.Postal);
      param.Add("@Status", statusTable.AsTableValuedParameter("dbo.SmallIntType"));
      param.Add("@ReqTypes", typeTable.AsTableValuedParameter("dbo.SmallIntType"));

      var mapper = new RequestMapper();

      return (await SqlMapper.QueryAsync<Model.Request>(_unitOfWork.Connection,
        "GetParentRequest",
        new[]
        {
          typeof(Model.Request),
          typeof(Model.RequestLineItem),
          typeof(Model.Premise)
        },
        obj =>
        {
          return mapper.Map(obj[0] as Model.Request,
             obj[1] as RequestLineItem,
             obj[2] as Premise);
        },
        param,
        splitOn: "ID,LineItemID,PremiseID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

  }
}
