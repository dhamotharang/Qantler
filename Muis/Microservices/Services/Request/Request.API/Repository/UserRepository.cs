using System;
using System.Data;
using System.Threading.Tasks;
using Core.API.Repository;
using Core.Model;
using Dapper;

namespace Request.API.Repository
{
  public class UserRepository : IUserRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public UserRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
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
