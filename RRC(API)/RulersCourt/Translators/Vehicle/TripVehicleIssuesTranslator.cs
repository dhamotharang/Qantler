using RulersCourt.Models.Vehicle.TripVehicleIssues;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Vehicle
{
    public static class TripVehicleIssuesTranslator
    {
        public static TripVehicleIssuesPostModel TranslateAsGetVehicleIssues(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var issues = new TripVehicleIssuesPostModel();

            if (reader.IsColumnExists("IssueID"))
                issues.IssueID = SqlHelper.GetNullableInt32(reader, "IssueID");

            if (reader.IsColumnExists("IssueName"))
                issues.IssueName = SqlHelper.GetNullableString(reader, "IssueName");

            if (reader.IsColumnExists("VehicleReqID"))
                issues.VehicleReqID = SqlHelper.GetNullableInt32(reader, "VehicleReqID");

            if (reader.IsColumnExists("DeleteFlag"))
                issues.DeleteFlag = SqlHelper.GetBoolean(reader, "DeleteFlag");

            return issues;
        }

        public static List<TripVehicleIssuesPostModel> TranslateAsVehicleIssuesList(this SqlDataReader reader)
        {
            var vehicleIssues = new List<TripVehicleIssuesPostModel>();
            while (reader.Read())
            {
                vehicleIssues.Add(TranslateAsGetVehicleIssues(reader, true));
            }

            return vehicleIssues;
        }
    }
}
