using System;
using System.Threading.Tasks;
using eHS.Portal.Model;

namespace eHS.Portal.Services.Emails
{
  public interface IEmailService
  {
    /// <summary>
    /// Get request email.
    /// </summary>
    /// <param name="id">the reference email id</param>
    Task<Email> GetRequestEmail(long id);
  }
}
