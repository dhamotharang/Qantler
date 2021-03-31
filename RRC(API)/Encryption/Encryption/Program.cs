using Encryption.Model;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Encryption
{
    public class Program
    {
        public static byte[] key;
        public static byte[] iv;
        public static void Main(string[] args)
        {
            try { 
            Console.WriteLine("Encryption Started");
            string path = Path.Combine(Directory.GetCurrentDirectory(), "RulersCourt.json");
            string jsonobj = File.ReadAllText(path);
            //Deserialize from file to object:
            var appSettings = new AppSettings();
            JsonConvert.PopulateObject(jsonobj, appSettings);

            key = Convert.FromBase64String(appSettings.EncryptionKeys.Key);
            iv = Convert.FromBase64String(appSettings.EncryptionKeys.IV);

            //Change Value
            appSettings.Logging.LogLevel = Encrypt_Aes(appSettings.Logging.LogLevel);
            appSettings.Logging.Address = Encrypt_Aes(appSettings.Logging.Address);
            appSettings.Logging.Port = Encrypt_Aes(appSettings.Logging.Port);
            appSettings.AllowedHosts = Encrypt_Aes(appSettings.AllowedHosts);
            appSettings.ConnectionSettings.ConnectionString = Encrypt_Aes(appSettings.ConnectionSettings.ConnectionString);
            appSettings.ConnectionSettings.AuthSecret = Encrypt_Aes(appSettings.ConnectionSettings.AuthSecret);
            appSettings.LdapConfig.Url = Encrypt_Aes(appSettings.LdapConfig.Url);
            appSettings.LdapConfig.BindDn = Encrypt_Aes(appSettings.LdapConfig.BindDn);
            appSettings.LdapConfig.BindCredentials = Encrypt_Aes(appSettings.LdapConfig.BindCredentials);
            appSettings.LdapConfig.Domain = Encrypt_Aes(appSettings.LdapConfig.Domain);
            appSettings.LdapConfig.SearchBase = Encrypt_Aes(appSettings.LdapConfig.SearchBase);
            appSettings.LdapConfig.AdminCn = Encrypt_Aes(appSettings.LdapConfig.AdminCn);
            appSettings.EmailSettings.SMTPHost = Encrypt_Aes(appSettings.EmailSettings.SMTPHost);
            appSettings.EmailSettings.SMTPPort = Encrypt_Aes(appSettings.EmailSettings.SMTPPort);
            appSettings.EmailSettings.Email = Encrypt_Aes(appSettings.EmailSettings.Email);
            appSettings.EmailSettings.Password = Encrypt_Aes(appSettings.EmailSettings.Password);
            appSettings.EmailSettings.EnableSsl = Encrypt_Aes(appSettings.EmailSettings.EnableSsl);
            appSettings.SMSProvider.SMSProviderUrl = Encrypt_Aes(appSettings.SMSProvider.SMSProviderUrl);
            appSettings.SMSProvider.SMSProviderUserName = Encrypt_Aes(appSettings.SMSProvider.SMSProviderUserName);
            appSettings.SMSProvider.SMSProviderPassword = Encrypt_Aes(appSettings.SMSProvider.SMSProviderPassword);
            appSettings.UIConfig.Url = Encrypt_Aes(appSettings.UIConfig.Url);
            appSettings.APIConfig.Url = Encrypt_Aes(appSettings.APIConfig.Url);
            appSettings.NotificationConfig.DeletionDay = Encrypt_Aes(appSettings.NotificationConfig.DeletionDay);
            appSettings.WrdUserLoginCredentials.UserName = Encrypt_Aes(appSettings.WrdUserLoginCredentials.UserName);
            appSettings.WrdUserLoginCredentials.Password = Encrypt_Aes(appSettings.WrdUserLoginCredentials.Password);

            // serialize JSON directly to a file again
            using (StreamWriter file = File.CreateText(Path.Combine(Directory.GetCurrentDirectory(), "RulersCourt.encrypted.json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, appSettings);
            }
            ADSyncEncryption();
            MailReminderEncryption();
            ManageEngineEncryption();
            TimeAttendanceServiceEncryption();
            Console.WriteLine("Encrypted Successfully");
           }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static string Encrypt_Aes(string plainText)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException(nameof(plainText));
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException(nameof(iv));
            byte[] encrypted;

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        public static void ADSyncEncryption()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "ADSync.json");
            string jsonobj = File.ReadAllText(path);
            //Deserialize from file to object:
            var appSettings = new ADSyncModel();
            JsonConvert.PopulateObject(jsonobj, appSettings);

            for(int i=0; i< appSettings.exclude.Count;i++)
            {
                appSettings.exclude[i] = Encrypt_Aes(appSettings.exclude[i]);
            }

            appSettings.ldapConnectionString = Encrypt_Aes(appSettings.ldapConnectionString);
            appSettings.sqlServerConnectionString = Encrypt_Aes(appSettings.sqlServerConnectionString);
            appSettings.binddn = Encrypt_Aes(appSettings.binddn);
            appSettings.bindpassword = Encrypt_Aes(appSettings.bindpassword);
            appSettings.searchBase = Encrypt_Aes(appSettings.searchBase);

            using (StreamWriter file = File.CreateText(Path.Combine(Directory.GetCurrentDirectory(), "ADSync.encrypted.json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, appSettings);
            }
        }

        public static void MailReminderEncryption()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "MailReminder.json");
            string jsonobj = File.ReadAllText(path);
            //Deserialize from file to object:
            var appSettings = new MailReminderModel();
            JsonConvert.PopulateObject(jsonobj, appSettings);

            appSettings.sqlServerConnectionString = Encrypt_Aes(appSettings.sqlServerConnectionString);
            appSettings.fromAddress = Encrypt_Aes(appSettings.fromAddress);
            appSettings.smtpuser = Encrypt_Aes(appSettings.smtpuser);
            appSettings.smtppassword = Encrypt_Aes(appSettings.smtppassword);
            appSettings.smtphost = Encrypt_Aes(appSettings.smtphost);
            appSettings.SMTPPort = Encrypt_Aes(appSettings.SMTPPort);
            appSettings.EnableSsl = Encrypt_Aes(appSettings.EnableSsl);
            appSettings.Url = Encrypt_Aes(appSettings.Url);

            using (StreamWriter file = File.CreateText(Path.Combine(Directory.GetCurrentDirectory(), "MailReminder.encrypted.json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, appSettings);
            }
        }
        
        public static void ManageEngineEncryption()
        {
            ExeConfigurationFileMap configFileMap =
        new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = Path.Combine(Directory.GetCurrentDirectory(), "ManageEngine.config"); // full path to the config file

            // Get the mapped configuration file
            Configuration config =
               ConfigurationManager.OpenMappedExeConfiguration(
                 configFileMap, ConfigurationUserLevel.None);

            //now on use config object


            KeyValueConfigurationCollection getConfCollection = config.AppSettings.Settings;

            ExeConfigurationFileMap configFileRes =        new ExeConfigurationFileMap();
            configFileRes.ExeConfigFilename = Path.Combine(Directory.GetCurrentDirectory(), "ManageEngine.encrypted.config"); 
            Configuration configManager = ConfigurationManager.OpenMappedExeConfiguration(configFileRes, ConfigurationUserLevel.None);
            
            configManager.AppSettings.Settings.Remove("SQLConnectionstring");
            configManager.AppSettings.Settings.Add("SQLConnectionstring", Encrypt_Aes(getConfCollection["SQLConnectionstring"].Value));
            configManager.AppSettings.Settings.Remove("TechnicianKey");
            configManager.AppSettings.Settings.Add("TechnicianKey", Encrypt_Aes(getConfCollection["TechnicianKey"].Value));
            configManager.AppSettings.Settings.Remove("ManageEngineUrl");
            configManager.AppSettings.Settings.Add("ManageEngineUrl", Encrypt_Aes(getConfCollection["ManageEngineUrl"].Value));
            configManager.AppSettings.Settings.Remove("EncryptionKey");
            configManager.AppSettings.Settings.Add("EncryptionKey", getConfCollection["EncryptionKey"].Value);
            configManager.AppSettings.Settings.Remove("EncryptionIV");
            configManager.AppSettings.Settings.Add("EncryptionIV", getConfCollection["EncryptionIV"].Value);


            configManager.Save(ConfigurationSaveMode.Modified, true);
           // configManager.SaveAs(Path.Combine(Directory.GetCurrentDirectory(), "ManageEngine.encrypted.config"), ConfigurationSaveMode.Modified, true);
            ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);
        }


        public static void TimeAttendanceServiceEncryption()
        {
            ExeConfigurationFileMap configFileMapTime = new ExeConfigurationFileMap();
            configFileMapTime.ExeConfigFilename = Path.Combine(Directory.GetCurrentDirectory(), "TimeAttendanceService.config"); // full path to the config file

            // Get the mapped configuration file
            Configuration configTime =
               ConfigurationManager.OpenMappedExeConfiguration(configFileMapTime, ConfigurationUserLevel.None);

            //now on use config object


            KeyValueConfigurationCollection getConfCollectionTime = configTime.AppSettings.Settings;

            ExeConfigurationFileMap configFileMapRes = new ExeConfigurationFileMap();
            configFileMapRes.ExeConfigFilename = Path.Combine(Directory.GetCurrentDirectory(), "TimeAttendanceService.encrypted.config");

            Configuration configManagerTime = ConfigurationManager.OpenMappedExeConfiguration(configFileMapRes, ConfigurationUserLevel.None);

            configManagerTime.AppSettings.Settings.Remove("SQLConnectionstring");
            configManagerTime.AppSettings.Settings.Add("SQLConnectionstring", Encrypt_Aes(getConfCollectionTime["SQLConnectionstring"].Value));
            configManagerTime.AppSettings.Settings.Remove("TASPunchUrl");
            configManagerTime.AppSettings.Settings.Add("TASPunchUrl", Encrypt_Aes(getConfCollectionTime["TASPunchUrl"].Value));
            configManagerTime.AppSettings.Settings.Remove("InTimeReasonCode");
            configManagerTime.AppSettings.Settings.Add("InTimeReasonCode", Encrypt_Aes(getConfCollectionTime["InTimeReasonCode"].Value));
            configManagerTime.AppSettings.Settings.Remove("OutTimeReasonCode");
            configManagerTime.AppSettings.Settings.Add("OutTimeReasonCode", Encrypt_Aes(getConfCollectionTime["OutTimeReasonCode"].Value));
            configManagerTime.AppSettings.Settings.Remove("TASLeaveMasterUrl");
            configManagerTime.AppSettings.Settings.Add("TASLeaveMasterUrl", Encrypt_Aes(getConfCollectionTime["TASLeaveMasterUrl"].Value));
            configManagerTime.AppSettings.Settings.Remove("TASPostLeaveUrl");
            configManagerTime.AppSettings.Settings.Add("TASPostLeaveUrl", Encrypt_Aes(getConfCollectionTime["TASPostLeaveUrl"].Value));
            configManagerTime.AppSettings.Settings.Remove("EncryptionKey");
            configManagerTime.AppSettings.Settings.Add("EncryptionKey", getConfCollectionTime["EncryptionKey"].Value);
            configManagerTime.AppSettings.Settings.Remove("EncryptionIV");
            configManagerTime.AppSettings.Settings.Add("EncryptionIV", getConfCollectionTime["EncryptionIV"].Value);

            configManagerTime.Save(ConfigurationSaveMode.Modified, true);
            //configManagerTime.SaveAs(Path.Combine(Directory.GetCurrentDirectory(), "TimeAttendanceService.encrypted.config"), ConfigurationSaveMode.Modified, true);
            ConfigurationManager.RefreshSection(configManagerTime.AppSettings.SectionInformation.Name);
            
        }

    }
}
