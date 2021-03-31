using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eHS.Portal.Client;
using eHS.Portal.Model;

namespace eHS.Portal.Services.Certificate
{
  public class CertificateService : ICertificateService
  {
    readonly ApiClient _apiClient;

    public CertificateService(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    public async Task<IList<CertificateBatch>> GetCertificateBatch(CertificateBatchOptions options,
      bool includeAll = false)
    {
      return await _apiClient.CertificateSdk.GetCertificateBatch(options, includeAll);
    }

    public async Task<CertificateBatch> GetCertificateBatchByID(long id, bool full)
    {
      return await _apiClient.CertificateSdk.GetCertificateBatchByID(id, full);
    }

    public async Task<Comment> AddComment(long batchID, string text)
    {
      return await _apiClient.CertificateSdk.AddComment(batchID, text);
    }

    public async Task<IList<Comment>> GetCertificateBatchComments(long id)
    {
      return await _apiClient.CertificateSdk.GetCertificateBatchComments(id);
    }

    public async Task AcknowledgeCertificateBatch(long[] ids)
    {
      await _apiClient.CertificateSdk.AcknowledgeBatch(ids);
    }

    public async Task MapFileToCertificateBatch(long batchID, Guid fileID)
    {
      await _apiClient.CertificateSdk.MapFileToCertificateBatch(batchID, fileID);
    }

    public async Task UpdateCertificateBatchStatus(long batchID, CertificateBatchStatus status)
    {
      await _apiClient.CertificateSdk.UpdateCertificateBatchStatus(batchID, status);
    }

    public async Task<IList<Model.Certificate>> GetCertificates(CertificateOptions options)
    {
      return await _apiClient.CertificateSdk.GetCertificates(options);
    }

    public async Task<IList<Model.Certificate>> CertDeliveryFilter(Client.CertificateDeliveryOptions options)
    {
      return await _apiClient.CertificateSdk.GetDeliveryCertificates(options);
    }

    public async Task<string> ExecuteCertDelivery(long[] IDs)
    {
      return await _apiClient.CertificateSdk.ExecuteCertDelivery(IDs);
    }
  }
}
