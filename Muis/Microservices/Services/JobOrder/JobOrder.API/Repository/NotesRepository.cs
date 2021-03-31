using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Core.API.Repository;
using Core.Model;
using Dapper;
using JobOrder.API.Repository.Mappers;
using JobOrder.Model;

namespace JobOrder.API.Repository
{
  public class NotesRepository : INotesRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public NotesRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<long> InsertNotes(Notes entity)
    {
      var param = new DynamicParameters();
      param.Add("@Text", entity.Text);
      param.Add("@CreatedBy", entity.Officer.ID);
      param.Add("@CreatedByName", entity.Officer.Name);
      param.Add("@JobOrderID", entity.JobOrderID);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertNotes",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<long>("@ID");
    }

    public Task MapNotesAttachments(long notesID, params long[] attachmentIDs)
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
      param.Add("@NotesID", notesID);
      param.Add("@AttachmentIDs", tAttachmentIDs.AsTableValuedParameter("dbo.BigIntType"));

      return SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapNotesAttachments",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<IList<Notes>> SelectNotes(long jobOrderID)
    {
      var param = new DynamicParameters();
      param.Add("@JobOrderID", jobOrderID);

      var mapper = new NotesMapper();

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "SelectNotes",
        new[]
        {
          typeof(Notes),
          typeof(Officer),
          typeof(Attachment)
        },
        obj =>
        {
          return mapper.Map(obj[0] as Notes,
              obj[1] as Officer,
              obj[2] as Attachment);
        },
        param,
        splitOn: "ID,CreatedBy,Attachment",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct().ToList();
    }
  }
}
