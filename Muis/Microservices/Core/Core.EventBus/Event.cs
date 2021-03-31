using Newtonsoft.Json;
using System;

namespace Core.EventBus
{
  public class Event
  {
    public string Id { get; private set; }
    public DateTime CreationDate { get; private set; }

    public Event()
    {
      Id = Guid.NewGuid().ToString();
      CreationDate = DateTime.UtcNow;
    }

    public Event(Guid id, DateTime createDate)
    {
      Id = id.ToString();
      CreationDate = createDate;
    }
  }
}
