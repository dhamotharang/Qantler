using System;
using System.Collections.Generic;
using System.Text;

namespace eHS.Portal.Model
{
  public enum SettingsDataType
  {
    String,
    Int,
    Double
  }

  public class BaseSettings
  {
    public long ID { get; set; }

    public string Value { get; set; }

    public string Text { get; set; }

    public SettingsDataType DataType { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public IList<Log> Logs { get; set; }
  }
}
