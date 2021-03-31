namespace File.API.Config
{
  public enum FileStrategy
  {
    Local,
    Azure,
    S3
  }

  public class FileConfig
  {
    public FileStrategy Strategy { get; set; }

    public string Root { get; set; }

    public string Key { get; set; }

    public string Secret { get; set; }

    public string Region { get; set; }
  }
}
