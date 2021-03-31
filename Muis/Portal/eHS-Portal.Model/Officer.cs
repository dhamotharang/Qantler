using System;

namespace eHS.Portal.Model
{
  public class Officer
  {
    public Officer()
    {
    }

    public Officer(Guid id, string name)
    {
      ID = id;
      Name = name;
    }

    public Guid ID { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public bool IsDeleted { get; set; }
  }
}
