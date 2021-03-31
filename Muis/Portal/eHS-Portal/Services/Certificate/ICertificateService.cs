using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eHS.Portal.Client;
using eHS.Portal.Model;

namespace eHS.Portal.Services.Certificate
{
  public interface ICertificateService
  {
    /// <summary>
    /// Retrieve certificate batch with specified options.
    /// </summary>
    Task<IList<CertificateBatch>> GetCertificateBatch(CertificateBatchOptions options,
      bool includeAll = false);

    /// <summary>
    /// Retrieve certificates with specified options.
    /// </summary>
    Task<IList<Model.Certificate>> GetCertificates(CertificateOptions options);

    /// <summary>
    /// Retrieve certificate batch based on specified ID.
    /// </summary>
    Task<CertificateBatch> GetCertificateBatchByID(long id, bool full);

    /// <summary>
    /// Retrieve certificate batch comments based on specified ID.
    /// </summary>
    Task<IList<Comment>> GetCertificateBatchComments(long id);

    /// <summary>
    /// Add comment to batch.
    /// </summary>
    Task<Comment> AddComment(long batchID, string text);

    /// <summary>
    /// Acknowledge certificate batch.
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task AcknowledgeCertificateBatch(long[] ids);

    /// <summary>
    /// Maps file to certificate batch.
    /// </summary>
    Task MapFileToCertificateBatch(long batchID, Guid fileID);

    /// <summary>
    /// Update certificate batch status.
    /// </summary>
    Task UpdateCertificateBatchStatus(long batchID, CertificateBatchStatus status);

    /// <summary>
    /// Get delivery certificates list
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    Task<IList<Model.Certificate>> CertDeliveryFilter(Client.CertificateDeliveryOptions options);

    /// <summary>
    /// Execute certificate delivery
    /// </summary>
    /// <param name="IDs"></param>
    /// <returns></returns>
    Task<string> ExecuteCertDelivery(long[] IDs);
  }
}
