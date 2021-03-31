using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using System.Linq;
using Core.API.Repository;

namespace File.API.Repository
{
  public class FileRepository : IFileRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public FileRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<Model.File> GetFileByID(Guid id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      return (await SqlMapper.QueryAsync<Model.File>(_unitOfWork.Connection,
        "GetFileByID",
        param,
        commandType:
        CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task InsertFile(Model.File file)
    {
      var param = new DynamicParameters();
      param.Add("@ID", file.ID);
      param.Add("@FileName", file.FileName);
      param.Add("@Directory", file.Directory);
      param.Add("@Extension", file.Extension);
      param.Add("@Size", file.Size);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertFile",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task DeleteFile(Guid id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "DeleteFile",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

    }
  }
}
