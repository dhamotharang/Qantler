using Core.API.Repository;
using Core.Model;
using Dapper;
using System.Data;
using System.Threading.Tasks;

namespace JobOrder.API.Repository
{
  public class TranslationRepository : ITranslationRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public TranslationRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<string> GetTranslation(Locale locale, string key)
    {
      var param = new DynamicParameters();
      param.Add("@Locale", locale);
      param.Add("@Key", key);
      param.Add("@Text", direction: ParameterDirection.Output, dbType: DbType.String, size: 2000);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "GetTranslation",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<string>("@Text");
    }
  }
}
