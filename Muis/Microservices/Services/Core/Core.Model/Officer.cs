using System;

namespace Core.Model
{
  public class Officer
  {
    public Officer()
    {
    }

    public Officer(Guid id, string name, string email = null)
    {
      ID = id;
      Name = name;
      Email = email;
    }

    public Guid ID { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public bool IsDeleted { get; set; }
  }
}
