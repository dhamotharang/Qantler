using Newtonsoft.Json;
using Serilog;
using SyncManageEngine.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SyncManageEngine
{
    class Program
    {
        static HttpClient client = new HttpClient();
        private static readonly byte[] key = Convert.FromBase64String(ConfigurationManager.AppSettings["EncryptionKey"]);
        private static readonly byte[] iv = Convert.FromBase64String(ConfigurationManager.AppSettings["EncryptionIV"]);

        private static readonly string TechnicianKey = Decrypt_Aes(ConfigurationManager.AppSettings["TechnicianKey"]);
        private static readonly string ManageEngineUrl = Decrypt_Aes(ConfigurationManager.AppSettings["ManageEngineUrl"]);
        static SQLManager sqlManager = new SQLManager();

        static void Main(string[] args)
        {

            ConfigurationManager.AppSettings["TechnicianKey"] = Decrypt_Aes(ConfigurationManager.AppSettings["TechnicianKey"]);
            ConfigurationManager.AppSettings["SQLConnectionstring"] = Decrypt_Aes(ConfigurationManager.AppSettings["SQLConnectionstring"]);
            ConfigurationManager.AppSettings["ManageEngineUrl"] = Decrypt_Aes(ConfigurationManager.AppSettings["ManageEngineUrl"]);
            
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("consoleapp.log")
                .CreateLogger();

            try
            {
                //Push IT support requests to service desk
                var itTickets = sqlManager.GetNonSyncTickets();
                foreach (var ticket in itTickets)
                {
                    PostRequest request = new PostRequest();
                    request.request = new request();
                    request.request.subject = ticket.Subject;
                    request.request.description = ticket.RequestDetails.Replace("'", " ").Replace('[', ' ').Replace(']', ' ').Replace('#', ' ').Replace('&', ' ');
                    request.request.requester = new requester() { name = ticket.RequestorName };
                    request.request.status = new status() { name = "Open" };
                    CreateRequestAsync(ticket.RefNo, request).Wait();
                }

                //Get all closed requests from ServiceDesk to update rulers court
                GetRequest req = new GetRequest();
                req.list_info = new List_Info();
                req.list_info.search_fields = new Search_Fields();
                req.list_info.search_fields.statusname = "Closed";
                ViewAllRequestAsync(req).Wait();

                sqlManager.UpdateSyncTime();
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.StackTrace);
            }
        }

        static async Task<string> CreateRequestAsync(string refNo, PostRequest req)
        {
            var strJson = JsonConvert.SerializeObject(req);
            ServiceDeskResponse resp = null;
            try
            {
                HttpResponseMessage response = await client.PostAsync(
                    string.Format("{0}?TECHNICIAN_KEY={1}&input_data={2}", ManageEngineUrl, TechnicianKey, strJson), null);
                response.EnsureSuccessStatusCode();

                //Updating ServiceDeskId and IsSync status so that request is not pushed again
                resp = JsonConvert.DeserializeObject<ServiceDeskResponse>(response.Content.ReadAsStringAsync().Result);
                sqlManager.UpdateServiceDeskResponseId(refNo, resp.request.id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.StackTrace);
            }
            return resp.request.id;
        }

        static async Task<Uri> ViewAllRequestAsync(GetRequest req)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await client.GetAsync(
                    string.Format("{0}?TECHNICIAN_KEY={1}", ManageEngineUrl, TechnicianKey));
                AllOpenRequests resp = JsonConvert.DeserializeObject<AllOpenRequests>(response.Content.ReadAsStringAsync().Result);
                var closedRequests = resp.requests.Where(x => x.status.name == "Closed").ToList();

                var itTickets = sqlManager.GetOpenTickets();
                foreach (var ticket in itTickets)
                {
                    //if there is a closed request from service desk, update the same in rulers court portal
                    if (closedRequests.Where(x => x.id == ticket.ServiceDeskId).Any())
                    {
                        sqlManager.UpdateStatus(ticket.RefNo);
                    }
                }

                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.StackTrace);
            }
            // return URI of the created resource.
            return response.Headers.Location;
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

    public class allrequests
    {
        public List<requests> requests { get; set; }
    }

    public class requests
    {
        public string id { get; set; }
        public status status { get; set; }
    }
}
