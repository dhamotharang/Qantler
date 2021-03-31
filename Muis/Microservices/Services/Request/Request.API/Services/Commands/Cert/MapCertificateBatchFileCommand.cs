using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Core.Model;
using Request.API.Repository;

namespace Request.API.Services.Commands.Cert
{
  public class MapCertificateBatchFileCommand : IUnitOfWorkCommand<Unit>
  {
    readonly long _batchID;
    readonly Guid _fileID;

    public MapCertificateBatchFileCommand(long batchID, Guid fileID)
    {
      _batchID = batchID;
      _fileID = fileID;
    }

    public async Task<Unit> Invoke(IUnitOfWork unitOfWork)
    {
      await DbContext.From(unitOfWork).Certificate.MapCertificateBatchToFile(_batchID, _fileID);
      return Unit.Default;
    }
  }
}
