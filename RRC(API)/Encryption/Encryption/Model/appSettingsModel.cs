
using System.Collections.Generic;

public class Logging
{
    public string LogLevel { get; set; }
    public string Address { get; set; }
    public string Port { get; set; }
}

public class ConnectionSettings
{
    public string ConnectionString { get; set; }
    public string AuthSecret { get; set; }
}

public class LdapConfig
{
    public string Url { get; set; }
    public string BindDn { get; set; }
    public string BindCredentials { get; set; }
    public string Domain { get; set; }
    public string SearchBase { get; set; }
    public string AdminCn { get; set; }
}

public class EmailSettings
{
    public string SMTPHost { get; set; }
    public string SMTPPort { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string EnableSsl { get; set; }
}

public class SMSProvider
{
    public string SMSProviderUrl { get; set; }
    public string SMSProviderUserName { get; set; }
    public string SMSProviderPassword { get; set; }
}

public class UIConfig
{
    public string Url { get; set; }
}

public class APIConfig
{
    public string Url { get; set; }
}
public class NotificationConfig
{
    public string DeletionDay { get; set; }
}
public class WrdUserLoginCredentials
{
    public string UserName { get; set; }
    public string Password { get; set; }
}
public class EncryptionKeys
{
    public string Key { get; set; }
    public string IV { get; set; }
}

public class AppSettings
{
    public Logging Logging { get; set; }
    public string AllowedHosts { get; set; }
    public ConnectionSettings ConnectionSettings { get; set; }
    public LdapConfig LdapConfig { get; set; }
    public EmailSettings EmailSettings { get; set; }
    public SMSProvider SMSProvider { get; set; }
    public UIConfig UIConfig { get; set; }
    public APIConfig APIConfig { get; set; }
    public NotificationConfig NotificationConfig { get; set; }
    public WrdUserLoginCredentials WrdUserLoginCredentials { get; set; }
    public EncryptionKeys EncryptionKeys { get; set; }
}

