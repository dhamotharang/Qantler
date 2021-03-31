using System.Collections.Generic;

namespace OERService.Helpers
{
	public class EmailHelperService
    {
        public List<string> To { get; set; }
        public string From { get; set; }
        public string MessageBody { get; set; }
        public string SmtpClient { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Subject { get; set; }
        public int SmtpPort { get; set; }
    }
    public class EmailSettings
    {
        public string SmtpClient { get; set; }


        public string Port { get; set; }

        public string emailusername { get; set; }
        public string emailpassword { get; set; }

    }
}
