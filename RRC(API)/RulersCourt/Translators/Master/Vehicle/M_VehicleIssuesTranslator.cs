using RulersCourt.Models.Master.M_Vehicle;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Master.Vehicle
{
    public static class M_VehicleIssuesTranslator
    {
        public static M_VehicleIssuesModel TranslateAsGetVehicleIssues(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var vehicleIssues = new M_VehicleIssuesModel();

            if (reader.IsColumnExists("VehicleIssuesID"))
                vehicleIssues.IssueID = SqlHelper.GetNullableInt32(reader, "VehicleIssuesID");

            if (reader.IsColumnExists("VehicleIssuesName"))
                vehicleIssues.IssueName = SqlHelper.GetNullableString(reader, "VehicleIssuesName");

            if (reader.IsColumnExists("DisplayOrder"))
                vehicleIssues.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return vehicleIssues;
        }

        public static List<M_VehicleIssuesModel> TranslateAsVehicleIssues(this SqlDataReader reader)
        {
            var vehicleIssues = new List<M_VehicleIssuesModel>();
            while (reader.Read())
            {
                vehicleIssues.Add(TranslateAsGetVehicleIssues(reader, true));
            }

            return vehicleIssues;
        }
    }
}
