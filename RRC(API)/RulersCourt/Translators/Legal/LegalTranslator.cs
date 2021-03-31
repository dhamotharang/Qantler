using RulersCourt.Models.Legal;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class LegalTranslator
    {
        public static LegalGetModel TranslateAsLegal(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var legalModel = new LegalGetModel();

            if (reader.IsColumnExists("LegalID"))
                legalModel.LegalID = SqlHelper.GetNullableInt32(reader, "LegalID");

            if (reader.IsColumnExists("ReferenceNumber"))
                legalModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("SourceOU"))
                legalModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                legalModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("Subject"))
                legalModel.Subject = SqlHelper.GetNullableString(reader, "Subject");

            if (reader.IsColumnExists("RequestDetails"))
                legalModel.RequestDetails = SqlHelper.GetNullableString(reader, "RequestDetails");

            if (reader.IsColumnExists("CreatedBy"))
                legalModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                legalModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("Status"))
                legalModel.Status = SqlHelper.GetNullableString(reader, "Status");

            if (reader.IsColumnExists("Comments"))
                legalModel.Comments = SqlHelper.GetNullableString(reader, "Comments");

            if (reader.IsColumnExists("CreatedDateTime"))
                legalModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                legalModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            return legalModel;
        }

        public static List<LegalGetModel> TranslateAsLegalList(this SqlDataReader reader)
        {
            var legalList = new List<LegalGetModel>();
            while (reader.Read())
            {
                legalList.Add(TranslateAsLegal(reader, true));
            }

            return legalList;
        }

        public static LegalPutModel TranslateAsPutLegal(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var legalModel = new LegalPutModel();

            if (reader.IsColumnExists("LegalID"))
                legalModel.LegalID = SqlHelper.GetNullableInt32(reader, "LegalID");

            if (reader.IsColumnExists("SourceOU"))
                legalModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                legalModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("Subject"))
                legalModel.Subject = SqlHelper.GetNullableString(reader, "Subject");

            if (reader.IsColumnExists("RequestDetails"))
                legalModel.RequestDetails = SqlHelper.GetNullableString(reader, "RequestDetails");

            if (reader.IsColumnExists("UpdatedBy"))
                legalModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                legalModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("Action"))
                legalModel.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("Comments"))
                legalModel.Comments = SqlHelper.GetNullableString(reader, "Comments");

            return legalModel;
        }

        public static List<LegalPutModel> TranslateAsPutLegalList(this SqlDataReader reader)
        {
            var legalList = new List<LegalPutModel>();
            while (reader.Read())
            {
                legalList.Add(TranslateAsPutLegal(reader, true));
            }

            return legalList;
        }

        public static LegalDashboardListModel TranslateAsDashboardLegal(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var legalDashboardListModel = new LegalDashboardListModel();

            if (reader.IsColumnExists("LegalID"))
                legalDashboardListModel.LegalID = SqlHelper.GetNullableInt32(reader, "LegalID");

            if (reader.IsColumnExists("ReferenceNumber"))
                legalDashboardListModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("Subject"))
                legalDashboardListModel.Subject = SqlHelper.GetNullableString(reader, "Subject");

            if (reader.IsColumnExists("SourceOU"))
                legalDashboardListModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("Status"))
                legalDashboardListModel.Status = SqlHelper.GetNullableString(reader, "Status");

            if (reader.IsColumnExists("RequestDate"))
                legalDashboardListModel.RequestDate = SqlHelper.GetDateTime(reader, "RequestDate");

            if (reader.IsColumnExists("Attendedby"))
                legalDashboardListModel.Attendedby = SqlHelper.GetNullableString(reader, "Attendedby");

            if (reader.IsColumnExists("AssignedTo"))
                legalDashboardListModel.AssignedTo = SqlHelper.GetNullableString(reader, "AssignedTo");

            return legalDashboardListModel;
        }

        public static List<LegalDashboardListModel> TranslateAsDashboardLegalList(this SqlDataReader reader)
        {
            var legalDashboardList = new List<LegalDashboardListModel>();
            while (reader.Read())
            {
                legalDashboardList.Add(TranslateAsDashboardLegal(reader, true));
            }

            return legalDashboardList;
        }

        public static LegalHomeDashboardModel TranslateaslegalDashboardcount(this SqlDataReader reader)
        {
            var legalhomemodel = new LegalHomeDashboardModel();
            while (reader.Read())
            {
                legalhomemodel = TranslateAslegalDashboardCount(reader, true);
            }

            return legalhomemodel;
        }

        public static LegalHomeDashboardModel TranslateAslegalDashboardCount(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var legalhomemodel = new LegalHomeDashboardModel();

            if (reader.IsColumnExists("New"))
            {
                legalhomemodel.New = SqlHelper.GetNullableInt32(reader, "New");
            }

            if (reader.IsColumnExists("NeedMoreInfo"))
            {
                legalhomemodel.NeedMoreInfo = SqlHelper.GetNullableInt32(reader, "NeedMoreInfo");
            }

            if (reader.IsColumnExists("Closed"))
            {
                legalhomemodel.Closed = SqlHelper.GetNullableInt32(reader, "Closed");
            }

            if (reader.IsColumnExists("MyOwnRequest"))
            {
                legalhomemodel.MyOwnRequest = SqlHelper.GetNullableInt32(reader, "MyOwnRequest");
            }

            if (reader.IsColumnExists("MyPendingRequest"))
            {
                legalhomemodel.MyPendingRequest = SqlHelper.GetNullableInt32(reader, "MyPendingRequest");
            }

            if (reader.IsColumnExists("InProgressRequest"))
            {
                legalhomemodel.InProgressRequest = SqlHelper.GetNullableInt32(reader, "InProgressRequest");
            }

            return legalhomemodel;
        }
    }
}