using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using Request.Model;

namespace Request.API.Services.Commands.Code
{
  public class GenerateCodeCommand : IUnitOfWorkCommand<string>
  {
    readonly CodeType _type;

    public GenerateCodeCommand(CodeType type)
    {
      _type = type;
    }

    public async Task<string> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);

      var series = $"{await dbContext.Code.GenerateCodeSeries(_type)}";

      var prefix = _type == CodeType.Code ? "C" : "G";

      return $"{prefix}{series.PadLeft(5, '0')}";
    }
  }
}
