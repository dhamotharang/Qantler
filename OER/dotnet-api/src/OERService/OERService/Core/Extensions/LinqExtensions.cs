using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Extensions
{
	public class LinqExtensions
    {
		LinqExtensions()
		{
		}

		public static List<Dictionary<string, string>> GetDictionary(DataTable dt)
        {
            return dt.AsEnumerable().Select(
                row => dt.Columns.Cast<DataColumn>().ToDictionary(
                    column => column.ColumnName,
                    column => row[column].ToString()
                )).ToList();
        }

        public static T GeObjectFromDictionary<T>(Dictionary<string, string> dict)
        {
            string json = JsonConvert.SerializeObject(dict);
            return JsonConvert.DeserializeObject<T>(json);
        }

    }
}
