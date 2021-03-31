using Core.Model;
using System.Threading.Tasks;

namespace JobOrder.API.Repository
{
  public interface ITranslationRepository
  {
    Task<string> GetTranslation(Locale locale, string code);
  }
}
