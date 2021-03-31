using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Model;
using Request.API.Repository;
using Request.Model;

namespace Request.API.Strategies.Certificate360
{
  public class HC03Certificate360Strategy : IStrategy<Model.Certificate360>
  {
    readonly DbContext _dbContext;

    readonly Model.Request _request;
    readonly Certificate _certificate;

    readonly Officer _approvedBy;

    public HC03Certificate360Strategy(DbContext dbContext, Model.Request request,
      Certificate certificate, Officer approvedBy)
    {
      _dbContext = dbContext;

      _request = request;
      _certificate = certificate;

      _approvedBy = approvedBy;
    }

    public async Task<Model.Certificate360> Invoke()
    {
      var certificate360 =
        await _dbContext.Certificate360.GetCertificateByCertNo(_certificate.Number)
        ?? new Model.Certificate360();

      certificate360.Number = _certificate.Number;
      certificate360.Status = CertificateStatus.Active;
      certificate360.Template = _certificate.Template;
      certificate360.Scheme = _certificate.Scheme;
      certificate360.SubScheme = _certificate.SubScheme;
      certificate360.IssuedOn = _certificate.IssuedOn;
      certificate360.ExpiresOn = _certificate.ExpiresOn;
      certificate360.SerialNo = _certificate.SerialNo;
      certificate360.CustomerID = _request.CustomerID;
      certificate360.CustomerName = _request.CustomerName;
      certificate360.RequestorID = _request.RequestorID;
      certificate360.RequestorName = _request.RequestorName;
      certificate360.AgentID = _request.AgentID;
      certificate360.AgentName = _request.AgentName;
      certificate360.PremiseID = _certificate.PremiseID;

      // Only case where existing certifcate is not yet available.
      // Probably from old data.
      if (certificate360.ID == 0)
      {
        certificate360.ID = await _dbContext.Certificate360.InsertCertificate360(certificate360);
      }
      else
      {
        await _dbContext.Certificate360.UpdateCertificate360(certificate360);
      }

      var history = new Certificate360History
      {
        RequestID = _request.ID,
        RefID = _request.RefID,
        RequestorID = _request.RequestorID,
        RequestorName = _request.RequestorName,
        AgentID = _request.AgentID,
        AgentName = _request.AgentName,
        IssuedOn = _certificate.IssuedOn,
        ExpiresOn = _certificate.ExpiresOn,
        SerialNo = _certificate.SerialNo,
        ApprovedBy = _approvedBy?.ID,
        ApprovedOn = DateTimeOffset.UtcNow,
        CertificateID = certificate360.ID
      };

      await _dbContext.Certificate360.InsertCertificate360History(history);

      var menus = (await _dbContext.Menu.Query(new MenuFilter
      {
        RequestID = _request.ID,
        Scheme = _certificate.Scheme
      }))?
      .Where(e => e.ChangeType != ChangeType.Delete)?
      .Select(e => e.ID)?
      .ToList();

      await _dbContext.Certificate360.MapCertificate360ToMenus(certificate360.ID, menus);

      var ingredients = (await _dbContext.Ingredient.Query(
          new IngredientFilter { RequestID = _request.ID }))?
        .Where(e => e.ChangeType != ChangeType.Delete)?
        .Select(e => e.ID)?
        .ToList();

      await _dbContext.Certificate360.MapCertificate360ToIngredients(certificate360.ID,
        ingredients);


      return certificate360;
    }
  }
}
