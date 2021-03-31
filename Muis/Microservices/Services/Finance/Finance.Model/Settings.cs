using System;
using Core.Model;

namespace Finance.Model
{
  public enum SettingsType
  {
    GST = 100,
    Stage1Percentage = 200,
  }

  public class Settings : BaseSettings
  {
    public SettingsType Type { get; set; }

    public new string DataType { get; set; }
  }
}
