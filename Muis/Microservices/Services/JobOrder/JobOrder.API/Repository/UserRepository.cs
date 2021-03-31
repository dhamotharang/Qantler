using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Core.API.Repository;
using Core.Model;
using Dapper;

namespace JobOrder.API.Repository
{
  public class UserRepository : IUserRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public UserRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<Officer> GetUserByID(Guid id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      return (await SqlMapper.QueryAsync<Officer>(_unitOfWork.Connection,
        "GetOfficerByID",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task InsertOrReplace(Officer user)
    {
      var param = new DynamicParameters();
      param.Add("@ID", user.ID);
      param.Add("@Name", user.Name);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertOrReplaceOfficer",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }
  }
}
