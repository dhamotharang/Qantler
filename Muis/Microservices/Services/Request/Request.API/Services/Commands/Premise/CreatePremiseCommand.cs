using Core.API;
using Core.API.Exceptions;
using Core.API.Repository;
using Core.Model;
using Request.API.Repository;
using System.Linq;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Premises
{
  public class CreatePremiseCommand : IUnitOfWorkCommand<Model.Premise>
  {
    readonly Model.Premise _premise;

    public CreatePremiseCommand(Model.Premise premise)
    {
      _premise = premise;
    }

    public async Task<Model.Premise> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      var premises = await dbContext.Premises.Select();

      if (premises.Any(x => x.FloorNo == _premise.FloorNo
        && x.UnitNo == _premise.UnitNo
        && x.Postal == _premise.Postal))
      {
        var errorText = await dbContext.Transalation.GetTranslation(Locale.EN, "ValidatePremise");

        throw new BadRequestException(errorText);
      }

      _premise.ID = await dbContext.Premises.CreatePremise(_premise);

      var result = await dbContext.Premises.GetByID(_premise.ID);

      unitOfWork.Commit();

      return result;
    }
  }
}
