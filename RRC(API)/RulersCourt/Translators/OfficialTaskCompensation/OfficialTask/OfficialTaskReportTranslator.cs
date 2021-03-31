using RulersCourt.Models.OfficialTaskCompensation.OfficialTask;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.OfficialTaskCompensation.OfficialTask
{
    public static class OfficialTaskReportTranslator
    {
        public static OfficialTaskReportModel TranslateAsOfficialTaskReport(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var officialTaskReport = new OfficialTaskReportModel();

            if (reader.IsColumnExists("ReferenceNumber"))
                officialTaskReport.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("Status"))
                officialTaskReport.Status = SqlHelper.GetNullableString(reader, "Status");

            if (reader.IsColumnExists("SourceOU"))
                officialTaskReport.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SubjectName"))
                officialTaskReport.SubjectName = SqlHelper.GetNullableString(reader, "SubjectName");

            if (reader.IsColumnExists("OfficialTaskType"))
                officialTaskReport.OfficialTaskType = SqlHelper.GetNullableString(reader, "OfficialTaskType");

            if (reader.IsColumnExists("AttendedBy"))
                officialTaskReport.AttendedBy = SqlHelper.GetNullableString(reader, "AttendedBy");

            return officialTaskReport;
        }

        public static List<OfficialTaskReportModel> TranslateAsOfficialTaskReportList(this SqlDataReader reader)
        {
            var officialTaskList = new List<OfficialTaskReportModel>();
            while (reader.Read())
            {
                officialTaskList.Add(TranslateAsOfficialTaskReport(reader, true));
            }

            return officialTaskList;
        }
    }
}