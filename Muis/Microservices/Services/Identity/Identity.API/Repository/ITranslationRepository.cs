using System;
using System.Threading.Tasks;
using Core.Model;

namespace Identity.API.Repository
{
  public interface ITranslationRepository
  {
    Task<string> GetTranslation(Locale locale, string code);
  }
}
