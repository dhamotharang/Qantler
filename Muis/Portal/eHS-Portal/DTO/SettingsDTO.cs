using eHS.Portal.Model;
using System.Collections.Generic;

namespace eHS.Portal.DTO
{
  public class SettingsDTO
  {
    public List<Settings> RequestSettings { get; set; }

    public List<Settings> JobOrderSettings { get; set; }
  }
}
