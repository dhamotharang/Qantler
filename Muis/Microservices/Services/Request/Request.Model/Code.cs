using System;
namespace Request.Model
{
  public enum CodeType
  {
    Code,
    Group
  }

  public class Code
  {
    public long ID { get; set; }

    public CodeType Type { get; set; }

    public string Value { get; set; }

    public string Text { get; set; }

    public string BillingCycle { get; set; }

    public DateTimeOffset? CertificateExpiry { get; set; }
  }
}
