using System;
using Newtonsoft.Json;

namespace Core.Model
{
  public enum LogType
  {
    Default,
    Request,
    RFA,
    JobOrder
  }

  public class Log
  {
    public long ID { get; set; }

    public LogType Type { get; set; }

    public string RefID { get; set; }

    public string Action { get; set; }

    public string Notes { get; set; }

    public Guid UserID { get; set; }

    public string UserName { get; set; }

    string _raw;
    public string Raw
    {
      get => _params != null ? JsonConvert.SerializeObject(_params) : null;
      set
      {
        _raw = value;
        _params = null;
      }
    }

    string[] _params;
    public string[] Params
    {
      get
      {
        if (_params == null
            && _raw != null)
        {
          _params = JsonConvert.DeserializeObject<string[]>(_raw);
        }
        return _params;
      }
      set
      {
        _params = value;
      }
    }

    public DateTimeOffset CreatedOn { get; set; }
  }
}
