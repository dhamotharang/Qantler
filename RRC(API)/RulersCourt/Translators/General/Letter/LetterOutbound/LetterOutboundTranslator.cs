using RulersCourt.Models.General.Letter.LetterOutbound;
using RulersCourt.Models.Letter;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Letter
{
    public static class LetterOutboundTranslator
    {
        public static LetterOutboundGetModel TranslateAsLetter(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var letterModel = new LetterOutboundGetModel();

            if (reader.IsColumnExists("LetterID"))
            {
                letterModel.LetterID = SqlHelper.GetNullableInt32(reader, "LetterID");
            }

            if (reader.IsColumnExists("LetterReferenceID"))
            {
                letterModel.LetterReferenceNumber = SqlHelper.GetNullableString(reader, "LetterReferenceID");
            }

            if (reader.IsColumnExists("LetterTitle"))
            {
                letterModel.Title = SqlHelper.GetNullableString(reader, "LetterTitle");
            }

            if (reader.IsColumnExists("SourceOU"))
            {
                letterModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");
            }

            if (reader.IsColumnExists("SourceName"))
            {
                letterModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");
            }

            if (reader.IsColumnExists("ApproverNameID"))
            {
                letterModel.ApproverId = SqlHelper.GetNullableInt32(reader, "ApproverNameID");
            }

            if (reader.IsColumnExists("ApproverDepartmentID"))
            {
                letterModel.ApproverDepartmentId = SqlHelper.GetNullableString(reader, "ApproverDepartmentID");
            }

            if (reader.IsColumnExists("LetterDetails"))
            {
                letterModel.LetterDetails = SqlHelper.GetNullableString(reader, "LetterDetails");
            }

            if (reader.IsColumnExists("RelatedToIncomingLetter"))
            {
                letterModel.RelatedToIncomingLetter = SqlHelper.GetNullableString(reader, "RelatedToIncomingLetter");
            }

            if (reader.IsColumnExists("DocumentClassification"))
            {
                letterModel.DocumentClassification = SqlHelper.GetNullableString(reader, "DocumentClassification");
            }

            if (reader.IsColumnExists("NeedReply"))
            {
                letterModel.NeedReply = SqlHelper.GetBoolean(reader, "NeedReply");
            }

            if (reader.IsColumnExists("Priority"))
            {
                letterModel.Priority = SqlHelper.GetNullableString(reader, "Priority");
            }

            if (reader.IsColumnExists("Status"))
            {
                letterModel.Status = SqlHelper.GetNullableInt32(reader, "Status");
            }

            if (reader.IsColumnExists("StatusCode"))
            {
                letterModel.StatusCode = SqlHelper.GetNullableInt32(reader, "StatusCode");
            }

            if (reader.IsColumnExists("LetterType"))
            {
                letterModel.LetterType = SqlHelper.GetNullableString(reader, "LetterType");
            }

            if (reader.IsColumnExists("CreatedBy"))
            {
                letterModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");
            }

            if (reader.IsColumnExists("UpdatedBy"))
            {
                letterModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");
            }

            if (reader.IsColumnExists("CreatedDateTime"))
            {
                letterModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");
            }

            if (reader.IsColumnExists("UpdatedDateTime"))
            {
                letterModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");
            }

            if (reader.IsColumnExists("IsRedirect"))
            {
                letterModel.IsRedirect = SqlHelper.GetNullableInt32(reader, "IsRedirect");
            }

            return letterModel;
        }

        public static List<LetterOutboundGetModel> TranslateAsLetterList(this SqlDataReader reader)
        {
            var letterList = new List<LetterOutboundGetModel>();
            while (reader.Read())
            {
                letterList.Add(TranslateAsLetter(reader, true));
            }

            return letterList;
        }

        public static LetterOutboundPutModel TranslateAsPutLetter(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var letterModel = new LetterOutboundPutModel();

            if (reader.IsColumnExists("LetterID"))
            {
                letterModel.LetterID = SqlHelper.GetNullableInt32(reader, "LetterID");
            }

            if (reader.IsColumnExists("LetterTitle"))
            {
                letterModel.Title = SqlHelper.GetNullableString(reader, "LetterTitle");
            }

            if (reader.IsColumnExists("SourceOU"))
            {
                letterModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");
            }

            if (reader.IsColumnExists("SourceName"))
            {
                letterModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");
            }

            if (reader.IsColumnExists("ApproverId"))
            {
                letterModel.ApproverId = SqlHelper.GetNullableInt32(reader, "ApproverId");
            }

            if (reader.IsColumnExists("ApproverDepartmentId"))
            {
                letterModel.ApproverDepartmentId = SqlHelper.GetNullableInt32(reader, "ApproverDepartmentId");
            }

            if (reader.IsColumnExists("LetterDetails"))
            {
                letterModel.LetterDetails = SqlHelper.GetNullableString(reader, "LetterDetails");
            }

            if (reader.IsColumnExists("RelatedToIncomingLetter"))
            {
                letterModel.RelatedToIncomingLetter = SqlHelper.GetNullableString(reader, "RelatedToIncomingLetter");
            }

            if (reader.IsColumnExists("DocumentClassification"))
            {
                letterModel.DocumentClassification = SqlHelper.GetNullableString(reader, "DocumentClassification");
            }

            if (reader.IsColumnExists("NeedReply"))
            {
                letterModel.NeedReply = SqlHelper.GetBoolean(reader, "NeedReply");
            }

            if (reader.IsColumnExists("Priority"))
            {
                letterModel.Priority = SqlHelper.GetNullableString(reader, "Priority");
            }

            if (reader.IsColumnExists("LetterType"))
            {
                letterModel.LetterType = SqlHelper.GetNullableString(reader, "LetterType");
            }

            if (reader.IsColumnExists("UpdatedBy"))
            {
                letterModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");
            }

            if (reader.IsColumnExists("UpdatedDateTime"))
            {
                letterModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");
            }

            if (reader.IsColumnExists("Action"))
            {
                letterModel.Action = SqlHelper.GetNullableString(reader, "Action");
            }

            if (reader.IsColumnExists("LetterReferenceID"))
            {
                letterModel.LetterReferenceNumber = SqlHelper.GetNullableString(reader, "LetterReferenceID");
            }

            return letterModel;
        }

        public static List<LetterOutboundPutModel> TranslateAsPutLetterList(this SqlDataReader reader)
        {
            var letterList = new List<LetterOutboundPutModel>();
            while (reader.Read())
            {
                letterList.Add(TranslateAsPutLetter(reader, true));
            }

            return letterList;
        }

        public static LetterOutboundDashboardListModel TranslateAsLetterDashboard(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var letterDashboardModel = new LetterOutboundDashboardListModel();

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

            if (reader.IsColumnExists("SenderName"))
            {
                letterDashboardModel.SenderName = SqlHelper.GetNullableString(reader, "SenderName");
            }

            if (reader.IsColumnExists("CreatedDateTime"))
            {
                letterDashboardModel.Date = SqlHelper.GetDateTime(reader, "CreatedDateTime");
            }

            if (reader.IsColumnExists("Replied"))
            {
                letterDashboardModel.Replied = SqlHelper.GetNullableString(reader, "Replied");
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

        public static List<LetterOutboundDashboardListModel> TranslateAsLetterDashboardList(this SqlDataReader reader)
        {
            var letterDashboardList = new List<LetterOutboundDashboardListModel>();
            while (reader.Read())
            {
                letterDashboardList.Add(TranslateAsLetterDashboard(reader, true));
            }

            return letterDashboardList;
        }

        public static LetterOutboundWorkflowModel TranslateAsLetterBulkWorkflow(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var letterModel = new LetterOutboundWorkflowModel();

            if (reader.IsColumnExists("LetterID"))
            {
                letterModel.LetterID = SqlHelper.GetNullableInt32(reader, "LetterID");
            }

            if (reader.IsColumnExists("LetterReferenceID"))
            {
                letterModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "LetterReferenceID");
            }

            if (reader.IsColumnExists("status"))
            {
                letterModel.Status = SqlHelper.GetNullableInt32(reader, "status").GetValueOrDefault();
            }

            if (reader.IsColumnExists("CreatedBy"))
            {
                letterModel.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatedBy").GetValueOrDefault();
            }

            if (reader.IsColumnExists("CreatedBy"))
            {
                letterModel.FromID = SqlHelper.GetNullableInt32(reader, "CreatedBy").GetValueOrDefault();
            }

            if (reader.IsColumnExists("ApproverNameID"))
            {
                letterModel.ApproverID = SqlHelper.GetNullableInt32(reader, "ApproverNameID");
            }

            return letterModel;
        }

        public static List<LetterOutboundWorkflowModel> TranslateAsLetterBulkWorkflowList(this SqlDataReader reader)
        {
            var letterBulkWorkflowList = new List<LetterOutboundWorkflowModel>();
            while (reader.Read())
            {
                letterBulkWorkflowList.Add(TranslateAsLetterBulkWorkflow(reader, true));
            }

            return letterBulkWorkflowList;
        }

        public static RecipientUsersModel TranslateAsOutboundLetterReceipient(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var letterModel = new RecipientUsersModel();

            if (reader.IsColumnExists("ProfilePhotoName"))
            {
                letterModel.ProfilePhotoName = SqlHelper.GetNullableString(reader, "ProfilePhotoName");
            }

            if (reader.IsColumnExists("ProfilePhotoID"))
            {
                letterModel.ProfilePhotoID = SqlHelper.GetNullableString(reader, "ProfilePhotoID");
            }

            if (reader.IsColumnExists("UserName"))
            {
                letterModel.UserName = SqlHelper.GetNullableString(reader, "UserName");
            }

            if (reader.IsColumnExists("PhoneNumber"))
            {
                letterModel.PhoneNumber = SqlHelper.GetNullableString(reader, "PhoneNumber");
            }

            if (reader.IsColumnExists("DepartmentName"))
            {
                letterModel.DepartmentName = SqlHelper.GetNullableString(reader, "DepartmentName");
            }

            return letterModel;
        }

        public static List<RecipientUsersModel> TranslateAsOutboundLetterReceipientList(this SqlDataReader reader)
        {
            var letterBulkWorkflowList = new List<RecipientUsersModel>();
            while (reader.Read())
            {
                letterBulkWorkflowList.Add(TranslateAsOutboundLetterReceipient(reader, true));
            }

            return letterBulkWorkflowList;
        }

        public static LetterOutboundPreviewModel TranslateAsLetterPreview(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var letterModel = new LetterOutboundPreviewModel();

            if (reader.IsColumnExists("LetterID"))
            {
                letterModel.LetterID = SqlHelper.GetNullableInt32(reader, "LetterID");
            }

            if (reader.IsColumnExists("LetterReferenceID"))
            {
                letterModel.LetterReferenceNumber = SqlHelper.GetNullableString(reader, "LetterReferenceID");
            }

            if (reader.IsColumnExists("SignaturePhotoName"))
            {
                letterModel.SignaturePhotoName = SqlHelper.GetNullableString(reader, "SignaturePhotoName");
            }

            if (reader.IsColumnExists("SignaturePhotoID"))
            {
                letterModel.SignaturePhotoID = SqlHelper.GetNullableString(reader, "SignaturePhotoID");
            }

            if (reader.IsColumnExists("ApproverTitle"))
            {
                letterModel.ApproverTitle = SqlHelper.GetNullableString(reader, "ApproverTitle");
            }

            if (reader.IsColumnExists("LetterTitle"))
            {
                letterModel.Title = SqlHelper.GetNullableString(reader, "LetterTitle");
            }

            if (reader.IsColumnExists("SourceOU"))
            {
                letterModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");
            }

            if (reader.IsColumnExists("DestinationTitle"))
            {
                letterModel.DestinationTitle = SqlHelper.GetNullableString(reader, "DestinationTitle");
            }

            if (reader.IsColumnExists("SourceName"))
            {
                letterModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");
            }

            if (reader.IsColumnExists("ApproverNameID"))
            {
                letterModel.ApproverName = SqlHelper.GetNullableString(reader, "ApproverNameID");
            }

            if (reader.IsColumnExists("ApproverDepartmentID"))
            {
                letterModel.ApproverDepartmentId = SqlHelper.GetNullableString(reader, "ApproverDepartmentID");
            }

            if (reader.IsColumnExists("ApproverDesignation"))
            {
                letterModel.ApproverDesignation = SqlHelper.GetNullableString(reader, "ApproverDesignation");
            }

            if (reader.IsColumnExists("Status"))
            {
                letterModel.Status = SqlHelper.GetNullableInt32(reader, "Status");
            }

            if (reader.IsColumnExists("LetterType"))
            {
                letterModel.LetterType = SqlHelper.GetNullableString(reader, "LetterType");
            }

            if (reader.IsColumnExists("LetterDetails"))
            {
                letterModel.LetterDetails = SqlHelper.GetNullableString(reader, "LetterDetails");
            }

            if (reader.IsColumnExists("CreatedBy"))
            {
                letterModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");
            }

            if (reader.IsColumnExists("ApproverID"))
            {
                letterModel.ApproverID = SqlHelper.GetNullableInt32(reader, "ApproverID");
            }

            if (reader.IsColumnExists("CreatedDateTime"))
            {
                letterModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");
            }

            return letterModel;
        }

        public static List<LetterOutboundPreviewModel> TranslateAsLetterPreviewList(this SqlDataReader reader)
        {
            var letterList = new List<LetterOutboundPreviewModel>();
            while (reader.Read())
            {
                letterList.Add(TranslateAsLetterPreview(reader, true));
            }

            return letterList;
        }
    }
}
