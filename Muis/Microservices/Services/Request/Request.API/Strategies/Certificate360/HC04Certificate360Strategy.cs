using Core.Model;
using Request.API.Repository;
using Request.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Request.API.Strategies.Certificate360
{
  public class HC04Certificate360Strategy : IStrategy<Model.Certificate360>
  {
    readonly DbContext _dbContext;

    readonly Model.Request _request;
    readonly string _certificateNumber;

    readonly Officer _approvedBy;
    public HC04Certificate360Strategy(DbContext dbContext, Model.Request request,
      string certificateNumber, Officer approvedBy)
    {
      _dbContext = dbContext;

      _request = request;
      _certificateNumber = certificateNumber;

      _approvedBy = approvedBy;
    }

    public async Task<Model.Certificate360> Invoke()
    {
      var certificate360 =
         await _dbContext.Certificate360.GetCertificateByCertNo(_certificateNumber);

      var history = new Certificate360History
      {
        RequestID = _request.ID,
        RefID = _request.RefID,
        RequestorID = _request.RequestorID,
        RequestorName = _request.RequestorName,
        AgentID = _request.AgentID,
        AgentName = _request.AgentName,
        IssuedOn = certificate360.IssuedOn,
        ExpiresOn = certificate360.ExpiresOn,
        SerialNo = certificate360.SerialNo,
        ApprovedBy = _approvedBy?.ID,
        ApprovedOn = DateTimeOffset.UtcNow,
        CertificateID = certificate360.ID
      };

      await _dbContext.Certificate360.InsertCertificate360History(history);

      await _dbContext.Certificate360.DeleteAllCertificate360Menus(certificate360.ID);
      await _dbContext.Certificate360.DeleteAllCertificate360Ingredients(certificate360.ID);
      await _dbContext.Certificate360.DeleteAllCertificate360Teams(certificate360.ID);

      var menus = (await _dbContext.Menu.Query(new MenuFilter
      {
        RequestID = _request.ID,
        Scheme = certificate360.Scheme
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

      var teams = _request.Teams?
        .Where(e => e.ChangeType != ChangeType.Delete)?
        .Select(e => e.ID)?
        .ToList();

      await _dbContext.Certificate360.MapCertificate360ToTeams(certificate360.ID, teams);

      return certificate360;
    }
  }
}
