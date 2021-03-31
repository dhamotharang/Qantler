using System;

namespace JobOrder.Model
{
  public class Customer
  {
    public Guid ID { get; set; }

    public string? Name { get; set; }

    public string? Code { get; set; }

    public string? GroupCode { get; set; }

    public bool IsDeleted { get; set; }
  }
}
