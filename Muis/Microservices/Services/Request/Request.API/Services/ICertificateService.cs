using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Model;
using Request.API.Repository;
using Request.Model;

namespace Request.API.Services
{
  public interface ICertificateService
  {
    /// <summary>
    /// Query certificate by specified filter.
    /// </summary>
    /// <param name="filter">the filter options</param>
    /// <returns>the batch</returns>
    Task<IList<Certificate>> CertificateFilter(CertificateFilter filter);

    /// <summary>
    /// Query certificate batch by specified filter.
    /// </summary>
    /// <param name="filter">the filter options</param>
    /// <returns>the batch</returns>
    Task<IList<CertificateBatch>> BatchFilter(CertificateBatchFilter filter, bool includeAll);

    /// <summary>
    /// Get certificate batch with specified ID.
    /// </summary>
    /// <param name="id">the reference ID</param>
    Task<CertificateBatch> GetCertificateBatchByID(long id, bool includeAll);

    /// <summary>
    /// Retrieve certificate batch comments.
    /// </summary>
    Task<IList<Comment>> GetCertificateBatchComments(long id);

    /// <summary>
    /// Acknowledge certificate batch.
    /// </summary>
    /// <param name="id">the batch ID</param>
    Task AcknowledgeCertificateBatch(long[] ids, Officer user);

    /// <summary>
    /// Add comment to batch.
    /// </summary>
    Task<Comment> AddComment(long id, string text, Officer user);

    /// <summary>
    /// Maps a file ID to certificate batch.
    /// </summary>
    Task MapCertificateBatchFile(long batchID, Guid fileID);

    /// <summary>
    /// Updates certificate batch status.
    /// </summary>
    Task UpdateCertificateBatchStatus(long batchID, CertificateBatchStatus status);

    /// <summary>
    /// Get certificates ready to deliver
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    Task<IList<Certificate>> CertDeliveryFilter(CertificateDeliveryFilter filter);

    /// <summary>
    /// Update certificate delivery status
    /// </summary>
    /// <param name="IDs"></param>
    /// <returns></returns>
    Task UpdateCertificateDeliveryStatus(long[] IDs);
  }
}
