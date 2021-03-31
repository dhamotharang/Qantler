using RulersCourt.Models;
using RulersCourt.Models.Maintenance;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Maintenance
{
    public static class MaintenanceCommunicationHistoryTranslator
    {
        public static MaintenanceCommunicationHistory TranslateAsMaintenanceCommunicationHistory(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var maintenanceHistory = new MaintenanceCommunicationHistory();

            if (reader.IsColumnExists("CommunicationID"))
                maintenanceHistory.CommunicationID = SqlHelper.GetNullableInt32(reader, "CommunicationID");

            if (reader.IsColumnExists("MaintenanceID"))
                maintenanceHistory.MaintenanceID = SqlHelper.GetNullableInt32(reader, "MaintenanceID");

            if (reader.IsColumnExists("Message"))
                maintenanceHistory.Message = SqlHelper.GetNullableString(reader, "Message");

            if (reader.IsColumnExists("ParentCommunicationID"))
                maintenanceHistory.ParentCommunicationID = SqlHelper.GetNullableInt32(reader, "ParentCommunicationID");

            if (reader.IsColumnExists("Action"))
                maintenanceHistory.Action = SqlHelper.GetNullableString(reader, "Action");

            string photoGuid = string.Empty, photoName = string.Empty;

            if (reader.IsColumnExists("PhotoGuid"))
                photoGuid = SqlHelper.GetNullableString(reader, "PhotoGuid");

            if (reader.IsColumnExists("PhotoName"))
                photoName = SqlHelper.GetNullableString(reader, "PhotoName");

            if (!string.IsNullOrEmpty(photoGuid) && !string.IsNullOrEmpty(photoName))
            {
                maintenanceHistory.Photo = photoGuid + "|" + photoName;
            }

            if (reader.IsColumnExists("CreatedBy"))
                maintenanceHistory.CreatedBy = SqlHelper.GetNullableString(reader, "CreatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                maintenanceHistory.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            return maintenanceHistory;
        }

        public static List<MaintenanceCommunicationHistory> TranslateAsMaintenanceCommunicationHistoryList(this SqlDataReader reader)
        {
            var communicationHistory = new List<MaintenanceCommunicationHistory>();
            while (reader.Read())
            {
                communicationHistory.Add(TranslateAsMaintenanceCommunicationHistory(reader, true));
            }

            return communicationHistory;
        }
    }
}
