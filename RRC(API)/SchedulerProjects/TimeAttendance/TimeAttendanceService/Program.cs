using Serilog;
using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;

namespace TimeAttendanceService
{
    class Program
    {
        private static readonly byte[] key = Convert.FromBase64String(ConfigurationManager.AppSettings["EncryptionKey"]);
        private static readonly byte[] iv = Convert.FromBase64String(ConfigurationManager.AppSettings["EncryptionIV"]);
        static void Main(string[] args)
        {
            ConfigurationManager.AppSettings["SQLConnectionstring"] = Decrypt_Aes(ConfigurationManager.AppSettings["SQLConnectionstring"]);
            ConfigurationManager.AppSettings["TASPunchUrl"] = Decrypt_Aes(ConfigurationManager.AppSettings["TASPunchUrl"]);
            ConfigurationManager.AppSettings["InTimeReasonCode"] = Decrypt_Aes(ConfigurationManager.AppSettings["InTimeReasonCode"]);
            ConfigurationManager.AppSettings["OutTimeReasonCode"] = Decrypt_Aes(ConfigurationManager.AppSettings["OutTimeReasonCode"]);
            ConfigurationManager.AppSettings["TASLeaveMasterUrl"] = Decrypt_Aes(ConfigurationManager.AppSettings["TASLeaveMasterUrl"]);
            ConfigurationManager.AppSettings["TASPostLeaveUrl"] = Decrypt_Aes(ConfigurationManager.AppSettings["TASPostLeaveUrl"]);

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("TasApp_" + DateTime.Now.ToString("yyyyMMdd") + ".log")
                .CreateLogger();

            DeleteOlderLogFiles();

            try
            {
                //Processing each employee punch in/out time
                PunchProcessService.ProcessPunchTimings();

                //Processing approved leave requests to TAS
                LeaveProcessService.ProcessLeaveRequests();

                //Processing official leave requests to TAS
                LeaveProcessService.ProcessOfficialLeaveRequests();
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.StackTrace);
            }
        }

        private static void DeleteOlderLogFiles()
        {
            try
            {
                var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.log");
                foreach (var file in files)
                {
                    if (File.GetCreationTime(file) <= DateTime.Now.AddDays(-3))
                        File.Delete(file);
                }
            }
            catch
            {
            }
        }
        public static string Decrypt_Aes(string cipherTextStr)
        {
            byte[] cipherText = Convert.FromBase64String(cipherTextStr);
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException(nameof(cipherText));
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException(nameof(iv));

            string plaintext;
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}