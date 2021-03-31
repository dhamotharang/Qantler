using System;
using System.Data;
using System.Threading.Tasks;
using Core.API.Repository;
using Core.Model;
using Dapper;

namespace HalalLibrary.API.Repository
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
      param.Add("@Actiontext",
        direction: ParameterDirection.Output,
        dbType: DbType.String, size: 2000);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "GetTranslationText",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<string>("@Actiontext");
    }
  }
}
