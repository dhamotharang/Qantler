using RulersCourt.Models.DiwanIdentity;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Protocol.Media.DiwanIdentity
{
    public static class DiwanIdentityTranslator
    {
        public static DiwanIdentityGetModel TranslateAsDiwanIdentity(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var diwanIdentityModel = new DiwanIdentityGetModel();

            if (reader.IsColumnExists("DiwanIdentityID"))
                diwanIdentityModel.DiwanIdentityID = SqlHelper.GetNullableInt32(reader, "DiwanIdentityID");

            if (reader.IsColumnExists("ReferenceNumber"))
                diwanIdentityModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("Date"))
                diwanIdentityModel.Date = SqlHelper.GetDateTime(reader, "Date");

            if (reader.IsColumnExists("SourceOU"))
                diwanIdentityModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                diwanIdentityModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("PurposeofUse"))
                diwanIdentityModel.PurposeofUse = SqlHelper.GetNullableString(reader, "PurposeofUse");

            if (reader.IsColumnExists("CreatedBy"))
                diwanIdentityModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                diwanIdentityModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("Status"))
                diwanIdentityModel.Status = SqlHelper.GetNullableString(reader, "Status");

            if (reader.IsColumnExists("Comments"))
                diwanIdentityModel.Comments = SqlHelper.GetNullableString(reader, "Comments");

            if (reader.IsColumnExists("CreatedDateTime"))
                diwanIdentityModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                diwanIdentityModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("ApproverDepartmentID"))
                diwanIdentityModel.ApproverDepartmentID = SqlHelper.GetNullableInt32(reader, "ApproverDepartmentID");

            if (reader.IsColumnExists("ApproverNameID"))
                diwanIdentityModel.ApproverID = SqlHelper.GetNullableInt32(reader, "ApproverNameID");

            return diwanIdentityModel;
        }

        public static List<DiwanIdentityGetModel> TranslateAsDiwanIdentityList(this SqlDataReader reader)
        {
            var diwanIdentityList = new List<DiwanIdentityGetModel>();
            while (reader.Read())
            {
                diwanIdentityList.Add(TranslateAsDiwanIdentity(reader, true));
            }

            return diwanIdentityList;
        }

        public static DiwanIdentityPutModel TranslateAsPutDiwanIdentity(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var diwanIdentityModel = new DiwanIdentityPutModel();

            if (reader.IsColumnExists("DiwanIdentityID"))
                diwanIdentityModel.DiwanIdentityID = SqlHelper.GetNullableInt32(reader, "DiwanIdentityID");

            if (reader.IsColumnExists("Date"))
                diwanIdentityModel.Date = SqlHelper.GetDateTime(reader, "Date");

            if (reader.IsColumnExists("SourceOU"))
                diwanIdentityModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                diwanIdentityModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("PurposeofUse"))
                diwanIdentityModel.PurposeofUse = SqlHelper.GetNullableString(reader, "PurposeofUse");

            if (reader.IsColumnExists("UpdatedBy"))
                diwanIdentityModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                diwanIdentityModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("Action"))
                diwanIdentityModel.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("Comments"))
                diwanIdentityModel.Comments = SqlHelper.GetNullableString(reader, "Comments");

            if (reader.IsColumnExists("ApproverID"))
                diwanIdentityModel.ApproverID = SqlHelper.GetNullableInt32(reader, "ApproverID");

            if (reader.IsColumnExists("ApproverDepartmentID"))
                diwanIdentityModel.ApproverDepartmentID = SqlHelper.GetNullableInt32(reader, "ApproverDepartmentID");

            return diwanIdentityModel;
        }

        public static List<DiwanIdentityPutModel> TranslateAsPutDiwanIdentityList(this SqlDataReader reader)
        {
            var diwanIdentityList = new List<DiwanIdentityPutModel>();
            while (reader.Read())
            {
                diwanIdentityList.Add(TranslateAsPutDiwanIdentity(reader, true));
            }

            return diwanIdentityList;
        }
    }
}
