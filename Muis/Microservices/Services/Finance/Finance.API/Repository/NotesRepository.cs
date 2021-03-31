using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Core.API.Repository;
using Core.Model;
using Dapper;
using Finance.API.Repository.Mappers;
using Finance.Model;

namespace Finance.API.Repository
{
  public class NotesRepository : INotesRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public NotesRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<long> InsertNotes(Note entity)
    {
      var param = new DynamicParameters();
      param.Add("@Text", entity.Text);
      param.Add("@CreatedBy", entity.Officer.ID);
      param.Add("@CreatedByName", entity.Officer.Name);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertNotes",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<long>("@ID");
    }

    public Task MapAttachment(long notesID, long attachmentID)
    {
      var param = new DynamicParameters();
      param.Add("@NotesID", notesID);
      param.Add("@AttachmentID", attachmentID);

      return SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapNotesAttachments",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }
  }
}
