using System;
using System.Collections.Generic;
using System.Text;

namespace Encryption.Model
{
    public class ADSyncModel
    {
        public List<string> exclude { get; set; }
        public string ldapConnectionString { get; set; }
        public string sqlServerConnectionString { get; set; }
        public string binddn { get; set; }
        public string bindpassword { get; set; }
        public string searchBase { get; set; }
        public EncryptionKeys EncryptionKeys { get; set; }
    }
}