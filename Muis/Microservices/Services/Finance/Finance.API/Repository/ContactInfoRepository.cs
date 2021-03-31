using Core.API.Repository;
using Dapper;
using Finance.Model;
using System.Data;
using System.Threading.Tasks;

namespace Finance.API.Repository
{
  public class ContactInfoRepository : IContactInfoRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public ContactInfoRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<long> Insert(ContactInfo info)
    {
      var param = new DynamicParameters();
      param.Add("@Type", info.Type);
      param.Add("@Value", info.Value);
      param.Add("@IsPrimary", info.IsPrimary);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertContactInfo",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<long>("@ID");
    }

    public async Task DeleteByID(long id)
    {
      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "DELETE FROM [ContactInfo] WHERE [ID] = @ID",
        new { ID = id },
        transaction: _unitOfWork.Transaction);
    }
  }
}
