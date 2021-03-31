using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.HR
{
    public static class HRTranslator
    {
        public static HRHomeDashboardListModel TranslateAsHRDashboard(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var hRDashboardModel = new HRHomeDashboardListModel();

            if (reader.IsColumnExists("ReferenceNumber"))
            {
                hRDashboardModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");
            }

            if (reader.IsColumnExists("Creator"))
            {
                hRDashboardModel.Creator = SqlHelper.GetNullableString(reader, "Creator");
            }

            if (reader.IsColumnExists("RequestType"))
            {
                hRDashboardModel.RequestType = SqlHelper.GetNullableString(reader, "RequestType");
            }

            if (reader.IsColumnExists("Status"))
            {
                hRDashboardModel.Status = SqlHelper.GetNullableString(reader, "Status");
            }

            if (reader.IsColumnExists("RequestDate"))
            {
                hRDashboardModel.CreationDate = SqlHelper.GetDateTime(reader, "RequestDate");
            }

            if (reader.IsColumnExists("RequestID"))
            {
                hRDashboardModel.ID = SqlHelper.GetNullableInt32(reader, "RequestID");
            }

            if (reader.IsColumnExists("IsCompensationAvaliable"))
            {
                hRDashboardModel.IsCompensationAvailable = SqlHelper.GetBoolean(reader, "IsCompensationAvaliable");
            }

            if (reader.IsColumnExists("AssignedTo"))
            {
                hRDashboardModel.AssignedTo = SqlHelper.GetNullableString(reader, "AssignedTo");
            }

            if (reader.IsColumnExists("IsCompensationRequest"))
            {
                hRDashboardModel.IsCompensationRequest = SqlHelper.GetBoolean(reader, "IsCompensationRequest");
            }

            return hRDashboardModel;
        }

        public static List<HRHomeDashboardListModel> TranslateAsHRDashboardList(this SqlDataReader reader)
        {
            var hRDashboardList = new List<HRHomeDashboardListModel>();
            while (reader.Read())
            {
                hRDashboardList.Add(TranslateAsHRDashboard(reader, true));
            }

            return hRDashboardList;
        }
    }
}
