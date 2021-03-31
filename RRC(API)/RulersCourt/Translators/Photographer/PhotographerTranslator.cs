using RulersCourt.Models.Photographer;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Protocol.Media.Photographer
{
    public static class PhotographerTranslator
    {
        public static PhotographerGetModel TranslateAsPhotographer(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var photographerModel = new PhotographerGetModel();

            if (reader.IsColumnExists("PhotoGrapherID"))
                photographerModel.PhotoGrapherID = SqlHelper.GetNullableInt32(reader, "PhotoGrapherID");

            if (reader.IsColumnExists("ReferenceNumber"))
                photographerModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("Date"))
                photographerModel.Date = SqlHelper.GetDateTime(reader, "Date");

            if (reader.IsColumnExists("SourceOU"))
                photographerModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                photographerModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("EventDate"))
                photographerModel.EventDate = SqlHelper.GetDateTime(reader, "EventDate");

            if (reader.IsColumnExists("Location"))
                photographerModel.Location = SqlHelper.GetNullableString(reader, "Location");

            if (reader.IsColumnExists("EventName"))
                photographerModel.EventName = SqlHelper.GetNullableString(reader, "EventName");

            if (reader.IsColumnExists("CreatedBy"))
                photographerModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                photographerModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("Status"))
                photographerModel.Status = SqlHelper.GetNullableInt32(reader, "Status");

            if (reader.IsColumnExists("CreatedDateTime"))
                photographerModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                photographerModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("ApproverDepartmentID"))
                photographerModel.ApproverDepartmentID = SqlHelper.GetNullableInt32(reader, "ApproverDepartmentID");

            if (reader.IsColumnExists("ApproverID"))
                photographerModel.ApproverID = SqlHelper.GetNullableInt32(reader, "ApproverID");

            return photographerModel;
        }

        public static List<PhotographerGetModel> TranslateAsPhotographerList(this SqlDataReader reader)
        {
            var photographerList = new List<PhotographerGetModel>();
            while (reader.Read())
            {
                photographerList.Add(TranslateAsPhotographer(reader, true));
            }

            return photographerList;
        }

        public static PhotographerPutModel TranslateAsPutPhotographer(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var photographerModel = new PhotographerPutModel();

            if (reader.IsColumnExists("PhotographerID"))
                photographerModel.PhotographerID = SqlHelper.GetNullableInt32(reader, "PhotographerID");

            if (reader.IsColumnExists("Date"))
                photographerModel.Date = SqlHelper.GetDateTime(reader, "Date");

            if (reader.IsColumnExists("SourceOU"))
                photographerModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                photographerModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("EventDate"))
                photographerModel.EventDate = SqlHelper.GetDateTime(reader, "EventDate");

            if (reader.IsColumnExists("Location"))
                photographerModel.Location = SqlHelper.GetNullableString(reader, "Location");

            if (reader.IsColumnExists("EventName"))
                photographerModel.EventName = SqlHelper.GetNullableString(reader, "EventName");

            if (reader.IsColumnExists("UpdatedBy"))
                photographerModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                photographerModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("Action"))
                photographerModel.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("Comments"))
                photographerModel.Comments = SqlHelper.GetNullableString(reader, "Comments");

            if (reader.IsColumnExists("ApproverID"))
                photographerModel.ApproverID = SqlHelper.GetNullableInt32(reader, "ApproverID");

            if (reader.IsColumnExists("ApproverDepartmentID"))
                photographerModel.ApproverDepartmentID = SqlHelper.GetNullableInt32(reader, "ApproverDepartmentID");

            return photographerModel;
        }

        public static List<PhotographerPutModel> TranslateAsPutPhotographerList(this SqlDataReader reader)
        {
            var photographerList = new List<PhotographerPutModel>();
            while (reader.Read())
            {
                photographerList.Add(TranslateAsPutPhotographer(reader, true));
            }

            return photographerList;
        }
    }
}
