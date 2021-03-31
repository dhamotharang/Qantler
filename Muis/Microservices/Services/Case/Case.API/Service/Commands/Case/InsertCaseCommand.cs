using Case.API.Repository;
using Core.API;
using Core.API.Repository;
using Core.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case
{
  public class InsertCaseCommand : IUnitOfWorkCommand<long>
  {
    readonly Model.Case _data;
    readonly Officer _user;

    public InsertCaseCommand(Model.Case data, Officer user)
    {
      _data = data;
      _user = user;
    }

    public async Task<long> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      _data.CreatedBy = _user;
      _data.ManagedBy = _user;

      await dbContext.User.InsertOrReplace(_user);

      if (_data.Offender != null
        && await dbContext.Offender.GetOffenderByID(_data.Offender.ID) == null)
      {
        await dbContext.Offender.InsertOffender(_data.Offender);
      }

      if (_data.ReportedBy != null)
      {
        _data.ReportedBy.ID = await dbContext.Reporter.InsertReporter(_data.ReportedBy);
      }

      _data.ID = await dbContext.Case.InsertCase(_data);

      await dbContext.Case.MapCaseOffence(_data.Offences, _data.ID);

      await dbContext.Case.MapCaseBreachByOffence(_data.Offences, _data.ID);

      foreach (var premise in _data.Premises)
      {
        await dbContext.Premise.InsertPremise(premise);
      }

      if (_data.Certificates?.Any() ?? false)
      {
        _data.Certificates.ToList().ForEach(x => x.CaseID = _data.ID);
        foreach (var certificate in _data.Certificates)
        {
          await dbContext.Certificate.InsertCertificate(certificate);
        }
      }

      await dbContext.Case.MapCasePremise(
        _data.Premises.Select(e => e.ID).ToArray()
        , _data.ID);

      if (_data.Attachments?.Any() ?? false)
      {
        foreach (var attachment in _data.Attachments)
        {
          attachment.ID = await dbContext.Attachment.InsertAttachment(attachment);
        }

        await dbContext.Case.MapCaseAttachments(
          _data.ID,
          _data.Attachments.Select(e => e.ID).ToArray());
      }

      unitOfWork.Commit();
      return _data.ID;
    }
  }
}
