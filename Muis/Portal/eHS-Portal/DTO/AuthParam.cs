using System;
using eHS.Portal.Model;

namespace eHS.Portal.DTO
{
  public class AuthParam
  {
    public Provider Provider { get; set; }

    public string Key { get; set; }

    public string Secret { get; set; }

    public string NewSecret { get; set; }
  }
}
