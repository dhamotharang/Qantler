using Core.Model;
using Dapper;
using JobOrder.Model;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using JobOrder.API.Repository.Mappers;
using Core.API.Repository;

namespace JobOrder.API.Repository
{
  public class FindingsRepository : IFindingsRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public FindingsRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<long> InsertFindings(Findings findings)
    {
      var param = new DynamicParameters();
      param.Add("@Remarks", findings.Remarks);
      param.Add("@OfficerID", findings.Officer?.ID);
      param.Add("@OfficerName", findings.Officer?.Name);
      param.Add("@JobID", findings.JobID);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertFindings",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      var result = param.Get<long>("@ID");

      if (findings.Attachment != null)
      {
        var attachmentsTable = new DataTable();
        attachmentsTable.Columns.Add("A", typeof(long));
        attachmentsTable.Columns.Add("B", typeof(long));

        var aParam = new DynamicParameters();
        aParam.Add("@FileID", findings.Attachment.FileID);
        aParam.Add("@FileName", findings.Attachment.FileName);
        aParam.Add("@Extension", findings.Attachment.Extension);
        aParam.Add("@Size", findings.Attachment.Size);
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
          "InsertFindingsAttachments",
          findingsAttachmentParam,
          commandType: CommandType.StoredProcedure,
          transaction: _unitOfWork.Transaction);
      }

      // Insert lineitem section
      if (findings.LineItems?.Any() ?? false)
      {
        foreach (var li in findings.LineItems)
        {
          var lineItemParam = new DynamicParameters();
          lineItemParam.Add("@Scheme", li.Scheme);
          lineItemParam.Add("@SubScheme", li.SubScheme);
          lineItemParam.Add("@Index", li.Index);
          lineItemParam.Add("@ChecklistCategoryID", li.ChecklistCategoryID);
          lineItemParam.Add("@ChecklistCategoryText", li.ChecklistCategoryText);
          lineItemParam.Add("@ChecklistItemID", li.ChecklistItemID);
          lineItemParam.Add("@ChecklistItemText", li.ChecklistItemText);
          lineItemParam.Add("@Remarks", li.Remarks);
          lineItemParam.Add("@Complied", li.Complied);
          lineItemParam.Add("@FindingsID", result);
          lineItemParam.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

          await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
            "InsertFindingsLineItem",
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
              var aParam = new DynamicParameters();
              aParam.Add("@FileID", a.FileID);
              aParam.Add("@FileName", a.FileName);
              aParam.Add("@Extension", a.Extension);
              aParam.Add("@Size", a.Size);
              aParam.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

              await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
                "InsertAttachment",
                aParam,
                commandType: CommandType.StoredProcedure,
                transaction: _unitOfWork.Transaction);

              attachmentsTable.Rows.Add(lineItemID, aParam.Get<long>("@ID"));
            }

            // Insert lineitem attachment mapping entities
            var liAttachmentParam = new DynamicParameters();
            liAttachmentParam.Add(
              "@IDMappingType",
              attachmentsTable.AsTableValuedParameter("dbo.IDMappingType"));

            await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
              "InsertFindingsLineItemAttachments",
              liAttachmentParam,
              commandType: CommandType.StoredProcedure,
              transaction: _unitOfWork.Transaction);
          }
        }
      }

      return result;
    }

    public async Task<Findings> GetFindingsByID(long id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      var mapper = new FindingsMapper();

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "GetFindingsByID",
        new[]
        {
          typeof(Findings),
          typeof(Attachment),
          typeof(FindingsLineItem),
          typeof(Attachment),
          typeof(Officer)
        },
        obj =>
        {
          Findings f = obj[0] as Findings;
          Attachment fatt = obj[1] as Attachment;
          FindingsLineItem fl = obj[2] as FindingsLineItem;
          Attachment at = obj[3] as Attachment;
          Officer o = obj[4] as Officer;
          return mapper.Map(f, fatt, fl, at, o);
        },
        param,
        splitOn: "ID,FindingsAttachmentID,FindingsLineItemID,AttachmentID,OfficerID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }
  }
}
