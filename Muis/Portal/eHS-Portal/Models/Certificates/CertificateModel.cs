using System;
using System.Collections.Generic;
using eHS.Portal.Model;

namespace eHS.Portal.Models.Certificates
{
  public class CertificateModel
  {
    public bool IsPreview { get; set; }

    public string Title { get; set; }

    public IList<CertificateHolder> Certificates { get; set; }
  }

  public class CertificateHolder
  {
    public CertificateTemplate Template { get; set; }

    public string Number { get; set; }

    public string SerialNo { get; set; }

    public string CustomerName { get; set; }

    public string Scheme { get; set; }

    public string SubScheme { get; set; }

    public DateTimeOffset? ExpiresOn { get; set; }

    public DateTimeOffset? IssuedOn { get; set; }

    public Premise Premise { get; set; }

    public IList<ItemGroup> ItemGroups { get; set; }
  }

  public class ItemGroup
  {
    public int StartIndex { get; set; }

    public IList<string> Items { get; set; } = new List<string>();
  }
}
