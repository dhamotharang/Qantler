using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Case.API.Repository;
using Case.Model;
using System.Linq;

namespace Case.API.Services.Commands
{
  public class GetLetterTemplateByTypeCommand : IUnitOfWorkCommand<LetterTemplate>
  {
    readonly LetterType _type;

    public GetLetterTemplateByTypeCommand(LetterType type)
    {
      _type = type;
    }

    public async Task<LetterTemplate> Invoke(IUnitOfWork unitOfWork)
    {
      return (await DbContext.From(unitOfWork).Letter.GetTemplate(_type)).FirstOrDefault();
    }
  }
}
