using Core.API.Repository;
using Core.Model;
using Dapper;
using System.Data;
using System.Threading.Tasks;

namespace Case.API.Repository
{
  public class AttachmentRepository : IAttachmentRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public AttachmentRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<long> InsertAttachment(Attachment entity)
    {
      var param = new DynamicParameters();
      param.Add("@FileID", entity.FileID);
      param.Add("@FileName", entity.FileName);
      param.Add("@Extension", entity.Extension);
      param.Add("@Size", entity.Size);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertAttachment",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<long>("@ID");
    }
  }
}
