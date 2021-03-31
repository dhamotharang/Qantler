using Core.Enums;
using Core.Helpers;
using Core.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OERService.DataAccess
{
	public class ElasticDataAccess
    {

		private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor setting configuration
        /// </summary>
        /// <param name="configuration"></param>
        public ElasticDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<DatabaseResponse> ElasticSearch(string elasticSearch,string partialUrl)
        {
            int result = 0;
            try
            {
                string uri = _configuration.GetValue<string>("ElasticURL");
                HttpClient client = new HttpClient();
                var content = new StringContent(elasticSearch, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri + partialUrl, content);

                var responseString = await response.Content.ReadAsStringAsync();

                if(response.StatusCode == HttpStatusCode.OK)
                {
                    result = 200;
                }
                else
                {
                    result = 201;
                }


                return new DatabaseResponse { ResponseCode = result, Results = JsonConvert.DeserializeObject<JObject>(responseString) };
            }

            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));
                throw;
            }
          
        }

        
    }
}
