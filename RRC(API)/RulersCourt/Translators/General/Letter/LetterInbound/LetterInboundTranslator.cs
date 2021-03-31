using RulersCourt.Models.General.Letter.LetterInbound;
using RulersCourt.Models.Letter.LetterInbound;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Letter.LetterInbound
{
    public static class LetterInboundTranslator
    {
        public static LetterInboundGetModel TranslateAsLetterInbound(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var letterModel = new LetterInboundGetModel();

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

            if (reader.IsColumnExists("Status"))
            {
                letterModel.Status = SqlHelper.GetNullableInt32(reader, "Status");
            }

            if (reader.IsColumnExists("StatusCode"))
            {
                letterModel.StatusCode = SqlHelper.GetNullableInt32(reader, "StatusCode");
            }

            if (reader.IsColumnExists("RelatedToIncomingLetter"))
            {
                letterModel.RelatedToIncomingLetter = SqlHelper.GetNullableString(reader, "RelatedToIncomingLetter");
            }

            if (reader.IsColumnExists("DocumentClassification"))
            {
                letterModel.DocumentClassification = SqlHelper.GetNullableString(reader, "DocumentClassification");
            }

            if (reader.IsColumnExists("LetterPhysicallySend"))
            {
                letterModel.LetterPhysicallySend = SqlHelper.GetBoolean(reader, "LetterPhysicallySend");
            }

            if (reader.IsColumnExists("ReceivingDate"))
            {
                letterModel.ReceivingDate = SqlHelper.GetDateTime(reader, "ReceivingDate");
            }

            if (reader.IsColumnExists("ReceivedFromGovernmentEntity"))
            {
                letterModel.ReceivedFromGovernmentEntity = SqlHelper.GetNullableString(reader, "ReceivedFromGovernmentEntity");
            }

            if (reader.IsColumnExists("ReceivedFromName"))
            {
                letterModel.ReceivedFromName = SqlHelper.GetNullableString(reader, "ReceivedFromName");
            }

            if (reader.IsColumnExists("NeedReply"))
            {
                letterModel.NeedReply = SqlHelper.GetBoolean(reader, "NeedReply");
            }

            if (reader.IsColumnExists("Priority"))
            {
                letterModel.Priority = SqlHelper.GetNullableString(reader, "Priority");
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

            if (reader.IsColumnExists("Notes"))
            {
                letterModel.Notes = SqlHelper.GetNullableString(reader, "Notes");
            }

            if (reader.IsColumnExists("LetterType"))
            {
                letterModel.LetterType = SqlHelper.GetNullableString(reader, "LetterType");
            }

            if (reader.IsColumnExists("IsGovernmentEntity"))
            {
                letterModel.IsGovernmentEntity = SqlHelper.GetBoolean(reader, "IsGovernmentEntity");
            }

            if (reader.IsColumnExists("ReceivedFromEntityID"))
            {
                letterModel.ReceivedFromEntityID = SqlHelper.GetNullableInt32(reader, "ReceivedFromEntityID");
            }

            if (reader.IsColumnExists("IsRedirect"))
                letterModel.IsRedirect = SqlHelper.GetNullableInt32(reader, "IsRedirect");

            return letterModel;
        }

        public static List<LetterInboundGetModel> TranslateAsLetterInboundList(this SqlDataReader reader)
        {
            var letterList = new List<LetterInboundGetModel>();
            while (reader.Read())
            {
                letterList.Add(TranslateAsLetterInbound(reader, true));
            }

            return letterList;
        }

        public static LetterInboundPutModel TranslateAsPutLetterInbound(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var letterModel = new LetterInboundPutModel();

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

            if (reader.IsColumnExists("ReceivingDate"))
            {
                letterModel.ReceivingDate = SqlHelper.GetDateTime(reader, "ReceivingDate");
            }

            if (reader.IsColumnExists("ReceivedFromGovernmentEntity"))
            {
                letterModel.ReceivedFromGovernmentEntity = SqlHelper.GetNullableString(reader, "ReceivedFromGovernmentEntity");
            }

            if (reader.IsColumnExists("ReceivedFromName"))
            {
                letterModel.ReceivedFromName = SqlHelper.GetNullableString(reader, "ReceivedFromName");
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

            if (reader.IsColumnExists("LetterPhysicallySend"))
            {
                letterModel.LetterPhysicallySend = SqlHelper.GetBoolean(reader, "LetterPhysicallySend");
            }

            if (reader.IsColumnExists("NeedReply"))
            {
                letterModel.NeedReply = SqlHelper.GetBoolean(reader, "NeedReply");
            }

            if (reader.IsColumnExists("Priority"))
            {
                letterModel.Priority = SqlHelper.GetNullableString(reader, "Priority");
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

            if (reader.IsColumnExists("Notes"))
            {
                letterModel.Notes = SqlHelper.GetNullableString(reader, "Notes");
            }

            if (reader.IsColumnExists("LetterReferenceID"))
            {
                letterModel.LetterReferenceNumber = SqlHelper.GetNullableString(reader, "LetterReferenceID");
            }

            if (reader.IsColumnExists("IsGovernmentEntity"))
            {
                letterModel.IsGovernmentEntity = SqlHelper.GetBoolean(reader, "IsGovernmentEntity");
            }

            if (reader.IsColumnExists("ReceivedFromEntityID"))
            {
                letterModel.ReceivedFromEntityID = SqlHelper.GetNullableInt32(reader, "ReceivedFromEntityID");
            }

            return letterModel;
        }

        public static List<LetterInboundPutModel> TranslateAsPutLetterInboundList(this SqlDataReader reader)
        {
            var letterList = new List<LetterInboundPutModel>();
            while (reader.Read())
            {
                letterList.Add(TranslateAsPutLetterInbound(reader, true));
            }

            return letterList;
        }

        public static LetterInboundDashboardListModel TranslateAsLetterInboundDashboard(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var letterDashboardModel = new LetterInboundDashboardListModel();

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

            if (reader.IsColumnExists("UserName"))
            {
                letterDashboardModel.UserName = SqlHelper.GetNullableString(reader, "UserName");
            }

            if (reader.IsColumnExists("Status"))
            {
                letterDashboardModel.Status = SqlHelper.GetNullableString(reader, "Status");
            }

            if (reader.IsColumnExists("StatusCode"))
            {
                letterDashboardModel.StatusCode = SqlHelper.GetNullableString(reader, "StatusCode");
            }

            if (reader.IsColumnExists("LinkToOtherLetter"))
            {
                letterDashboardModel.LinkedToOtherLetter = SqlHelper.GetNullableString(reader, "LinkToOtherLetter");
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

            if (reader.IsColumnExists("SenderName"))
            {
                letterDashboardModel.SenderName = SqlHelper.GetNullableString(reader, "SenderName");
            }

            if (reader.IsColumnExists("SenderEntity"))
            {
                letterDashboardModel.SenderEntity = SqlHelper.GetNullableString(reader, "SenderEntity");
            }

            return letterDashboardModel;
        }

        public static List<LetterInboundDashboardListModel> TranslateAsLetterInboundDashboardList(this SqlDataReader reader)
        {
            var letterDashboardList = new List<LetterInboundDashboardListModel>();
            while (reader.Read())
            {
                letterDashboardList.Add(TranslateAsLetterInboundDashboard(reader, true));
            }

            return letterDashboardList;
        }

        public static LetterInBoundPreviewModel TranslateAsLetterInboundPreview(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var letterModel = new LetterInBoundPreviewModel();

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

        public static List<LetterInBoundPreviewModel> TranslateAsLetterInboundPreviewList(this SqlDataReader reader)
        {
            var letterList = new List<LetterInBoundPreviewModel>();
            while (reader.Read())
            {
                letterList.Add(TranslateAsLetterInboundPreview(reader, true));
            }

            return letterList;
        }
    }
}
