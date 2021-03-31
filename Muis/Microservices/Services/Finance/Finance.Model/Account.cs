using System;

namespace Finance.Model
{
  public class Account
  {
    public Guid ID { get; set; }

    public string Name { get; set; }

    public decimal Balance { get; set; }
  }
}
