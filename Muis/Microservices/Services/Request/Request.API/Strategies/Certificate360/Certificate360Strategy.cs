using System;
using System.Threading.Tasks;
using Core.Model;
using Request.API.Repository;
using Request.Model;

namespace Request.API.Strategies.Certificate360
{
  public class Certificate360Strategy : IStrategy<Model.Certificate360>
  {
    readonly DbContext _dbContext;

    readonly Model.Request _request;
    readonly Certificate _certificate;

    readonly Officer _approvedBy;

    public Certificate360Strategy(DbContext dbContext, Model.Request request,
      Certificate certificate, Officer approvedBy)
    {
      _dbContext = dbContext;

      _request = request;
      _certificate = certificate;

      _approvedBy = approvedBy;
    }

    public Task<Model.Certificate360> Invoke()
    {
      switch(_request.Type)
      {
        case RequestType.New:
        case RequestType.Renewal:
          return new NewCertificate360Strategy(_dbContext, _request, _certificate, _approvedBy)
            .Invoke();

        case RequestType.HC01:
          return new HC01Certificate360Strategy(_dbContext, _request, _certificate.Number).Invoke();

        case RequestType.HC03:
          return new HC03Certificate360Strategy(_dbContext, _request, _certificate, _approvedBy)
            .Invoke();
      }

      return null;
    }
  }
}
