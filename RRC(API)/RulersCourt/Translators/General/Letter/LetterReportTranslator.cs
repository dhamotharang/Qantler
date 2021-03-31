using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Letter.LetterInbound
{
    public static class LetterReportTranslator
    {
        public static List<LetterReportModel> TranslateAsLetterReportList(this SqlDataReader reader)
        {
            var letterList = new List<LetterReportModel>();
            while (reader.Read())
            {
                letterList.Add(TranslateAsLetterReport(reader, true));
            }

            return letterList;
        }

        public static LetterReportModel TranslateAsLetterReport(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var letterDashboardModel = new LetterReportModel();

            if (reader.IsColumnExists("LetterID"))
            {
                letterDashboardModel.LetterID = SqlHelper.GetNullableInt32(reader, "LetterID");
            }

            if (reader.IsColumnExists("ReferenceNumber"))
            {
                letterDashboardModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");
            }

            if (reader.IsColumnExists("Title"))
            {
                letterDashboardModel.Title = SqlHelper.GetNullableString(reader, "Title");
            }

            if (reader.IsColumnExists("SourceOU"))
            {
                letterDashboardModel.Source = SqlHelper.GetNullableString(reader, "SourceOU");
            }

            if (reader.IsColumnExists("Destination"))
            {
                letterDashboardModel.Destination = SqlHelper.GetNullableString(reader, "Destination");
            }

            if (reader.IsColumnExists("Status"))
            {
                letterDashboardModel.Status = SqlHelper.GetNullableString(reader, "Status");
            }

            if (reader.IsColumnExists("LinkToOtherLetter"))
            {
                letterDashboardModel.LinkedToOtherLetter = SqlHelper.GetNullableString(reader, "LinkToOtherLetter");
            }

            if (reader.IsColumnExists("Priority"))
            {
                letterDashboardModel.Priority = SqlHelper.GetNullableString(reader, "Priority");
            }

            if (reader.IsColumnExists("LetterType"))
            {
                letterDashboardModel.LetterType = SqlHelper.GetNullableString(reader, "LetterType");
            }

            return letterDashboardModel;
        }
    }
}
