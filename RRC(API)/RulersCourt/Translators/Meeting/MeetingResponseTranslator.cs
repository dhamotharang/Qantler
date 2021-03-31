using RulersCourt.Models.Meeting;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Meeting
{
    public static class MeetingResponseTranslator
    {
        public static MeetingResponseModel TranslateAsMeetingSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var designSave = new MeetingResponseModel();

            if (reader.IsColumnExists("MeetingID"))
                designSave.MeetingID = SqlHelper.GetNullableInt32(reader, "MeetingID");

            if (reader.IsColumnExists("ReferenceNumber"))
                designSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CreatorID"))
                designSave.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();

            if (reader.IsColumnExists("FromID"))
                designSave.FromID = SqlHelper.GetNullableInt32(reader, "FromID").GetValueOrDefault();

            return designSave;
        }

        public static MeetingResponseModel TranslateAsMeetingSaveResponseList(this SqlDataReader reader)
        {
            var designSaveResponse = new MeetingResponseModel();
            while (reader.Read())
            {
                designSaveResponse = TranslateAsMeetingSaveResponse(reader, true);
            }

            return designSaveResponse;
        }

        public static MeetingResponseModel TranslateAsMOMSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var designSave = new MeetingResponseModel();

            if (reader.IsColumnExists("MOMID"))
                designSave.MOMID = SqlHelper.GetNullableInt32(reader, "MOMID");
            if (reader.IsColumnExists("MeetingID"))
                designSave.MeetingID = SqlHelper.GetNullableInt32(reader, "MeetingID");

            if (reader.IsColumnExists("ReferenceNumber"))
                designSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CreatorID"))
                designSave.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();

            if (reader.IsColumnExists("FromID"))
                designSave.FromID = SqlHelper.GetNullableInt32(reader, "FromID").GetValueOrDefault();

            return designSave;
        }

        public static MeetingResponseModel TranslateAsMOMSaveResponseList(this SqlDataReader reader)
        {
            var designSaveResponse = new MeetingResponseModel();
            while (reader.Read())
            {
                designSaveResponse = TranslateAsMOMSaveResponse(reader, true);
            }

            return designSaveResponse;
        }
    }
}