using RulersCourt.Models.OfficialTaskCompensation.OfficialTask;
using System.Data.SqlClient;

namespace RulersCourt.Translators.OfficalTaskCompensation.OfficalTask
{
    public static class OfficialTaskSaveResponseTranslator
    {
        public static OfficialTaskWorkflowModel TranslateAsOfficialTaskSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var officialTaskSave = new OfficialTaskWorkflowModel();

            if (reader.IsColumnExists("OfficialTaskID"))
                officialTaskSave.OfficialTaskID = SqlHelper.GetNullableInt32(reader, "OfficialTaskID");

            if (reader.IsColumnExists("ReferenceNumber"))
                officialTaskSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CreatorID"))
                officialTaskSave.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();

            if (reader.IsColumnExists("FromID"))
                officialTaskSave.FromID = SqlHelper.GetNullableInt32(reader, "FromID").GetValueOrDefault();

            return officialTaskSave;
        }

        public static OfficialTaskWorkflowModel TranslateAsOfficialTaskSaveResponseList(this SqlDataReader reader)
        {
            var officialTaskSaveResponse = new OfficialTaskWorkflowModel();
            while (reader.Read())
            {
                officialTaskSaveResponse = TranslateAsOfficialTaskSaveResponse(reader, true);
            }

            return officialTaskSaveResponse;
        }
    }
}