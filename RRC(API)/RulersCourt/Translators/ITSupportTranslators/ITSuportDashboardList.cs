using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.ITSupportTranslators
{
    public static class ITSuportDashboardList
    {
        public static ITSupportHomeDashboardListModel TranslateAsITDashboard(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var iTDashboardModel = new ITSupportHomeDashboardListModel();

            if (reader.IsColumnExists("ReferenceNumber"))
            {
                iTDashboardModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");
            }

            if (reader.IsColumnExists("SourceOU"))
            {
                iTDashboardModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");
            }

            if (reader.IsColumnExists("Priority"))
            {
                iTDashboardModel.Priority = SqlHelper.GetNullableInt32(reader, "Priority");
            }

            if (reader.IsColumnExists("Subject"))
            {
                iTDashboardModel.Subject = SqlHelper.GetNullableString(reader, "Subject");
            }

            if (reader.IsColumnExists("RequestType"))
            {
                iTDashboardModel.RequestType = SqlHelper.GetNullableString(reader, "RequestType");
            }

            if (reader.IsColumnExists("RequestID"))
            {
                iTDashboardModel.RequestID = SqlHelper.GetNullableInt32(reader, "RequestID");
            }

            if (reader.IsColumnExists("Status"))
            {
                iTDashboardModel.Status = SqlHelper.GetNullableString(reader, "Status");
            }

            if (reader.IsColumnExists("RequestDate"))
            {
                iTDashboardModel.CreationDate = SqlHelper.GetDateTime(reader, "RequestDate");
            }

            return iTDashboardModel;
        }

        public static List<ITSupportHomeDashboardListModel> TranslateAsITDashboardList(this SqlDataReader reader)
        {
            var iTDashboardList = new List<ITSupportHomeDashboardListModel>();
            while (reader.Read())
            {
                iTDashboardList.Add(TranslateAsITDashboard(reader, true));
            }

            return iTDashboardList;
        }
    }
}
