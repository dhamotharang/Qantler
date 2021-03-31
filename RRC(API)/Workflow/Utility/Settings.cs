namespace Workflow.Utility
{
    public class Settings
    {
        public string ConnectionString { get; set; }
        public string EmailHost { get; set; }
        public int EmailPort { get; set; }
        public bool EnableSsl { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
		public string SMSProviderURL { get; set; }
		public string SMSProviderUserName { get; set; }
		public string SMSProviderPassword { get; set; }
		public int SysLogPort { get; set; }
		public string SysLogHost { get; set; }
        public string UIConfigUrl { get; set; }
    }
}
