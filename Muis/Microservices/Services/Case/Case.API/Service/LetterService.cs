using Core.API;
using Core.API.Provider;
using Core.Model;
using Case.API.Services.Commands;
using Case.Model;
using System;
using System.Threading.Tasks;

namespace Case.API.Service
{
  public class LetterService : TransactionalService, ILetterService
  {
    public LetterService(IDbConnectionProvider connectionProvider)
         : base(connectionProvider)
    {
    }

    public async Task<LetterTemplate> GetTemplate(LetterType type)
    {
      return await Execute(new GetLetterTemplateByTypeCommand(type));
    }

    public async Task UpdateTemplate(LetterTemplate template, Guid id, string userName)
    {
      await Execute(new UpdateLetterTemplateCommand(template, id, userName));
    }

    public async Task<Letter> GetLetterByID(long letterID)
    {
      return await Execute(new GetLeterByIDCommand(letterID));
    }
  }
}
