using System;
using Identity.Model;

namespace Identity.API.Dto
{
  public class AuthParam
  {
    public Provider Provider { get; set; }

    public string Key { get; set; }

    public string Secret { get; set; }

    public string NewSecret { get; set; }
  }
}
