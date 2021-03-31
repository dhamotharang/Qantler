using RulersCourt.Models;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class HomeListTranslator
    {
        public static HomeModel TranslateAsDashboard(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var listModel = new HomeModel();

            if (reader.IsColumnExists("Circular"))
                listModel.Circular = SqlHelper.GetNullableInt32(reader, "Circular");

            if (reader.IsColumnExists("DutyTask"))
                listModel.DutyTask = SqlHelper.GetNullableInt32(reader, "DutyTask");

            if (reader.IsColumnExists("Letters"))
                listModel.Letters = SqlHelper.GetNullableInt32(reader, "Letters");

            if (reader.IsColumnExists("Meeting"))
                listModel.Meeting = SqlHelper.GetNullableInt32(reader, "Meeting");

            if (reader.IsColumnExists("Memo"))
                listModel.Memo = SqlHelper.GetNullableInt32(reader, "Memo");

            if (reader.IsColumnExists("MeetingID"))
                listModel.MeetingID = SqlHelper.GetNullableInt32(reader, "MeetingID");

            if (reader.IsColumnExists("NextMeetingDateTime"))
                listModel.NextMeetingDateTime = SqlHelper.GetDateTime(reader, "NextMeetingDateTime");

            return listModel;
        }

        public static HomeModel TranslateAsDashboardList(this SqlDataReader reader)
        {
            var dashboardList = new HomeModel();
            while (reader.Read())
            {
                dashboardList = TranslateAsDashboard(reader, true);
            }

            return dashboardList;
        }
    }
}
