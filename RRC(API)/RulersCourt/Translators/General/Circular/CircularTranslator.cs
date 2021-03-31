using RulersCourt.Models.Circular;
using RulersCourt.Models.General.Circular;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Circular
{
    public static class CircularTranslator
    {
        public static CircularGetModel TranslateAsCircular(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var circularModel = new CircularGetModel();

            if (reader.IsColumnExists("CircularID"))
                circularModel.CircularID = SqlHelper.GetNullableInt32(reader, "CircularID");

            if (reader.IsColumnExists("ReferenceNumber"))
                circularModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("Title"))
                circularModel.Title = SqlHelper.GetNullableString(reader, "Title");

            if (reader.IsColumnExists("SourceOU"))
                circularModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                circularModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("Details"))
                circularModel.Details = SqlHelper.GetNullableString(reader, "Details");

            if (reader.IsColumnExists("Priority"))
                circularModel.Priority = SqlHelper.GetNullableString(reader, "Priority");

            if (reader.IsColumnExists("CreatedBy"))
                circularModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                circularModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                circularModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                circularModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("Status"))
                circularModel.Status = SqlHelper.GetNullableString(reader, "Status");

            if (reader.IsColumnExists("StatusCode"))
                circularModel.StatusCode = SqlHelper.GetNullableString(reader, "StatusCode");

            if (reader.IsColumnExists("ApproverDepartmentId"))
                circularModel.ApproverDepartmentId = SqlHelper.GetNullableInt32(reader, "ApproverDepartmentId");

            if (reader.IsColumnExists("ApproverNameId"))
                circularModel.ApproverId = SqlHelper.GetNullableInt32(reader, "ApproverNameId");

            if (reader.IsColumnExists("Destination"))
                circularModel.Destination = SqlHelper.GetNullableString(reader, "Destination");

            return circularModel;
        }

        public static List<CircularGetModel> TranslateAsCircularList(this SqlDataReader reader)
        {
            var circularList = new List<CircularGetModel>();
            while (reader.Read())
            {
                circularList.Add(TranslateAsCircular(reader, true));
            }

            return circularList;
        }

        public static CircularPutModel TranslateAsPutCircular(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var circularModel = new CircularPutModel();

            if (reader.IsColumnExists("CircularID"))
                circularModel.CircularID = SqlHelper.GetNullableInt32(reader, "CircularID");

            if (reader.IsColumnExists("Title"))
                circularModel.Title = SqlHelper.GetNullableString(reader, "Title");

            if (reader.IsColumnExists("SourceOU"))
                circularModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                circularModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("ApproverId"))
                circularModel.ApproverId = SqlHelper.GetNullableInt32(reader, "ApproverId");

            if (reader.IsColumnExists("ApproverDepartmentId"))
                circularModel.ApproverDepartmentId = SqlHelper.GetNullableInt32(reader, "ApproverDepartmentId");

            if (reader.IsColumnExists("Details"))
                circularModel.Details = SqlHelper.GetNullableString(reader, "Details");

            if (reader.IsColumnExists("Priority"))
                circularModel.Priority = SqlHelper.GetNullableString(reader, "Priority");

            if (reader.IsColumnExists("UpdatedBy"))
                circularModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                circularModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("Action"))
                circularModel.Action = SqlHelper.GetNullableString(reader, "Action");

            return circularModel;
        }

        public static List<CircularPutModel> TranslateAsPutCircularList(this SqlDataReader reader)
        {
            var circularList = new List<CircularPutModel>();
            while (reader.Read())
            {
                circularList.Add(TranslateAsPutCircular(reader, true));
            }

            return circularList;
        }

        public static CircularReport TranslateAsCircularReport(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var report = new CircularReport();

            if (reader.IsColumnExists("CircularID"))
                report.CircularID = SqlHelper.GetNullableInt32(reader, "CircularID");

            if (reader.IsColumnExists("ReferenceNumber"))
                report.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("Title"))
                report.Title = SqlHelper.GetNullableString(reader, "Title");

            if (reader.IsColumnExists("Source"))
                report.SourceOU = SqlHelper.GetNullableString(reader, "Source");

            if (reader.IsColumnExists("Destination"))
                report.DestinationOU = SqlHelper.GetNullableString(reader, "Destination");

            if (reader.IsColumnExists("Status"))
                report.Status = SqlHelper.GetNullableString(reader, "Status");

            if (reader.IsColumnExists("Priority"))
                report.Priority = SqlHelper.GetNullableString(reader, "Priority");

            return report;
        }

        public static List<CircularReport> TranslateAsCircularReportList(this SqlDataReader reader)
        {
            var list = new List<CircularReport>();
            while (reader.Read())
            {
                list.Add(TranslateAsCircularReport(reader, true));
            }

            return list;
        }

        public static CircularPreviewModel TranslateAsCircularPreview(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var circularModel = new CircularPreviewModel();

            if (reader.IsColumnExists("CircularID"))
                circularModel.CircularID = SqlHelper.GetNullableInt32(reader, "CircularID");

            if (reader.IsColumnExists("ReferenceNumber"))
                circularModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("Title"))
                circularModel.Title = SqlHelper.GetNullableString(reader, "Title");

            if (reader.IsColumnExists("SourceOU"))
                circularModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                circularModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("Details"))
                circularModel.Details = SqlHelper.GetNullableString(reader, "Details");

            if (reader.IsColumnExists("Priority"))
                circularModel.Priority = SqlHelper.GetNullableString(reader, "Priority");

            if (reader.IsColumnExists("CreatedBy"))
                circularModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("ApproverID"))
                circularModel.ApproverID = SqlHelper.GetNullableInt32(reader, "ApproverID");

            if (reader.IsColumnExists("UpdatedBy"))
                circularModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                circularModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                circularModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("Status"))
                circularModel.Status = SqlHelper.GetNullableString(reader, "Status");

            if (reader.IsColumnExists("ApproverDepartmentId"))
                circularModel.ApproverDepartmentId = SqlHelper.GetNullableInt32(reader, "ApproverDepartmentId");

            if (reader.IsColumnExists("ApproverNameID"))
                circularModel.ApproverName = SqlHelper.GetNullableString(reader, "ApproverNameID");

            if (reader.IsColumnExists("Destination"))
                circularModel.Destination = SqlHelper.GetNullableString(reader, "Destination");

            if (reader.IsColumnExists("SignaturePhotoApprover"))
                circularModel.SignaturePhotoApprover = SqlHelper.GetNullableString(reader, "SignaturePhotoApprover");

            if (reader.IsColumnExists("ApproverDesignation"))
                circularModel.ApproverDesignation = SqlHelper.GetNullableString(reader, "ApproverDesignation");

            return circularModel;
        }

        public static List<CircularPreviewModel> TranslateAsCircularPreviewList(this SqlDataReader reader)
        {
            var circularList = new List<CircularPreviewModel>();
            while (reader.Read())
            {
                circularList.Add(TranslateAsCircularPreview(reader, true));
            }

            return circularList;
        }
    }
}
