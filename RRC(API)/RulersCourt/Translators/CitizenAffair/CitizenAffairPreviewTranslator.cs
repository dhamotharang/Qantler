using RulersCourt.Models.CitizenAffair;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.CitizenAffair
{
    public static class CitizenAffairPreviewTranslator
    {
        public static CitizenAffairPreview_model TranslateAsGetCitizenAffair(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var citizenAffair = new CitizenAffairPreview_model();

            if (reader.IsColumnExists("CitizenAffairID"))
            {
                citizenAffair.CitizenAffairID = SqlHelper.GetNullableInt32(reader, "CitizenAffairID");
            }

            if (reader.IsColumnExists("SourceOU"))
            {
                citizenAffair.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");
            }

            if (reader.IsColumnExists("SourceName"))
            {
                citizenAffair.SourceName = SqlHelper.GetNullableString(reader, "SourceName");
            }

            if (reader.IsColumnExists("Status"))
            {
                citizenAffair.Status = SqlHelper.GetNullableInt32(reader, "Status");
            }

            if (reader.IsColumnExists("CurrentApproverID"))
            {
                citizenAffair.CurrentApproverID = SqlHelper.GetNullableInt32(reader, "CurrentApproverID");
            }

            if (reader.IsColumnExists("ReferenceNumber"))
            {
                citizenAffair.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");
            }

            if (reader.IsColumnExists("RequestType"))
            {
                citizenAffair.RequestType = SqlHelper.GetNullableString(reader, "RequestType");
            }

            if (reader.IsColumnExists("ApproverNameID"))
            {
                citizenAffair.ApproverNameId = SqlHelper.GetNullableString(reader, "ApproverNameID");
            }

            if (reader.IsColumnExists("ApproverID"))
            {
                citizenAffair.ApproverID = SqlHelper.GetNullableInt32(reader, "ApproverID");
            }

            if (reader.IsColumnExists("InitalApproverDepartmentID"))
            {
                citizenAffair.ApproverDepartmentId = SqlHelper.GetNullableInt32(reader, "InitalApproverDepartmentID");
            }

            if (reader.IsColumnExists("NotifyUpon"))
            {
                citizenAffair.NotifyUpon = SqlHelper.GetNullableString(reader, "NotifyUpon");
            }

            if (reader.IsColumnExists("InternalRequestorID"))
            {
                citizenAffair.InternalRequestorID = SqlHelper.GetNullableInt32(reader, "InternalRequestorID");
            }

            if (reader.IsColumnExists("InternalRequestorDepartmentID"))
            {
                citizenAffair.InternalRequestorDepartmentID = SqlHelper.GetNullableInt32(reader, "InternalRequestorDepartmentID");
            }

            if (reader.IsColumnExists("ExternalRequestEmailID"))
            {
                citizenAffair.ExternalRequestEmailID = SqlHelper.GetNullableString(reader, "ExternalRequestEmailID");
            }

            if (reader.IsColumnExists("CreatedBy"))
            {
                citizenAffair.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");
            }

            if (reader.IsColumnExists("Creator"))
            {
                citizenAffair.Creator = SqlHelper.GetNullableString(reader, "Creator");
            }

            if (reader.IsColumnExists("CreatedDateTime"))
            {
                citizenAffair.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");
            }

            if (reader.IsColumnExists("UpdatedDateTime"))
            {
                citizenAffair.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");
            }

            if (reader.IsColumnExists("SignaturePhotoApprover"))
            {
                citizenAffair.SignaturePhotoApprover = SqlHelper.GetNullableString(reader, "SignaturePhotoApprover");
            }

            if (reader.IsColumnExists("SignaturePhotoCreator"))
            {
                citizenAffair.SignaturePhotoCreator = SqlHelper.GetNullableString(reader, "SignaturePhotoCreator");
            }

            if (reader.IsColumnExists("Source"))
            {
                citizenAffair.Source = SqlHelper.GetNullableString(reader, "Source");
            }

            return citizenAffair;
        }

        public static List<CitizenAffairPreview_model> TranslateAsCitizenAffairPreviewList(this SqlDataReader reader)
        {
            var citizenAffairList = new List<CitizenAffairPreview_model>();
            while (reader.Read())
            {
                citizenAffairList.Add(TranslateAsGetCitizenAffair(reader, true));
            }

            return citizenAffairList;
        }
    }
}