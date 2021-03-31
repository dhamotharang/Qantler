using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class M_OfficialTaskRequestTranslator
    {
        public static M_OfficialTaskRequestModel TranslateAsGetOfficialTaskRequest(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var officialTaskRequest = new M_OfficialTaskRequestModel();

            if (reader.IsColumnExists("OfficialTaskRequestID"))
                officialTaskRequest.OfficialTaskRequestID = SqlHelper.GetNullableInt32(reader, "OfficialTaskRequestID");

            if (reader.IsColumnExists("OfficialTaskRequestName"))
                officialTaskRequest.OfficialTaskRequestName = SqlHelper.GetNullableString(reader, "OfficialTaskRequestName");

            if (reader.IsColumnExists("DisplayOrder"))
                officialTaskRequest.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return officialTaskRequest;
        }

        public static List<M_OfficialTaskRequestModel> TranslateAsOfficialTaskRequest(this SqlDataReader reader)
        {
            var officialTaskRequests = new List<M_OfficialTaskRequestModel>();
            while (reader.Read())
            {
                officialTaskRequests.Add(TranslateAsGetOfficialTaskRequest(reader, true));
            }

            return officialTaskRequests;
        }
    }
}
