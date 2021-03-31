using System;
using System.Linq;
using System.Threading.Tasks;
using Core.API.Exceptions;
using Request.API.Repository;
using Request.Model;

namespace Request.API.Strategies.Certificate360
{
  public class HC01Certificate360Strategy : IStrategy<Model.Certificate360>
  {
    readonly DbContext _dbContext;

    readonly Model.Request _request;
    readonly string _certificateNumber;

    public HC01Certificate360Strategy(DbContext dbContext, Model.Request request,
      string certificateNumber)
    {
      _dbContext = dbContext;

      _request = request;
      _certificateNumber = certificateNumber;
    }

    public async Task<Model.Certificate360> Invoke()
    {
      Model.Certificate360 result = null;

      // This assumes there will only be one lineitem for HC01. Else, need to reconsider.
      var lineItem = _request.LineItems?.FirstOrDefault();

      if (lineItem != null)
      {
        var certificate360 =
          await _dbContext.Certificate360.GetCertificateByCertNo(_certificateNumber);

        if (certificate360 == null)
        {
          var premise = _request.Premises?.FirstOrDefault(e => e.IsPrimary);
          if (premise == null)
          {
            throw new BadRequestException("Primary premise not defined.");
          }

          certificate360 = new Model.Certificate360
          {
            Number = _certificateNumber,
            Status = CertificateStatus.Active,
            Scheme = lineItem.Scheme.Value,
            SubScheme = lineItem.SubScheme,
            CustomerID = _request.CustomerID,
            CustomerName = _request.CustomerName,
            RequestorID = _request.RequestorID,
            RequestorName = _request.RequestorName,
            AgentID = _request.AgentID,
            AgentName = _request.AgentName,
            PremiseID = premise.ID
          };

          certificate360.ID = await _dbContext.Certificate360.InsertCertificate360(certificate360);
        }

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
          ApprovedOn = DateTimeOffset.UtcNow,
          CertificateID = certificate360.ID
        };

        await _dbContext.Certificate360.InsertCertificate360History(history);

        await _dbContext.Certificate360.DeleteAllCertificate360Teams(certificate360.ID);

        var teams = _request.Teams?
          .Where(e => e.ChangeType != ChangeType.Delete)?
          .Select(e => e.ID)?
          .ToList();

        await _dbContext.Certificate360.MapCertificate360ToTeams(certificate360.ID, teams);
      }

      return result;
    }
  }
}
