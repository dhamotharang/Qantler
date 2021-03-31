using Case.Model;
using Core.API.Repository;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Case.API.Repository
{
  public class MasterRepository : IMasterRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public MasterRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Master>> GetMasterList(MasterType type)
    {
      var param = new DynamicParameters();
      param.Add("@Type", type);

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "GetMaster",
        new[]
        {
          typeof(Master)
        },
        obj =>
        {
          return obj[0] as Master;
        },
        param,
        commandType: System.Data.CommandType.StoredProcedure));
    }
  }
}
