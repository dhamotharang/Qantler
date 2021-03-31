using RulersCourt.Models.Gift;
using RulersCourt.Repository.Gift;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Gift
{
    public static class GiftTranslator
    {
        public static GiftGetModel TranslateAsGift(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var giftSave = new GiftGetModel();

            if (reader.IsColumnExists("GiftID"))
            {
                giftSave.GiftID = SqlHelper.GetNullableInt32(reader, "GiftID");
            }

            if (reader.IsColumnExists("ReferenceNumber"))
            {
                giftSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");
            }

            if (reader.IsColumnExists("GiftType"))
            {
                giftSave.GiftType = SqlHelper.GetNullableInt32(reader, "GiftType");
            }

            if (reader.IsColumnExists("RecievedFromOrganization"))
            {
                giftSave.RecievedFromOrganization = SqlHelper.GetNullableString(reader, "RecievedFromOrganization");
            }

            if (reader.IsColumnExists("RecievedFromName"))
            {
                giftSave.RecievedFromName = SqlHelper.GetNullableString(reader, "RecievedFromName");
            }

            if (reader.IsColumnExists("RecievedDate"))
            {
                giftSave.RecievedDate = SqlHelper.GetDateTime(reader, "RecievedDate");
            }

            if (reader.IsColumnExists("PurchasedBy"))
            {
                giftSave.PurchasedBy = SqlHelper.GetNullableInt32(reader, "PurchasedBy");
            }

            if (reader.IsColumnExists("PurchasedToName"))
            {
                giftSave.PurchasedToName = SqlHelper.GetNullableString(reader, "PurchasedToName");
            }

            if (reader.IsColumnExists("PurchasedToOrganisation"))
            {
                giftSave.PurchasedToOrganisation = SqlHelper.GetNullableString(reader, "PurchasedToOrganisation");
            }

            if (reader.IsColumnExists("PurchasedBy"))
            {
                giftSave.PurchasedBy = SqlHelper.GetNullableInt32(reader, "PurchasedBy");
            }

            if (reader.IsColumnExists("IsSend"))
            {
                giftSave.IsSend = SqlHelper.GetBoolean(reader, "IsSend");
            }

            if (reader.IsColumnExists("Status"))
            {
                giftSave.Status = SqlHelper.GetNullableInt32(reader, "Status");
            }

            if (reader.IsColumnExists("CreatedBy"))
            {
                giftSave.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");
            }

            if (reader.IsColumnExists("UpdatedBy"))
            {
                giftSave.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");
            }

            if (reader.IsColumnExists("CreatedDateTime"))
            {
                giftSave.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");
            }

            if (reader.IsColumnExists("UpdatedDateTime"))
            {
                giftSave.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");
            }

            return giftSave;
        }

        public static List<GiftGetModel> TranslateAsGiftList(this SqlDataReader reader)
        {
            var giftList = new List<GiftGetModel>();
            while (reader.Read())
            {
                giftList.Add(TranslateAsGift(reader, true));
            }

            return giftList;
        }

        public static GiftPutModel TranslateAsPutGift(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var giftPut = new GiftPutModel();

            if (reader.IsColumnExists("GiftID"))
            {
                giftPut.GiftID = SqlHelper.GetNullableInt32(reader, "GiftID");
            }

            if (reader.IsColumnExists("GiftType"))
            {
                giftPut.GiftType = SqlHelper.GetNullableInt32(reader, "GiftType");
            }

            if (reader.IsColumnExists("RecievedFromOrganization"))
            {
                giftPut.RecievedFromOrganization = SqlHelper.GetNullableString(reader, "RecievedFromOrganization");
            }

            if (reader.IsColumnExists("RecievedFromName"))
            {
                giftPut.RecievedFromName = SqlHelper.GetNullableString(reader, "RecievedFromName");
            }

            if (reader.IsColumnExists("RecievedDate"))
            {
                giftPut.RecievedDate = SqlHelper.GetDateTime(reader, "RecievedDate");
            }

            if (reader.IsColumnExists("PurchasedBy"))
            {
                giftPut.PurchasedBy = SqlHelper.GetNullableInt32(reader, "PurchasedBy");
            }

            if (reader.IsColumnExists("PurchasedToName"))
            {
                giftPut.PurchasedToName = SqlHelper.GetNullableString(reader, "PurchasedToName");
            }

            if (reader.IsColumnExists("PurchasedToOrganisation"))
            {
                giftPut.PurchasedToOrganisation = SqlHelper.GetNullableString(reader, "PurchasedToOrganisation");
            }

            if (reader.IsColumnExists("PurchasedBy"))
            {
                giftPut.PurchasedBy = SqlHelper.GetNullableInt32(reader, "PurchasedBy");
            }

            if (reader.IsColumnExists("UpdatedBy"))
            {
                giftPut.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");
            }

            if (reader.IsColumnExists("UpdatedDateTime"))
            {
                giftPut.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");
            }

            return giftPut;
        }

        public static GiftPutModel TranslateAsGiftPutList(this SqlDataReader reader)
        {
            var giftList = new GiftPutModel();
            while (reader.Read())
            {
                giftList = TranslateAsPutGift(reader, true);
            }

            return giftList;
        }

        public static GiftModel TranslateAsGiftDashboard(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var giftDashboardModel = new GiftModel();

            if (reader.IsColumnExists("GiftID"))
            {
                giftDashboardModel.GiftID = SqlHelper.GetNullableInt32(reader, "GiftID");
            }

            if (reader.IsColumnExists("ReferenceNumber"))
            {
                giftDashboardModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");
            }

            if (reader.IsColumnExists("GiftType"))
            {
                giftDashboardModel.GiftType = SqlHelper.GetNullableString(reader, "GiftType");
            }

            if (reader.IsColumnExists("PurchasedBy"))
            {
                giftDashboardModel.PurchasedBy = SqlHelper.GetNullableString(reader, "PurchasedBy");
            }

            if (reader.IsColumnExists("Status"))
            {
                giftDashboardModel.Status = SqlHelper.GetNullableString(reader, "Status");
            }

            return giftDashboardModel;
        }

        public static List<GiftModel> TranslateAsGiftDashboardList(this SqlDataReader reader)
        {
            var giftDashboardList = new List<GiftModel>();
            while (reader.Read())
            {
                giftDashboardList.Add(TranslateAsGiftDashboard(reader, true));
            }

            return giftDashboardList;
        }

        public static GiftDeliveryNoteModel TranslateAsGiftDeliveryNote(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var giftSave = new GiftDeliveryNoteModel();
            if (reader.IsColumnExists("GiftID"))
            {
                giftSave.GiftID = SqlHelper.GetNullableInt32(reader, "GiftID");
            }

            if (reader.IsColumnExists("ReferenceNumber"))
            {
                giftSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");
            }

            if (reader.IsColumnExists("GiftType"))
            {
                giftSave.GiftType = SqlHelper.GetNullableString(reader, "GiftType");
            }

            if (reader.IsColumnExists("RecievedFromOrganization"))
            {
                giftSave.RecievedFromOrganization = SqlHelper.GetNullableString(reader, "RecievedFromOrganization");
            }

            if (reader.IsColumnExists("RecievedFromName"))
            {
                giftSave.RecievedFromName = SqlHelper.GetNullableString(reader, "RecievedFromName");
            }

            if (reader.IsColumnExists("RecievedDate"))
            {
                giftSave.RecievedDate = SqlHelper.GetDateTime(reader, "RecievedDate");
            }

            if (reader.IsColumnExists("PurchasedBy"))
            {
                giftSave.PurchasedBy = SqlHelper.GetNullableString(reader, "PurchasedBy");
            }

            if (reader.IsColumnExists("PurchasedToName"))
            {
                giftSave.PurchasedToName = SqlHelper.GetNullableString(reader, "PurchasedToName");
            }

            if (reader.IsColumnExists("PurchasedToOrganisation"))
            {
                giftSave.PurchasedToOrganisation = SqlHelper.GetNullableString(reader, "PurchasedToOrganisation");
            }

            if (reader.IsColumnExists("PurchasedBy"))
            {
                giftSave.PurchasedBy = SqlHelper.GetNullableString(reader, "PurchasedBy");
            }

            if (reader.IsColumnExists("IsSend"))
            {
                giftSave.IsSend = SqlHelper.GetBoolean(reader, "IsSend");
            }

            if (reader.IsColumnExists("Status"))
            {
                giftSave.Status = SqlHelper.GetNullableInt32(reader, "Status");
            }

            if (reader.IsColumnExists("CreatedBy"))
            {
                giftSave.CreatedBy = SqlHelper.GetNullableString(reader, "CreatedBy");
            }

            if (reader.IsColumnExists("UpdatedBy"))
            {
                giftSave.UpdatedBy = SqlHelper.GetNullableString(reader, "UpdatedBy");
            }

            if (reader.IsColumnExists("CreatedDateTime"))
            {
                giftSave.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");
            }

            if (reader.IsColumnExists("UpdatedDateTime"))
            {
                giftSave.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");
            }

            return giftSave;
        }

        public static List<GiftDeliveryNoteModel> TranslateAsGiftDeliveryNoteList(this SqlDataReader reader)
        {
            var giftList = new List<GiftDeliveryNoteModel>();
            while (reader.Read())
            {
                giftList.Add(TranslateAsGiftDeliveryNote(reader, true));
            }

            return giftList;
        }

        public static GiftReportModel TranslateAsGiftReport(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var giftSave = new GiftReportModel();

            if (reader.IsColumnExists("ReferenceNumber"))
            {
                giftSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");
            }

            if (reader.IsColumnExists("GiftType"))
            {
                giftSave.GiftType = SqlHelper.GetNullableString(reader, "GiftType");
            }

            if (reader.IsColumnExists("RecievedFrom/PurchasedBy"))
            {
                giftSave.RecievedFromPurchasedBy = SqlHelper.GetNullableString(reader, "RecievedFrom/PurchasedBy");
            }

            if (reader.IsColumnExists("Status"))
            {
                giftSave.Status = SqlHelper.GetNullableString(reader, "Status");
            }

            if (reader.IsColumnExists("CreatedBy"))
            {
                giftSave.CreatedBy = SqlHelper.GetNullableString(reader, "CreatedBy");
            }

            return giftSave;
        }

        public static List<GiftReportModel> TranslateAsGiftReportList(this SqlDataReader reader)
        {
            var giftList = new List<GiftReportModel>();
            while (reader.Read())
            {
                giftList.Add(TranslateAsGiftReport(reader, true));
            }

            return giftList;
        }
    }
}
