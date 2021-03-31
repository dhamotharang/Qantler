using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.Gift;
using RulersCourt.Repository.CitizenAffair;
using RulersCourt.Translators;
using RulersCourt.Translators.Gift;
using RulersCourt.Translators.Legal;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Gift
{
    public class GiftClient
    {
        public GiftCountModel GetModuleCount(string connString, int userID, string lang)
        {
            GiftCountModel count = new GiftCountModel();
            SqlParameter[] parama = {
                new SqlParameter("@P_Type", "2"),
                new SqlParameter("@P_UserID", userID)
            };
            count.GiftsPurchased = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnString(connString, "Get_GiftCount", parama));

            SqlParameter[] param = {
                new SqlParameter("@P_Type", "1"),
                new SqlParameter("@P_UserID", userID)
            };
            count.GiftsReceived = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnString(connString, "Get_GiftCount", param));
            return count;
        }

        public GiftListModel GetGift(string connString, int pageNumber, int pageSize, string receivedPurchasedBy, string userID, string status, string smartSearch, string giftType, string lang)
        {
            GiftListModel list = new GiftListModel();

            SqlParameter[] param = {
                   new SqlParameter("@P_PageNumber", pageNumber),
                   new SqlParameter("@P_PageSize", pageSize),
                   new SqlParameter("@P_ReceivedPurchasedBy", receivedPurchasedBy),
                   new SqlParameter("@P_UserID", userID),
                   new SqlParameter("@P_Method", 1),
                   new SqlParameter("@P_Status", status),
                   new SqlParameter("@P_GiftType", giftType),
                   new SqlParameter("@P_Language", lang),
                   new SqlParameter("@P_SmartSearch", smartSearch)
            };

            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<GiftModel>>(connString, "Get_GiftList", r => r.TranslateAsGiftDashboardList(), param);

            SqlParameter[] parama = {
                   new SqlParameter("@P_PageNumber", pageNumber),
                   new SqlParameter("@P_PageSize", pageSize),
                   new SqlParameter("@P_ReceivedPurchasedBy", receivedPurchasedBy),
                   new SqlParameter("@P_UserID", userID),
                   new SqlParameter("@P_Method", 0),
                   new SqlParameter("@P_Status", status),
                   new SqlParameter("@P_GiftType", giftType),
                   new SqlParameter("@P_Language", lang),
                   new SqlParameter("@P_SmartSearch", smartSearch)
            };
            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_GiftList", parama);
            Parallel.Invoke(
              () => list.M_OrganizationList = GetM_OrganisationDashboard(connString, lang),
              () => list.M_LookupsList = GetM_LookupsDashboard(connString, lang));
            return list;
        }

        public GiftGetModel GetGiftID(string connString, int giftID, int? userID, string lang)
        {
            GiftGetModel giftDetails = new GiftGetModel();

            SqlParameter[] param = {
                new SqlParameter("@P_GiftID", giftID)
            };

            if (giftID != 0)
            {
                giftDetails = SqlHelper.ExecuteProcedureReturnData<List<GiftGetModel>>(connString, "Get_GiftByID", r => r.TranslateAsGiftList(), param).FirstOrDefault();
                giftDetails.GiftPhotos = new GiftAttachmentClient().GetAttachmentById(connString, giftDetails.GiftID, "GiftPhotos");
                giftDetails.HistoryLog = new GiftHistoryLogClient().GiftHistory(connString, giftDetails.GiftID, lang);
            }

            Parallel.Invoke(
              () => giftDetails.M_OrganizationList = GetM_Organisation(connString, lang),
              () => giftDetails.M_LookupsList = GetM_Lookups(connString, lang));
            return giftDetails;
        }

        public List<OrganizationModel> GetM_Organisation(string connString, string lang)
        {
            SqlParameter[] param = { new SqlParameter("@P_Language", lang) };
            var e = SqlHelper.ExecuteProcedureReturnData<List<OrganizationModel>>(connString, "Get_Organization", r => r.TranslateAsOrganizationList(), param);
            return e;
        }

        public List<OrganizationModel> GetM_OrganisationDashboard(string connString, string lang)
        {
            SqlParameter[] param = { new SqlParameter("@P_Language", lang) };
            List<OrganizationModel> res = new List<OrganizationModel>();
            OrganizationModel org = new OrganizationModel();
            if (lang == "EN")
            {
                org.OrganizationUnits = "All";
            }
            else
            {
                org.OrganizationUnits = "الكل";
            }

            res.Add(org);
            List<OrganizationModel> e = SqlHelper.ExecuteProcedureReturnData<List<OrganizationModel>>(connString, "Get_Organization", r => r.TranslateAsOrganizationList(), param);
            foreach (OrganizationModel m in e)
            {
                res.Add(m);
            }

            return res;
        }

        public List<M_LookupsModel> GetM_Lookups(string connString, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Type", "Gift"),
                new SqlParameter("@P_Language", lang)
            };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        public List<M_LookupsModel> GetM_LookupsDashboard(string connString, string lang)
        {
            List<M_LookupsModel> res = new List<M_LookupsModel>();
            M_LookupsModel org = new M_LookupsModel();
            if (lang == "EN")
            {
                org.DisplayName = "All";
            }
            else
            {
                org.DisplayName = "الكل";
            }

            res.Add(org);
            SqlParameter[] param = {
                new SqlParameter("@P_Type", "Gift"),
                new SqlParameter("@P_Language", lang)
            };
            List<M_LookupsModel> e = SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
            foreach (M_LookupsModel m in e)
            {
                res.Add(m);
            }

            return res;
        }

        public GiftSaveResponseModel PostGift(string connString, GiftPostModel gift)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_GiftType", gift.GiftType),
                new SqlParameter("@P_ReceivedFromOrganization", gift.RecievedFromOrganization),
                new SqlParameter("@P_ReceivedFromName", gift.RecievedFromName),
                new SqlParameter("@P_ReceivedDate", gift.RecievedDate),
                new SqlParameter("@P_PurchasedToOrganization", gift.PurchasedToOrganisation),
                new SqlParameter("@P_PurchasedToName", gift.PurchasedToName),
                new SqlParameter("@P_PurchasedBy", gift.PurchasedBy),
                new SqlParameter("@P_Action", gift.Action),
                new SqlParameter("@P_CreatedBy", gift.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", gift.CreatedDateTime),
                new SqlParameter("@P_Comments", gift.Comments)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<GiftSaveResponseModel>(connString, "Save_Gift", r => r.TranslateAsGiftSaveResponseList(), param);

            if (gift.GiftPhotos != null)
                new GiftAttachmentClient().GiftPostAttachments(connString, "GiftPhotos", gift.GiftPhotos, result.GiftID);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "Gift"),
                new SqlParameter("@P_Action", gift.Action),
                new SqlParameter("@P_GiftType", gift.GiftType)
            };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            return result;
        }

        public GiftSaveResponseModel PutGift(string connString, GiftPutModel gift)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_GiftType", gift.GiftType),
                new SqlParameter("@P_GiftID", gift.GiftID),
                new SqlParameter("@P_ReceivedFromOrganization", gift.RecievedFromOrganization),
                new SqlParameter("@P_ReceivedFromName", gift.RecievedFromName),
                new SqlParameter("@P_ReceivedDate", gift.RecievedDate),
                new SqlParameter("@P_PurchasedToOrganization", gift.PurchasedToOrganisation),
                new SqlParameter("@P_PurchasedToName", gift.PurchasedToName),
                new SqlParameter("@P_PurchasedBy", gift.PurchasedBy),
                new SqlParameter("@P_Action", gift.Action),
                new SqlParameter("@P_UpdatedBy", gift.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", gift.UpdatedDateTime),
                new SqlParameter("@P_Comments", gift.Comments)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<GiftSaveResponseModel>(connString, "Save_Gift", r => r.TranslateAsGiftSaveResponseList(), param);

            if (gift.GiftPhotos != null)
                new GiftAttachmentClient().GiftPostAttachments(connString, "GiftPhotos", gift.GiftPhotos, result.GiftID);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "Gift"),
                new SqlParameter("@P_Action", gift.Action)
            };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            return result;
        }

        public GiftPutModel GetPatchGiftByID(string connString, int giftID)
        {
            GiftPutModel giftDetails = new GiftPutModel();

            SqlParameter[] param = {
                new SqlParameter("@P_GiftID", giftID)
            };

            if (giftID != 0)
            {
                giftDetails = SqlHelper.ExecuteProcedureReturnData<GiftPutModel>(connString, "Get_LegalByID", r => r.TranslateAsGiftPutList(), param);

                giftDetails.GiftPhotos = new GiftAttachmentClient().GetAttachmentById(connString, giftDetails.GiftID, "GiftPhoto");
            }

            return giftDetails;
        }

        public GiftSaveResponseModel ConfirmGift(string connString, GiftConfirmModel gift)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_GiftID", gift.GiftID),
                new SqlParameter("@P_HandedOverTo", gift.HandedOverTo),
                new SqlParameter("@P_HandedOverDate", gift.HandedOverDate),
                new SqlParameter("@P_Action", "DeliveryConfirmed"),
                new SqlParameter("@P_UpdatedBy", gift.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", gift.UpdatedDateTime)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<GiftSaveResponseModel>(connString, "Save_Gift", r => r.TranslateAsGiftSaveResponseList(), param);

            if (gift.Attachment != null)
                new GiftAttachmentClient().GiftPostAttachments(connString, "GiftAttachments", gift.Attachment, result.GiftID);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "Gift"),
                new SqlParameter("@P_Action", "DeliveryConfirmed"),
                new SqlParameter("@P_GiftType", 2)
            };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            return result;
        }

        public GiftSaveResponseModel SendForDelviery(string connString, int id)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_GiftID", id),
                new SqlParameter("@P_Action", "SendforDelivery")
            };

            var result = SqlHelper.ExecuteProcedureReturnData<GiftSaveResponseModel>(connString, "Save_Gift", r => r.TranslateAsGiftSaveResponseList(), param);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "Gift"),
                new SqlParameter("@P_Action", "SendforDelivery"),
                new SqlParameter("@P_GiftType", 2)
            };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            return result;
        }

        public GiftDeliveryNoteModel GetGiftDeliveryNote(string connString, int giftID, string lang)
        {
            GiftDeliveryNoteModel giftDetails = new GiftDeliveryNoteModel();
            SqlParameter[] param = {
                new SqlParameter("@P_GiftID", giftID),
                new SqlParameter("@P_Language", lang)
            };

            if (giftID != 0)
            {
                giftDetails = SqlHelper.ExecuteProcedureReturnData(connString, "Get_GiftDelvieryNoteByID", r => r.TranslateAsGiftDeliveryNoteList(), param).FirstOrDefault();
                giftDetails.GiftPhotos = new GiftAttachmentClient().GetAttachmentById(connString, giftDetails.GiftID, "GiftPhotos");
            }

            Parallel.Invoke(
              () => giftDetails.M_OrganizationList = GetM_Organisation(connString, lang),
              () => giftDetails.M_LookupsList = GetM_Lookups(connString, lang));
            return giftDetails;
        }

        public List<GiftReportModel> GetGiftReport(string connString, GiftReportPostModel gift, string lang)
        {
            List<GiftReportModel> giftDetails = new List<GiftReportModel>();

            SqlParameter[] param = {
                   new SqlParameter("@P_ReceivedPurchasedBy", gift.RecievedPurchasedBy),
                   new SqlParameter("@P_UserID", gift.UserID),
                   new SqlParameter("@P_Language", lang),
                   new SqlParameter("@P_Status", gift.Status),
                   new SqlParameter("@P_GiftType", gift.GiftType),
                   new SqlParameter("@P_SmartSearch", gift.SmartSearch)
            };

            giftDetails = SqlHelper.ExecuteProcedureReturnData<List<GiftReportModel>>(connString, "Get_GiftReportList", r => r.TranslateAsGiftReportList(), param);

            return giftDetails;
        }

        internal GiftSaveResponseModel PatchGift(string connString, int id, JsonPatchDocument<GiftPutModel> value)
        {
            var result = GetPatchGiftByID(connString, id);

            value.ApplyTo(result);
            var res = PutGift(connString, result);
            return res;
        }
    }
}
