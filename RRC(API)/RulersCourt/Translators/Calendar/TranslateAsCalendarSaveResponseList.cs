using RulersCourt.Models.Calendar;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Calendar
{
    public static class TranslateAsCalendarSaveResponseList
    {
        public static CalendarWorkFlowModel TranslateAsDesignSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var designSave = new CalendarWorkFlowModel();

            if (reader.IsColumnExists("CalendarID"))
                designSave.CalendarID = SqlHelper.GetNullableInt32(reader, "CalendarID");

            if (reader.IsColumnExists("ReferenceNumber"))
                designSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CreatorID"))
                designSave.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();

            if (reader.IsColumnExists("FromID"))
                designSave.FromID = SqlHelper.GetNullableInt32(reader, "FromID").GetValueOrDefault();

            return designSave;
        }

        public static CalendarWorkFlowModel TranslateAsSaveResponseList(this SqlDataReader reader)
        {
            var designSaveResponse = new CalendarWorkFlowModel();
            while (reader.Read())
            {
                designSaveResponse = TranslateAsDesignSaveResponse(reader, true);
            }

            return designSaveResponse;
        }
    }
}