using Core.Enums;
using Newtonsoft.Json;
using Serilog;
using System;

namespace Core.Helpers
{
	public class CommonHelper
    {
        public static JsonSerializerSettings MicrosoftDateFormatSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
            }
        }

        public string GetJsonString(object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj, MicrosoftDateFormatSettings);

            }
            catch (Exception ex)
            {
				Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));
				throw;
            }
        }
    }
}
