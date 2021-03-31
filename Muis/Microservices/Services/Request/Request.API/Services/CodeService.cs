using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Provider;
using Request.API.Services.Commands.Code;
using Request.Model;

namespace Request.API.Services
{
  public class CodeService : TransactionalService,
                             ICodeService
  {
    public CodeService(IDbConnectionProvider connectionProvider)
         : base(connectionProvider)
    {
    }

    public async Task<Code> Create(Code code)
    {
      return await Execute(new CreateCodeCommand(code));
    }

    public async Task<Code> GetByID(long id)
    {
      return await Execute(new GetCodeByID(id));
    }

    public async Task<IList<Code>> List(CodeType type)
    {
      return await Execute(new ListOfCodeCommand(type));
    }

    public async Task Update(Code code)
    {
      await Execute(new UpdateCodeCommand(code));
    }

    public async Task<string> GenerateCode(CodeType type)
    {
      return await Execute(new GenerateCodeCommand(type));
    }
  }
}
