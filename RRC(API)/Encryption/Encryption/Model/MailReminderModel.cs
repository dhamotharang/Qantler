using System;
using System.Collections.Generic;
using System.Text;

namespace Encryption.Model
{
    public class MailReminderModel
    {
        public string sqlServerConnectionString { get; set; }
        public string fromAddress { get; set; }
        public string smtpuser { get; set; }
        public string smtppassword { get; set; }
        public string smtphost { get; set; }
        public string SMTPPort { get; set; }
        public string EnableSsl { get; set; }
        public string Url { get; set; }
        public EncryptionKeys EncryptionKeys { get; set; }
    }
}