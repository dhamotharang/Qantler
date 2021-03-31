using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Model;
using Request.Model;

namespace Request.API.Repository
{
  public interface ICertificateRepository
  {
    /// <summary>
    /// Query certificate by specified filter.
    /// </summary>
    /// <param name="filter">the filter options</param>
    Task<IList<Certificate>> CertificateFilter(CertificateFilter filter);

    /// <summary>
    /// Query certificate batch by specified filter.
    /// </summary>
    /// <param name="filter">the filter options</param>
    /// <returns>the batch</returns>
    Task<IList<CertificateBatch>> BatchFilter(CertificateBatchFilter filter);

    /// <summary>
    /// Query certificate batch by specified filter.
    /// </summary>
    /// <param name="filter">the filter options</param>
    /// <returns>the batch</returns>
    Task<IList<CertificateBatch>> BatchFullFilter(CertificateBatchFilter filter);

    /// <summary>
    /// Get certificate batch with specified ID.
    /// </summary>
    /// <param name="id">the reference ID</param>
    Task<CertificateBatch> GetCertificateBatchByID(long id);

    /// <summary>
    /// Get certificate batch full details with specified ID.
    /// </summary>
    /// <param name="id">the reference ID</param>
    Task<CertificateBatch> GetCertificateBatchByIDFull(long id);

    /// <summary>
    /// Retrieve certificate batch with specified code.
    /// </summary>
    Task<CertificateBatch> GetCertificateBatchByCode(string code);

    /// <summary>
    /// Insert certificate batch.
    /// </summary>
    Task<long> InsertCertificateBatch(CertificateBatch batch);

    /// <summary>
    /// Get certificate batch basic details with specified ID.
    /// </summary>
    /// <param name="id">the reference ID</param>
    Task<CertificateBatch> GetCertificateBatchByIDBasic(long id);

    /// <summary>
    /// Insert comment to batch.
    /// </summary>
    Task<long> InsertComment(long batchID, string text, Officer officer);

    /// <summary>
    /// Retrieve comment with specified ID.
    /// </summary>
    Task<Comment> GetCommentByID(long id);

    /// <summary>
    /// Get certificate batch comments.
    /// </summary>
    Task<IList<Comment>> GetCertificateBatchComments(long id);

    /// <summary>
    /// Execute acknowledge certificate batch.
    /// </summary>
    Task ExecAcknowledgeBatch(long batchID, Officer user);

    /// <summary>
    /// Generates certificate sequence no.
    /// </summary>
    Task<int> GenerateCertificateSequence();

    /// <summary>
    /// Generates certificate serial no.
    /// </summary>
    /// <returns></returns>
    Task<int> GenerateCertificateSerialNo();

    /// <summary>
    /// Retrieve certificate 360 with specified certificate no.
    /// </summary>
    Task<Certificate360> GetCertificate360ByNo(string certificateNo);

    /// <summary>
    /// Insert certificate.
    /// </summary>
    Task<long> InsertCertificate(Certificate certificate);

    /// <summary>
    /// To map generated certificate.
    /// </summary>
    Task MapCertificate(long batchID, long requestID, Scheme scheme, long certificateID);

    /// <summary>
    /// Maps file ID to certificate batch.
    /// </summary>
    Task MapCertificateBatchToFile(long batchID, Guid fileID);

    /// <summary>
    /// Update certificate batch status.
    /// </summary>
    Task UpdateCertificateBatchStatus(long batchID, CertificateBatchStatus status);

    /// <summary>
    /// Get certificate for delivery
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    Task<IList<Certificate>> CertDeliveryFilter(CertificateDeliveryFilter filter);

    /// <summary>
    /// Update Certificate delivery status
    /// </summary>
    /// <param name="certificateID"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    Task UpdateCertificateDeliveryStatus(long certificateID,
      CertificateDeliveryStatus status);

    /// <summary>
    /// Retrieve certificate by request ID.
    /// </summary>
    Task<Certificate> GetByRequestID(long requestID);
  }

  public class CertificateBatchFilter
  {
    public long? RequestID { get; set; }

    public DateTimeOffset From { get; set; }

    public DateTimeOffset To { get; set; } = DateTimeOffset.UtcNow.Date;

    public IList<CertificateBatchStatus> Status { get; set; }
  }

  public class CertificateFilter
  {
    public long[] IDs { get; set; }

    public long? PremiseID { get; set; }
  }

  public class CertificateDeliveryFilter
  {
    public string CustomerCode { get; set; }

    public string CustomerName { get; set; }

    public string Premise { get; set; }

    public string Postal { get; set; }

    public DateTimeOffset? IssuedOnFrom { get; set; }

    public DateTimeOffset? IssuedOnTo { get; set; }

    public IList<CertificateDeliveryStatus> Status { get; set; }

    public string SerialNo { get; set; }
  }
}
