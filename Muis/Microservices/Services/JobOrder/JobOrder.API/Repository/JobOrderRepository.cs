using Core.API.Repository;
using Core.Model;
using Dapper;
using JobOrder.API.Models;
using JobOrder.API.Repository.Mappers;
using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JobOrder.API.Repository
{
  public class JobOrderRepository : IJobOrderRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public JobOrderRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Model.JobOrder>> Select(
      JobOrderOptions options)
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

      var mapper = new JobOrderMapper();

      var param = new DynamicParameters();
      param.Add("@ID", options.ID);
      param.Add("@CustomerName", options.Customer);
      param.Add("@Premise", options.Premise);
      param.Add("@From", options.From);
      param.Add("@To", options.To);
      param.Add("@Type", typeTable.AsTableValuedParameter("dbo.SmallIntType"));
      param.Add("@Status", statusTable.AsTableValuedParameter("dbo.SmallIntType"));
      param.Add("@AssignedTo", assignedToTable.AsTableValuedParameter("dbo.UniqueIdentifierType"));

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "SelectJobOrder",
        new[]
        {
          typeof(Model.JobOrder),
          typeof(JobOrderLineItem),
          typeof(Customer),
          typeof(Officer),
          typeof(Premise),
          typeof(Person),
          typeof(ContactInfo)
        },
        obj =>
        {
          return mapper.Map(obj[0] as Model.JobOrder,
            obj[1] as JobOrderLineItem,
            obj[4] as Premise,
            obj[2] as Customer,
            obj[5] as Person,
            obj[6] as ContactInfo,
            officer: obj[3] as Officer);
        },
        param,
        splitOn: "ID,LineItemID,CustomerID,OfficerID,PremiseID,PersonID,ContactInfoID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct();
    }

    public async Task<long> InsertJobOrder(Model.JobOrder model)
    {
      var param = new DynamicParameters();
      param.Add("@RefID", model.RefID);
      param.Add("@Type", model.Type);
      param.Add("@Status", model.Status);
      param.Add("@Notes", model.Notes);
      param.Add("@CustomerID", model.Customer.ID);
      param.Add("@CustomerName", model.Customer.Name);
      param.Add("@CustomerCode", model.Customer.Code);
      param.Add("@OfficerID", model.Officer?.ID);
      param.Add("@OfficerName", model.Officer?.Name);
      param.Add("@TargetDate", model.TargetDate);
      param.Add("@ScheduledOn", model.ScheduledOn);
      param.Add("@ScheduledOnTo", model.ScheduledOnTo);
      param.Add("@ContactPersonID", model.ContactPerson?.ID);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertJobOrder",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      var result = param.Get<long>("@ID");

      if (model.LineItems?.Any() ?? false)
      {
        foreach (var lineItem in model.LineItems)
        {
          var lparam = new DynamicParameters();
          lparam.Add("@Scheme", lineItem.Scheme);
          lparam.Add("@SubScheme", lineItem.SubScheme);
          lparam.Add("@ChecklistHistoryID", lineItem.ChecklistHistoryID);
          lparam.Add("@JobID", result);
          lparam.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

          await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
            "InsertJobOrderLineItem",
            lparam,
            commandType: CommandType.StoredProcedure,
            transaction: _unitOfWork.Transaction);
        }
      }

      if (model.Premises?.Any() ?? false)
      {
        var premises = new DataTable();
        premises.Columns.Add("A", typeof(long));
        premises.Columns.Add("B", typeof(long));

        foreach (var premise in model.Premises)
        {
          var premParam = new DynamicParameters();
          premParam.Add("@ID", premise.ID);
          premParam.Add("@IsLocal", premise.IsLocal);
          premParam.Add("@Name", premise.Name);
          premParam.Add("@Type", premise.Type);
          premParam.Add("@Area", premise.Area);
          premParam.Add("@Schedule", premise.Schedule);
          premParam.Add("@BlockNo", premise.BlockNo);
          premParam.Add("@UnitNo", premise.UnitNo);
          premParam.Add("@FloorNo", premise.FloorNo);
          premParam.Add("@BuildingName", premise.BuildingName);
          premParam.Add("@Address1", premise.Address1);
          premParam.Add("@Address2", premise.Address2);
          premParam.Add("@City", premise.City);
          premParam.Add("@Province", premise.Province);
          premParam.Add("@Country", premise.Country);
          premParam.Add("@Postal", premise.Postal);
          premParam.Add("@Latitude", premise.Latitude);
          premParam.Add("@Longtitude", premise.Longitude);
          premParam.Add("@Grade", null);
          premParam.Add("@IsHighPriority", null);

          await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
            "InsertOrReplacePremise",
            premParam,
            commandType: CommandType.StoredProcedure,
            transaction: _unitOfWork.Transaction);

          premises.Rows.Add(result, premise.ID);
        }

        var premisesParam = new DynamicParameters();
        premisesParam.Add("@IDMappingType", premises.AsTableValuedParameter("dbo.IDMappingType"));

        await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
          "InsertJobOrderPremises",
          premisesParam,
          commandType: CommandType.StoredProcedure,
          transaction: _unitOfWork.Transaction);
      }

      return result;
    }

    public async Task UpdateJobOrder(Model.JobOrder model)
    {
      var param = new DynamicParameters();
      param.Add("@ID", model.ID);
      param.Add("@RefID", model.RefID);
      param.Add("@Type", model.Type);
      param.Add("@Status", model.Status);
      param.Add("@Notes", model.Notes);
      param.Add("@CustomerID", model.Customer.ID);
      param.Add("@OfficerID", model.Officer?.ID);
      param.Add("@OfficerName", model.Officer?.Name);
      param.Add("@TargetDate", model.TargetDate);
      param.Add("@ScheduledOn", model.ScheduledOn);
      param.Add("@ScheduledOnTo", model.ScheduledOnTo);
      param.Add("@ContactPersonID", model.ContactPersonID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateJobOrder",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<IEnumerable<Model.JobOrder>> GetJobOrders(Guid assignedTo,
    DateTimeOffset? lastupdatedOn)
    {
      var param = new DynamicParameters();
      param.Add("@AssignedTo", assignedTo);
      param.Add("@LastUpdatedOn", lastupdatedOn);

      var mapper = new JobOrderMapper();

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "GetJobOrderByAssignedToID",
        new[]
        {
          typeof(Model.JobOrder),
          typeof(Attendee),
          typeof(Officer),
          typeof(Findings),
          typeof(FindingsLineItem),
          typeof(Premise),
          typeof(Attachment),
          typeof(Attachment),
          typeof(Customer),
          typeof(JobOrderLineItem),
          typeof(Person),
          typeof(ContactInfo)
        },
        obj =>
        {
          return mapper.Map(obj[0] as Model.JobOrder,
            obj[9] as JobOrderLineItem,
            obj[5] as Premise,
            obj[8] as Customer,
            obj[10] as Person,
            obj[11] as ContactInfo,
            obj[1] as Attendee,
            obj[2] as Officer,
            obj[6] as Attachment,
            findings: obj[3] as Findings,
            findingsLineItem: obj[4] as FindingsLineItem,
            findingsLineItemAttachment: obj[7] as Attachment);
        },
        param,
        splitOn: "ID,AttendeeID,OfficerID,FindingsID,FindingsLineItemID" +
          ",PremiseID,AttachmentID,FindingsAttachmentID,CustomerID,LineItemID,PersonID"+
          ",ContactInfoID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct();
    }

    public async Task UpdateJobOrderStatus(long id, JobOrderStatus newState)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);
      param.Add("@Status", (int)newState);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateJobOrderState",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

    }

    public async Task<Model.JobOrder> GetJobOrderByIDBasic(long id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      return (await SqlMapper.QueryAsync<Model.JobOrder>(_unitOfWork.Connection,
        "GetJobOrderByIDBasic",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task<Model.JobOrder> GetJobOrderByID(long id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      var mapper = new JobOrderMapper();

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "GetJobOrderByID",
        new[]
        {
          typeof(Model.JobOrder),
          typeof(JobOrderLineItem),
          typeof(Customer),
          typeof(Officer),
          typeof(Premise),
          typeof(Attachment),
          typeof(Attendee),
          typeof(Officer),
          typeof(Log),
          typeof(Findings),
          typeof(Officer),
          typeof(FindingsLineItem),
          typeof(Attachment),
          typeof(Attachment),
          typeof(Person),
          typeof(ContactInfo)
        },
        obj =>
        {
          return mapper.Map(obj[0] as Model.JobOrder,
            obj[1] as JobOrderLineItem,
            obj[4] as Premise,
            obj[2] as Customer,
            obj[14] as Person,
            obj[15] as ContactInfo,
            obj[6] as Attendee,
            obj[3] as Officer,
            obj[5] as Attachment,
            obj[7] as Officer,
            obj[8] as Log,
            obj[9] as Findings,
            obj[10] as Officer,
            obj[11] as FindingsLineItem,
            obj[12] as Attachment,
            obj[13] as Attachment);
        },
        param,
        splitOn: "ID,LineItemID,CustomerID,OfficerID,PremiseID,LineItemAttachment,Attendee,Invitee," +
          "LogID,Findings,FindingsOfficer,FindingsLineItem,FLIAttachment,Signature,PersonID"+
          ",ContactInfoID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task AddAttendees(long id, IList<Attendee> attendees)
    {
      foreach (Attendee attendee in attendees)
      {
        var param = new DynamicParameters();
        param.Add("@Name", attendee.Name);
        param.Add("@Designation", attendee.Designation);
        param.Add("@Start", attendee.Start);
        param.Add("@End", attendee.End);
        param.Add("@JobID", id);
        param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

        await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
          "InsertAttendees",
          param,
          commandType: CommandType.StoredProcedure,
          transaction: _unitOfWork.Transaction);

        var result = param.Get<long>("@ID");

        if (attendee.StartingSignature != null)
        {
          var attachmentsTable = new DataTable();
          attachmentsTable.Columns.Add("A", typeof(long));
          attachmentsTable.Columns.Add("B", typeof(long));

          var aParam = new DynamicParameters();
          aParam.Add("@FileID", attendee.StartingSignature.FileID);
          aParam.Add("@FileName", attendee.StartingSignature.FileName);
          aParam.Add("@Extension", attendee.StartingSignature.Extension);
          aParam.Add("@Size", attendee.StartingSignature.Size);
          aParam.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

          await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
            "InsertAttachment",
            aParam,
            commandType: CommandType.StoredProcedure,
            transaction: _unitOfWork.Transaction);

          attachmentsTable.Rows.Add(result, aParam.Get<long>("@ID"));

          // Insert findings attachment mapping entities
          var findingsAttachmentParam = new DynamicParameters();
          findingsAttachmentParam.Add(
            "@IDMappingType",
            attachmentsTable.AsTableValuedParameter("dbo.IDMappingType"));

          await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
            "InsertAttendeeAttachments",
            findingsAttachmentParam,
            commandType: CommandType.StoredProcedure,
            transaction: _unitOfWork.Transaction);
        }

        if (attendee.EndingSignature != null)
        {
          var attachmentsTable = new DataTable();
          attachmentsTable.Columns.Add("A", typeof(long));
          attachmentsTable.Columns.Add("B", typeof(long));
          var aParam = new DynamicParameters();
          aParam.Add("@FileID", attendee.EndingSignature.FileID);
          aParam.Add("@FileName", attendee.EndingSignature.FileName);
          aParam.Add("@Extension", attendee.EndingSignature.Extension);
          aParam.Add("@Size", attendee.EndingSignature.Size);
          aParam.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

          await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
            "InsertAttachment",
            aParam,
            commandType: CommandType.StoredProcedure,
            transaction: _unitOfWork.Transaction);

          attachmentsTable.Rows.Add(result, aParam.Get<long>("@ID"));

          // Insert findings attachment mapping entities
          var findingsAttachmentParam = new DynamicParameters();
          findingsAttachmentParam.Add(
            "@IDMappingType",
            attachmentsTable.AsTableValuedParameter("dbo.IDMappingType"));

          await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
            "InsertAttendeeAttachments",
            findingsAttachmentParam,
            commandType: CommandType.StoredProcedure,
            transaction: _unitOfWork.Transaction);
        }
      }
    }

    public async Task<IEnumerable<Attendee>> GetAttendeesByJobID(long id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      return (await SqlMapper.QueryAsync<Attendee>(_unitOfWork.Connection,
        "GetAttendeesByJobID",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction));
    }

    public async Task UpdateAttendees(long id, IList<Attendee> attendees)
    {
      var dt = new DataTable();
      dt.Columns.Add("ID", typeof(long));
      dt.Columns.Add("Name", typeof(string));
      dt.Columns.Add("Designation", typeof(string));
      dt.Columns.Add("Start", typeof(bool));
      dt.Columns.Add("End", typeof(bool));
      dt.Columns.Add("JobID", typeof(long));

      foreach (Attendee attendee in attendees)
      {
        DataRow row = dt.NewRow();
        row["ID"] = attendee.ID;
        row["Name"] = attendee.Name;
        row["Designation"] = attendee.Designation;
        row["Start"] = attendee.Start;
        row["End"] = attendee.End;
        dt.Rows.Add(row);
      }

      var param = new DynamicParameters();
      param.Add("@ID", id);
      param.Add("@Attendees", dt.AsTableValuedParameter("dbo.AttendeeType"));

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateAttendees",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task MapLog(long ID, long logID)
    {
      var param = new DynamicParameters();
      param.Add("@JobOrderID", ID);
      param.Add("@LogID", logID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapLogToJobRequest",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<long> InsertRescheduleHistory(long jobID, Guid masterID, string notes)
    {
      var param = new DynamicParameters();
      param.Add("@JobID", jobID);
      param.Add("@MasterID", masterID);
      param.Add("@Notes", notes);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
         "InsertRescheduleHistory",
         param,
         commandType: CommandType.StoredProcedure,
         transaction: _unitOfWork.Transaction);

      return param.Get<long>("@ID");
    }

    public async Task AddInvitee(long id, Officer officer)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);
      param.Add("@OfficerID", officer.ID);
      param.Add("@OfficerName", officer.Name);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "AddInvitee",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task RemoveInvitee(long id, Guid officerID)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);
      param.Add("@OfficerID", officerID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "RemoveInvitee",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task MapEmail(long jobID, long emailID)
    {
      var param = new DynamicParameters();
      param.Add("@JobId", jobID);
      param.Add("@EmailId", emailID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapEmailToJobOrder",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<IList<long>> GetEmails(long id)
    {
      var param = new DynamicParameters();
      param.Add("@JobID", id);

      return (await SqlMapper.QueryAsync<long>(_unitOfWork.Connection,
        "SelectJobOrderEmails",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).ToList();
    }
  }
}
