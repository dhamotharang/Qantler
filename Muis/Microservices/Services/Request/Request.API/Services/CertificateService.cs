using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Provider;
using Core.EventBus;
using Core.Model;
using Request.API.Repository;
using Request.API.Services.Commands.Cert;
using Request.Model;

namespace Request.API.Services
{
  public class CertificateService : TransactionalService,
                                    ICertificateService
  {
    readonly IEventBus _eventBus;

    public CertificateService(IDbConnectionProvider connectionProvider, IEventBus eventBus)
         : base(connectionProvider)
    {
      _eventBus = eventBus;
    }

    public async Task<IList<Certificate>> CertificateFilter(CertificateFilter filter)
    {
      return await Execute(new CertificateFilterCommand(filter));
    }

    public async Task<IList<CertificateBatch>> BatchFilter(CertificateBatchFilter filter,
      bool includeAll)
    {
      return await Execute(new CertBatchFilterCommand(filter, includeAll));
    }

    public async Task<CertificateBatch> GetCertificateBatchByID(long id, bool includeAll)
    {
      return await Execute(new GetCertByIDCommand(id, includeAll));
    }

    public async Task AcknowledgeCertificateBatch(long[] ids, Officer user)
    {
      await Execute(new AcknowledgeCertificateBatchCommand(ids, user, _eventBus));
    }

    public async Task<Comment> AddComment(long id, string text, Officer user)
    {
      return await Execute(new AddCommentCommand(id, text, user, _eventBus));
    }

    public async Task<IList<Comment>> GetCertificateBatchComments(long id)
    {
      return await Execute(new GetCertificateBatchCommentsCommand(id));
    }

    public async Task MapCertificateBatchFile(long batchID, Guid fileID)
    {
      await Execute(new MapCertificateBatchFileCommand(batchID, fileID));
    }

    public async Task UpdateCertificateBatchStatus(long batchID, CertificateBatchStatus status)
    {
      await Execute(new UpdateCertificateBatchStatusCommand(batchID, status));
    }

    public async Task<IList<Certificate>> CertDeliveryFilter(CertificateDeliveryFilter filter)
    {
      return await Execute(new CertDeliveryFilterCommand(filter));
    }

    public async Task UpdateCertificateDeliveryStatus(long[] IDs)
    {
      await Execute(new UpdateCertificateDeliveryStatusCommand(IDs));
    }

  }
}
