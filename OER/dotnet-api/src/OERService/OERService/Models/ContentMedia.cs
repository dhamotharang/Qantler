namespace OERService.Models
{
	public class ContentMedia
    {
        public string FileBase64 { get; set; }
        public string FileName { get; set; }
       
    }

    public class TempFilesToDestination
    {
        public string tempObjectName { get; set; }
        public string distObjectName { get; set; }

    }

}
