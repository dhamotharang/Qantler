using System;
using System.Threading.Tasks;
using Core.Model;

namespace Request.API.Repository
{
  public interface IAttachmentRepository
  {
    /// <summary>
    /// Insert attachment.
    /// </summary>
    Task<long> InsertAttachment(Attachment attachment);
  }
}
