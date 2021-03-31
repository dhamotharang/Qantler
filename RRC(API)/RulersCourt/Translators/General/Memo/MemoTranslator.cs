using RulersCourt.Models;
using RulersCourt.Models.General.Memo;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class MemoTranslator
    {
        public static MemoGetModel TranslateAsMemo(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var memoModel = new MemoGetModel();

            if (reader.IsColumnExists("MemoID"))
                memoModel.MemoID = SqlHelper.GetNullableInt32(reader, "MemoID");

            if (reader.IsColumnExists("ReferenceNumber"))
                memoModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("Title"))
                memoModel.Title = SqlHelper.GetNullableString(reader, "Title");

            if (reader.IsColumnExists("SourceOU"))
                memoModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                memoModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("Details"))
                memoModel.Details = SqlHelper.GetNullableString(reader, "Details");

            if (reader.IsColumnExists("Private"))
                memoModel.Private = SqlHelper.GetNullableString(reader, "Private");

            if (reader.IsColumnExists("Priority"))
                memoModel.Priority = SqlHelper.GetNullableString(reader, "Priority");

            if (reader.IsColumnExists("CreatedBy"))
                memoModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                memoModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                memoModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                memoModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("Status"))
                memoModel.Status = SqlHelper.GetNullableString(reader, "Status");

            if (reader.IsColumnExists("StatusCode"))
                memoModel.StatusCode = SqlHelper.GetNullableString(reader, "StatusCode");

            if (reader.IsColumnExists("ApproverDepartmentID"))
                memoModel.ApproverDepartmentID = SqlHelper.GetNullableString(reader, "ApproverDepartmentID");

            if (reader.IsColumnExists("ApproverNameID"))
                memoModel.ApproverNameID = SqlHelper.GetNullableString(reader, "ApproverNameID");

            if (reader.IsColumnExists("Destination"))
                memoModel.Destination = SqlHelper.GetNullableString(reader, "Destination");

            if (reader.IsColumnExists("IsRedirect"))
                memoModel.IsRedirect = SqlHelper.GetNullableInt32(reader, "IsRedirect");

            return memoModel;
        }

        public static List<MemoGetModel> TranslateAsMemoList(this SqlDataReader reader)
        {
            var memoList = new List<MemoGetModel>();
            while (reader.Read())
            {
                memoList.Add(TranslateAsMemo(reader, true));
            }

            return memoList;
        }

        public static MemoPutModel TranslateAsPutMemo(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var memoModel = new MemoPutModel();

            if (reader.IsColumnExists("MemoID"))
                memoModel.MemoID = SqlHelper.GetNullableInt32(reader, "MemoID");

            if (reader.IsColumnExists("Title"))
                memoModel.Title = SqlHelper.GetNullableString(reader, "Title");

            if (reader.IsColumnExists("SourceOU"))
                memoModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                memoModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("ApproverId"))
                memoModel.ApproverId = SqlHelper.GetNullableInt32(reader, "ApproverId");

            if (reader.IsColumnExists("ApproverDepartmentId"))
                memoModel.ApproverDepartmentId = SqlHelper.GetNullableInt32(reader, "ApproverDepartmentId");

            if (reader.IsColumnExists("Details"))
                memoModel.Details = SqlHelper.GetNullableString(reader, "Details");

            if (reader.IsColumnExists("Private"))
                memoModel.Private = SqlHelper.GetNullableString(reader, "Private");

            if (reader.IsColumnExists("Priority"))
                memoModel.Priority = SqlHelper.GetNullableString(reader, "Priority");

            if (reader.IsColumnExists("UpdatedBy"))
                memoModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                memoModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("Action"))
                memoModel.Action = SqlHelper.GetNullableString(reader, "Action");

            return memoModel;
        }

        public static List<MemoPutModel> TranslateAsPutMemoList(this SqlDataReader reader)
        {
            var memoList = new List<MemoPutModel>();
            while (reader.Read())
            {
                memoList.Add(TranslateAsPutMemo(reader, true));
            }

            return memoList;
        }

        public static MemoPreviewModel TranslateAsMemoPreview(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var memoModel = new MemoPreviewModel();

            if (reader.IsColumnExists("MemoID"))
                memoModel.MemoID = SqlHelper.GetNullableInt32(reader, "MemoID");

            if (reader.IsColumnExists("ReferenceNumber"))
                memoModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("Title"))
                memoModel.Title = SqlHelper.GetNullableString(reader, "Title");

            if (reader.IsColumnExists("SourceOU"))
                memoModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                memoModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("DestinationTitle"))
                memoModel.DestinationTitle = SqlHelper.GetNullableString(reader, "DestinationTitle");

            if (reader.IsColumnExists("Details"))
                memoModel.Details = SqlHelper.GetNullableString(reader, "Details");

            if (reader.IsColumnExists("Private"))
                memoModel.Private = SqlHelper.GetNullableString(reader, "Private");

            if (reader.IsColumnExists("Priority"))
                memoModel.Priority = SqlHelper.GetNullableString(reader, "Priority");

            if (reader.IsColumnExists("CreatedBy"))
                memoModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                memoModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                memoModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                memoModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("Status"))
                memoModel.Status = SqlHelper.GetNullableString(reader, "Status");

            if (reader.IsColumnExists("SignaturePhotoApprover"))
                memoModel.SignaturePhotoApprover = SqlHelper.GetNullableString(reader, "SignaturePhotoApprover");

            if (reader.IsColumnExists("SignaturePhotoRedirector"))
                memoModel.SignaturePhotoRedirector = SqlHelper.GetNullableString(reader, "SignaturePhotoRedirector");

            if (reader.IsColumnExists("RedirectID"))
                memoModel.RedirectID = SqlHelper.GetNullableInt32(reader, "RedirectID");

            if (reader.IsColumnExists("ApproverID"))
                memoModel.ApproverID = SqlHelper.GetNullableInt32(reader, "ApproverID");

            if (reader.IsColumnExists("RedirectNameID"))
                memoModel.RedirectNameID = SqlHelper.GetNullableString(reader, "RedirectNameID");

            if (reader.IsColumnExists("RedirectReferenceNumber"))
                memoModel.RedirectReferenceNumber = SqlHelper.GetNullableString(reader, "RedirectReferenceNumber");

            if (reader.IsColumnExists("Comments"))
                memoModel.Comments = SqlHelper.GetNullableString(reader, "Comments");

            if (reader.IsColumnExists("ApproverDesignation"))
                memoModel.ApproverDesignation = SqlHelper.GetNullableString(reader, "ApproverDesignation");

            if (reader.IsColumnExists("ApproverDepartmentID"))
                memoModel.ApproverDepartmentID = SqlHelper.GetNullableString(reader, "ApproverDepartmentID");

            if (reader.IsColumnExists("ApproverNameID"))
                memoModel.ApproverNameID = SqlHelper.GetNullableString(reader, "ApproverNameID");

            if (reader.IsColumnExists("Destination"))
                memoModel.Destination = SqlHelper.GetNullableString(reader, "Destination");

            return memoModel;
        }

        public static List<MemoPreviewModel> TranslateAsMemoPreviewList(this SqlDataReader reader)
        {
            var memoList = new List<MemoPreviewModel>();
            while (reader.Read())
            {
                memoList.Add(TranslateAsMemoPreview(reader, true));
            }

            return memoList;
        }
    }
}