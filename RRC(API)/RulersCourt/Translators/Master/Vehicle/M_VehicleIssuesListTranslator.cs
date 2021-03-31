using RulersCourt.Models.Master.M_Vehicle;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Master.Vehicle
{
    public static class M_VehicleIssuesListTranslator
    {
        public static M_VehicleIssueModel TranslateAsGetVehicleIssue(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var issue = new M_VehicleIssueModel();

            if (reader.IsColumnExists("IssueID"))
                issue.IssueID = SqlHelper.GetNullableInt32(reader, "IssueID");

            if (reader.IsColumnExists("IssueName"))
                issue.IssueName = SqlHelper.GetNullableString(reader, "IssueName");

            if (reader.IsColumnExists("DisplayOrder"))
                issue.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return issue;
        }

        public static List<M_VehicleIssueModel> TranslateAsGetVehicleIssueLIst(this SqlDataReader reader)
        {
            var logType = new List<M_VehicleIssueModel>();
            while (reader.Read())
            {
                logType.Add(TranslateAsGetVehicleIssue(reader, true));
            }

            return logType;
        }
    }
}
