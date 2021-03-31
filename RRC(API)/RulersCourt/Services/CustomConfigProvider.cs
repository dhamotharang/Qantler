using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Services
{
    public class CustomConfigProvider : ConfigurationProvider, IConfigurationSource
    {
        private IConfiguration configuration;
        private byte[] key;
        private byte[] iV;
        EncryptionService service;

        public CustomConfigProvider(IConfiguration configuration, byte[] key, byte[] iV)
        {
            this.configuration = configuration;
            this.key = key;
            this.iV = iV;
            service = new EncryptionService(new KeyInfo(key, iV));
            UnencryptMyConfiguration();
        }

        private void UnencryptMyConfiguration()
        {
            this.configuration["Logging:LogLevel"] = service.Decrypt_AES(this.configuration["Logging:LogLevel"]);
            this.configuration["Logging:Address"] = service.Decrypt_AES(this.configuration["Logging:Address"]);
            this.configuration["Logging:Port"] = service.Decrypt_AES(this.configuration["Logging:Port"]);

            this.configuration["AllowedHosts"] = service.Decrypt_AES(this.configuration["AllowedHosts"]);
            this.configuration["ConnectionSettings:ConnectionString"] = service.Decrypt_AES(this.configuration["ConnectionSettings:ConnectionString"]);
            this.configuration["ConnectionSettings:AuthSecret"] = service.Decrypt_AES(this.configuration["ConnectionSettings:AuthSecret"]);
            this.configuration["LdapConfig:Url"] = service.Decrypt_AES(this.configuration["LdapConfig:Url"]);
            this.configuration["LdapConfig:BindDn"] = service.Decrypt_AES(this.configuration["LdapConfig:BindDn"]);
            this.configuration["LdapConfig:BindCredentials"] = service.Decrypt_AES(this.configuration["LdapConfig:BindCredentials"]);
            this.configuration["LdapConfig:Domain"] = service.Decrypt_AES(this.configuration["LdapConfig:Domain"]);
            this.configuration["LdapConfig:SearchBase"] = service.Decrypt_AES(this.configuration["LdapConfig:SearchBase"]);
            this.configuration["LdapConfig:AdminCn"] = service.Decrypt_AES(this.configuration["LdapConfig:AdminCn"]);
            this.configuration["EmailSettings:SMTPHost"] = service.Decrypt_AES(this.configuration["EmailSettings:SMTPHost"]);
            this.configuration["EmailSettings:SMTPPort"] = service.Decrypt_AES(this.configuration["EmailSettings:SMTPPort"]);
            this.configuration["EmailSettings:Email"] = service.Decrypt_AES(this.configuration["EmailSettings:Email"]);
            this.configuration["EmailSettings:Password"] = service.Decrypt_AES(this.configuration["EmailSettings:Password"]);
            this.configuration["EmailSettings:EnableSsl"] = service.Decrypt_AES(this.configuration["EmailSettings:EnableSsl"]);
            this.configuration["SMSProvider:SMSProviderUrl"] = service.Decrypt_AES(this.configuration["SMSProvider:SMSProviderUrl"]);
            this.configuration["SMSProvider:SMSProviderUserName"] = service.Decrypt_AES(this.configuration["SMSProvider:SMSProviderUserName"]);
            this.configuration["SMSProvider:SMSProviderPassword"] = service.Decrypt_AES(this.configuration["SMSProvider:SMSProviderPassword"]);
            this.configuration["UIConfig:Url"] = service.Decrypt_AES(this.configuration["UIConfig:Url"]);
            this.configuration["APIConfig:Url"] = service.Decrypt_AES(this.configuration["APIConfig:Url"]);
            this.configuration["NotificationConfig:DeletionDay"] = service.Decrypt_AES(this.configuration["NotificationConfig:DeletionDay"]);
            this.configuration["WrdUserLoginCredentials:UserName"] = service.Decrypt_AES(this.configuration["WrdUserLoginCredentials:UserName"]);
            this.configuration["WrdUserLoginCredentials:Password"] = service.Decrypt_AES(this.configuration["WrdUserLoginCredentials:Password"]);            
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return null;
        }
    }
}
