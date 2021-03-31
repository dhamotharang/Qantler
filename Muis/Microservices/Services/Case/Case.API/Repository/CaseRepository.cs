using Case.API.Params;
using Case.API.Repository.Mappers;
using Case.Model;
using Core.API.Repository;
using Core.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Case.API.Repository
{
  public class CaseRepository : ICaseRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public CaseRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Model.Case>> Select(CaseOptions options)
    {
      var mapper = new CaseMapper();
      var assignedToTable = new DataTable();
      var managedByTable = new DataTable();
      var offenceTypeTable = new DataTable();
      var statusTable = new DataTable();

      assignedToTable.Columns.Add("Val", typeof(Guid));
      managedByTable.Columns.Add("Val", typeof(Guid));
      offenceTypeTable.Columns.Add("Val", typeof(Guid));
      statusTable.Columns.Add("Val", typeof(long));

      if (options.AssignedTo?.Any() ?? false)
      {
        foreach (var assignedTo in options.AssignedTo)
        {
          assignedToTable.Rows.Add(assignedTo);
        }
      }

      if (options.ManagedBy?.Any() ?? false)
      {
        foreach (var managedBy in options.ManagedBy)
        {
          managedByTable.Rows.Add(managedBy);
        }
      }

      if (options.OffenceType?.Any() ?? false)
      {
        foreach (var offenceType in options.OffenceType)
        {
          offenceTypeTable.Rows.Add(offenceType);
        }
      }

      if (options.Status?.Any() ?? false)
      {
        foreach (var statustype in options.Status)
        {
          statusTable.Rows.Add(statustype);
        }
      }

      var param = new DynamicParameters();
      param.Add("@ID", options.ID);
      param.Add("@OffenceType", offenceTypeTable.AsTableValuedParameter("dbo.UniqueIdentifierType"));
      param.Add("@Source", options.Source);
      param.Add("@ManagedBy", managedByTable.AsTableValuedParameter("dbo.UniqueIdentifierType"));
      param.Add("@AssignedTO", assignedToTable.AsTableValuedParameter("dbo.UniqueIdentifierType"));
      param.Add("@Status", statusTable.AsTableValuedParameter("dbo.BigIntType"));
      param.Add("@From", options.From);
      param.Add("@To", options.To);

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "SelectCase",
        new[]
        {
            typeof(Model.Case),
            typeof(Officer),
            typeof(Officer),
            typeof(Person),
            typeof(Master),
            typeof(Master),
            typeof(Offender),
        },
        obj =>
        {
          return mapper.Map(obj[0] as Model.Case,
            obj[6] as Offender,
            null,
            obj[1] as Officer,
            obj[2] as Officer,
            obj[3] as Person,
           null,
            obj[4] as Master,
            obj[5] as Master);
        },
        param,
        splitOn: "managedByID,assignedToID,reporterID,breachID,offences,offenderID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct();
    }

    public async Task<Model.Case> GetByID(long id)
    {
      var mapper = new CaseMapper();

      var param = new DynamicParameters();
      param.Add("@ID", id);

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "GetCaseByID",
        new[]
        {
            typeof(Model.Case),
            typeof(Offender),
            typeof(ContactInfo),
            typeof(Officer),
            typeof(Officer),
            typeof(Person),
            typeof(ContactInfo),
            typeof(Master),
            typeof(Master),
            typeof(Attachment),
            typeof(Certificate),
            typeof(Premise),
            typeof(SanctionInfo),
        },
        obj =>
        {
          return mapper.Map(obj[0] as Model.Case,
            obj[1] as Offender,
            obj[2] as ContactInfo,
            obj[3] as Officer,
            obj[4] as Officer,
            obj[5] as Person,
            obj[6] as ContactInfo,
            obj[7] as Master,
            obj[8] as Master,
            obj[9] as Attachment,
            obj[10] as Certificate,
            obj[11] as Premise,
            obj[12] as SanctionInfo);
        },
        param,
        splitOn: "offenderID,offenderContactID,managedByID,assignedToID,reporterID,contactID,breachID," +
          "offences,attachmentID,certificateID,premiseID,sanctionInfoID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct().FirstOrDefault();
    }

    public async Task<Model.Case> GetBasicByID(long id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      return (await SqlMapper.QueryAsync<Model.Case>(_unitOfWork.Connection,
        "SELECT *, StatusMinor AS 'MinorStatus' FROM [dbo].[Case] WHERE ID = @ID", param,
        commandType: CommandType.Text,
        transaction: _unitOfWork.Transaction)).Distinct().FirstOrDefault();
    }

    public async Task<IList<Activity>> GetActivityByCaseID(long id)
    {
      var mapper = new CaseMapper();

      var param = new DynamicParameters();
      param.Add("@ID", id);

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "GetActivityByCaseID",
        new[]
        {
            typeof(Activity),
            typeof(Officer),
            typeof(Attachment),
            typeof(Letter),
            typeof(Email),
        },
        obj =>
        {
          return mapper.MapActivity(obj[0] as Activity,
            obj[1] as Officer,
            obj[2] as Attachment,
            obj[3] as Letter,
            obj[4] as Email);
        },
        param,
        splitOn: "userID,activityAttachmentID,letterID,emailID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct().ToList();
    }

    public async Task<long> InsertCase(Model.Case data)
    {
      var caseParam = new DynamicParameters();
      var offenceTypeTable = new DataTable();

      offenceTypeTable.Columns.Add("Val", typeof(Guid));
      if (data.Offences?.Any() ?? false)
      {
        foreach (var offenceType in data.Offences)
        {
          offenceTypeTable.Rows.Add(offenceType.ID);
        }
      }

      caseParam.Add("@Ref", data.RefID);
      caseParam.Add("@Source", data.Source);
      caseParam.Add("@Type", data.Type);
      caseParam.Add("@Title", data.Title);
      caseParam.Add("@Background", data.Background);
      caseParam.Add("@OffenderID", data.Offender?.ID ?? null);
      caseParam.Add("@ReportedByID", data.ReportedBy?.ID ?? null);
      caseParam.Add("@ManagedByID", data.ManagedBy.ID);
      caseParam.Add("@CreatedByID", data.CreatedBy.ID);
      caseParam.Add("@Out", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertCase",
        caseParam,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      var caseID = caseParam.Get<long>("@Out");

      foreach (var contact in data.ReportedBy.ContactInfos)
      {
        var contactParam = new DynamicParameters();
        contactParam.Add("@CaseID", caseID);
        contactParam.Add("@Type", contact.Type);
        contactParam.Add("@Value", contact.Value);

        await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
          "InsertReporterContactInfo",
          contactParam,
          commandType: CommandType.StoredProcedure,
          transaction: _unitOfWork.Transaction);
      }
      return caseID;
    }

    public async Task MapCaseOffence(IList<Master> offence, long caseID)
    {
      var param = new DynamicParameters();
      var offenceTypeTable = new DataTable();

      offenceTypeTable.Columns.Add("Val", typeof(Guid));
      if (offence?.Any() ?? false)
      {
        foreach (var offenceType in offence)
        {
          offenceTypeTable.Rows.Add(offenceType.ID);
        }
      }

      param.Add("@Offences", offenceTypeTable.AsTableValuedParameter("dbo.UniqueIdentifierType"));
      param.Add("@CaseID", caseID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapCaseOffence",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task MapCaseBreachByOffence(IList<Master> offence, long caseID)
    {
      var param = new DynamicParameters();
      var offenceTypeTable = new DataTable();

      offenceTypeTable.Columns.Add("Val", typeof(Guid));
      if (offence?.Any() ?? false)
      {
        foreach (var offenceType in offence)
        {
          offenceTypeTable.Rows.Add(offenceType.ID);
        }
      }

      param.Add("@Offences", offenceTypeTable.AsTableValuedParameter("dbo.UniqueIdentifierType"));
      param.Add("@CaseID", caseID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapCaseBreachByOffence",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task MapCasePremise(long[] premises, long caseID)
    {
      var tPremiseIDs = new DataTable();
      tPremiseIDs.Columns.Add("Val", typeof(long));

      if (premises?.Any() ?? false)
      {
        foreach (var id in premises)
        {
          tPremiseIDs.Rows.Add(id);
        }
      }

      var param = new DynamicParameters();
      param.Add("@CaseID", caseID);
      param.Add("@PremisesIDs", tPremiseIDs.AsTableValuedParameter("dbo.BigIntType"));

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapCasePremises",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task MapCaseAttachments(long notesID, params long[] attachmentIDs)
    {
      var tAttachmentIDs = new DataTable();
      tAttachmentIDs.Columns.Add("Val", typeof(long));

      if (attachmentIDs?.Any() ?? false)
      {
        foreach (var id in attachmentIDs)
        {
          tAttachmentIDs.Rows.Add(id);
        }
      }

      var param = new DynamicParameters();
      param.Add("@CaseID", notesID);
      param.Add("@AttachmentIDs", tAttachmentIDs.AsTableValuedParameter("dbo.BigIntType"));

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapCaseAttachments",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task UpdateStatus(long id, CaseStatus status, CaseMinorStatus? statusMinor = null)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);
      param.Add("@Status", status);
      param.Add("@StatusMinor", statusMinor);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateCaseStatus",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task MapCaseSanctionInfo(long caseID, long sanctionInfoID)
    {
      var param = new DynamicParameters();
      param.Add("@CaseID", caseID);
      param.Add("@SanctionInfoID", sanctionInfoID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapCaseSanctionInfo",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<long> UpdateCaseInfo(Model.Case data)
    {
      var caseParam = new DynamicParameters();

      caseParam.Add("@Ref", data.RefID);
      caseParam.Add("@Source", data.Source);
      caseParam.Add("@Status", data.Status);
      caseParam.Add("@OldStatus", data.OldStatus);
      caseParam.Add("@StatusMinor", data.MinorStatus);
      caseParam.Add("@OtherStatus", data.OtherStatus);
      caseParam.Add("@OtherStatusMinor", data.OtherStatusMinor);
      caseParam.Add("@Type", data.Type);
      caseParam.Add("@Occurrence", data.Occurrence);
      caseParam.Add("@Background", data.Background);
      caseParam.Add("@OffenderID", data.OffenderID);
      caseParam.Add("@ManagedByID", data.ManagedByID);
      caseParam.Add("@AssignedToID", data.AssignedToID);
      caseParam.Add("@ReportedByID", data.ReportedByID);
      caseParam.Add("@SuspendedFrom", data.SuspendedFrom);
      caseParam.Add("@Title", data.Title);
      caseParam.Add("@Sanction", data.Sanction);
      caseParam.Add("@ID", data.ID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateCaseInfo",
        caseParam,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return data.ID;
    }
  }
}
