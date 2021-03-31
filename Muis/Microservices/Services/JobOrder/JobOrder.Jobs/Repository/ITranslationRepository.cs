using Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JobOrder.Jobs.Repository
{
  public interface ITranslationRepository
  {
    Task<string> GetTranslation(Locale locale, string code);
  }
}
