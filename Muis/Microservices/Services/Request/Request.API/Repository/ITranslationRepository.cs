using System;
using System.Threading.Tasks;
using Core.Model;

namespace Request.API.Repository
{
  public interface ITranslationRepository
  {
    Task<string> GetTranslation(Locale locale, string code);
  }
}
