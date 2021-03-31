using RulersCourt.Models.LeaveRequest;
using System.Data.SqlClient;

namespace RulersCourt.Translators.LeaveRequest
{
    public static class LeaveSaveResponseTranslator
    {
        public static LeaveWorkflowModel TranslateAsLeaveSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var leaveSave = new LeaveWorkflowModel();

            if (reader.IsColumnExists("LeaveID"))
                leaveSave.LeaveID = SqlHelper.GetNullableInt32(reader, "LeaveID");

            if (reader.IsColumnExists("ReferenceNumber"))
                leaveSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CreatorID"))
                leaveSave.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();

            if (reader.IsColumnExists("FromID"))
                leaveSave.FromID = SqlHelper.GetNullableInt32(reader, "FromID").GetValueOrDefault();

            return leaveSave;
        }

        public static LeaveWorkflowModel TranslateAsLeaveSaveResponseList(this SqlDataReader reader)
        {
            var leaveSaveResponse = new LeaveWorkflowModel();
            while (reader.Read())
            {
                leaveSaveResponse = TranslateAsLeaveSaveResponse(reader, true);
            }

            return leaveSaveResponse;
        }
    }
}
