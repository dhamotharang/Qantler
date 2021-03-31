using System;
using System.Threading.Tasks;
using Core.Model;

namespace JobOrder.API.Repository
{
  public interface IAttachmentRepository
  {
    /// <summary>
    /// Insert attachment.
    /// </summary>
    Task<long> InsertAttachment(Attachment attachment);
  }
}
