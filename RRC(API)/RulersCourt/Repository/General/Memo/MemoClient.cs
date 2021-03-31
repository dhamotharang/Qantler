using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.General.Memo;
using RulersCourt.Translators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository
{
    public class MemoClient
    {
        public MemoGetModel GetMemoByID(string connString, int memoID, int userID, string lang)
        {
            MemoGetModel memoDetails = new MemoGetModel();
            SqlParameter[] param = {
                new SqlParameter("@P_MemoID", memoID), new SqlParameter("@P_UserID", userID) };

            SqlParameter[] getDestinationUserparam = {
                new SqlParameter("@P_MemoID", memoID),
                new SqlParameter("@P_Language", lang) };

            SqlParameter[] getDestinationDepartmentparam = {
                    new SqlParameter("@P_MemoID", memoID) };

            if (memoID != 0)
            {
                memoDetails = SqlHelper.ExecuteProcedureReturnData(connString, "Get_MemoByID", r => r.TranslateAsMemoList(), param).FirstOrDefault();
                memoDetails.DestinationUsernameID = SqlHelper.ExecuteProcedureReturnData(connString, "Get_MemoDestinationUsers", r => r.TranslateAsDestinationUserList(), getDestinationUserparam);
                memoDetails.DestinationDepartmentID = SqlHelper.ExecuteProcedureReturnData(connString, "Get_MemoDestinationDepartment", r => r.TranslateAsDestinationDepartmentList(), getDestinationDepartmentparam);
                memoDetails.Attachments = new MemoAttachmentClient().GetAttachmentById(connString, memoDetails.MemoID, "Memo");
                memoDetails.HistoryLog = new MemoHistoryLogClient().MemoHistoryLogByMemoID(connString, memoID, lang);

                SqlParameter[] getKeywordparam = {
                    new SqlParameter("@P_MemoID", memoID),
                    new SqlParameter("@P_Type", 1),
                    new SqlParameter("@P_UserID", userID) };

                memoDetails.Keywords = SqlHelper.ExecuteProcedureReturnData(connString, "Get_MemoKeywords", r => r.TranslateAsKeywordsList(), getKeywordparam);

                SqlParameter[] getApproverparam = {
                    new SqlParameter("@P_ReferenceNumber", memoDetails.ReferenceNumber),
                    new SqlParameter("@P_UserID", userID) };

                memoDetails.CurrentApprover = SqlHelper.ExecuteProcedureReturnData(connString, "Get_MemoByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam);
                userID = memoDetails.CreatedBy.GetValueOrDefault();
            }

            Parallel.Invoke(
              () => memoDetails.M_OrganizationList = GetM_Organisation(connString, lang),
              () => memoDetails.M_LookupsList = GetM_Lookups(connString, lang),
              () => memoDetails.M_KeywordsList = GetM_Keyword(connString, memoID, userID),
              () => memoDetails.M_ApproverDepartmentList = new GetApproverConfiguration().GetM_ApproverDeparment(connString, lang));

            return memoDetails;
        }

        public MemoListModel GetMemo(string connString, int pageNumber, int pageSize, string type, string username, string status, string sourceOU, string stringDestinationOU, string privated, string priority, DateTime? dateFrom, DateTime? dateTo, string smartSearch, string lang)
        {
            EnumMemo e = (EnumMemo)Enum.ToObject(typeof(EnumMemo), Convert.ToInt32(type));

            MemoListModel list = new MemoListModel();

            SqlParameter[] param = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_Type", e),
                new SqlParameter("@P_Username", username),
                new SqlParameter("@P_Method", 0),
                new SqlParameter("@P_Status", status),
                new SqlParameter("@P_SourceOU", sourceOU),
                new SqlParameter("@P_DestinationOU", stringDestinationOU),
                new SqlParameter("@P_Private", privated),
                new SqlParameter("@P_Priority", priority),
                new SqlParameter("@P_DateFrom", dateFrom),
                new SqlParameter("@P_DateTo", dateTo),
                new SqlParameter("@P_SmartSearch", smartSearch),
                new SqlParameter("@P_Language", lang) };

            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<MemoGetModel>>(connString, "Get_MemoList", r => r.TranslateAsMemoList(), param);

            SqlParameter[] parama = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_Type", e),
                new SqlParameter("@P_Username", username),
                new SqlParameter("@P_Method", 1),
                new SqlParameter("@P_Status", status),
                new SqlParameter("@P_SourceOU", sourceOU),
                new SqlParameter("@P_DestinationOU", stringDestinationOU),
                new SqlParameter("@P_Private", privated),
                new SqlParameter("@P_Priority", priority),
                new SqlParameter("@P_DateFrom", dateFrom),
                new SqlParameter("@P_DateTo", dateTo),
                new SqlParameter("@P_SmartSearch", smartSearch),
                new SqlParameter("@P_Language", lang) };

            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_MemoList", parama);

            Parallel.Invoke(
              () => list.OrganizationList = GetM_OrganisationDashboard(connString, lang),
              () => list.LookupsList = GetM_Lookups(connString, lang),
              () => list.M_ApproverDepartmentList = new GetApproverConfiguration().GetM_ApproverDeparment(connString, lang));

            return list;
        }

        public MemoWorkflowModel PostMemo(string connString, MemoPostModel memo, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Title", memo.Title),
                new SqlParameter("@P_SourceOU", memo.SourceOU),
                new SqlParameter("@P_SourceName", memo.SourceName),
                new SqlParameter("@P_Details", memo.Details),
                new SqlParameter("@P_Private", memo.Private),
                new SqlParameter("@P_Priority", memo.Priority),
                new SqlParameter("@P_CreatedBy", memo.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", memo.CreatedDateTime),
                new SqlParameter("@P_Action", memo.Action),
                new SqlParameter("@P_Comment", memo.Comments),
                new SqlParameter("@P_ApproverDepartmentId", memo.ApproverDepartmentId) };

            var result = SqlHelper.ExecuteProcedureReturnData(connString, "Save_Memo", r => r.TranslateAsMemoSaveResponseList(), param);

            if (memo.DestinationUserId != null)
                new MemoDestinationClient().SaveUser(connString, memo.DestinationUserId, result.MemoId, lang);

            if (memo.DestinationDepartmentId != null)
                new MemoDestinationClient().SaveDepartment(connString, memo.DestinationDepartmentId, result.MemoId);

            if (memo.Keywords != null)
                new MemoKeywordClient().SaveUser(connString, memo.Keywords, result.MemoId, memo.CreatedBy);

            if (memo.Attachments != null)
                new MemoAttachmentClient().PostAttachments(connString, "Memo", memo.Attachments, result.MemoId);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "Memo"),
                new SqlParameter("@P_Action", memo.Action) };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            SqlParameter[] getApproverparam = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber) };

            result.ApproverID = memo.ApproverId;

            SqlParameter[] getDestinationUserparam = {
                    new SqlParameter("@P_MemoID", result.MemoId), new SqlParameter("@P_Language", lang) };

            result.DestinationUserID = SqlHelper.ExecuteProcedureReturnData(connString, "Get_MemoDestinationUsers", r => r.TranslateAsDestinationUserList(), getDestinationUserparam);

            SqlParameter[] getShareUserparam = {
                    new SqlParameter("@P_ServiceId", result.MemoId) };

            result.ShredUserID = SqlHelper.ExecuteProcedureReturnData(connString, "Get_ShareparticipationUsers", r => r.TranslateAsGetShareUserList(), getShareUserparam);

            result.Action = memo.Action;
            return result;
        }

        public MemoWorkflowModel PutMemo(string connString, MemoPutModel memo, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_MemoID", memo.MemoID),
                new SqlParameter("@P_Title", memo.Title),
                new SqlParameter("@P_SourceOU", memo.SourceOU),
                new SqlParameter("@P_SourceName", memo.SourceName),
                new SqlParameter("@P_Details", memo.Details),
                new SqlParameter("@P_Private", memo.Private),
                new SqlParameter("@P_Priority", memo.Priority),
                new SqlParameter("@P_UpdatedBy", memo.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", memo.UpdatedDateTime),
                new SqlParameter("@P_Action", memo.Action),
                new SqlParameter("@P_Comment", memo.Comments),
                new SqlParameter("@P_DeleteFlag", memo.DeleteFlag),
                new SqlParameter("@P_ApproverDepartmentId", memo.ApproverDepartmentId),
                new SqlParameter("@P_DestinationRedirected_EscalatedUserID", memo.Action == "Escalate" ? memo.ApproverId : memo.Action == "Redirect" ? memo.DestinatoionRedirectedBy : 0)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<MemoWorkflowModel>(connString, "Save_Memo", r => r.TranslateAsMemoSaveResponseList(), param);

            if (memo.DestinationUserId != null)
                new MemoDestinationClient().SaveUser(connString, memo.DestinationUserId, result.MemoId, lang);

            if (memo.DestinationDepartmentId != null)
                new MemoDestinationClient().SaveDepartment(connString, memo.DestinationDepartmentId, result.MemoId);

            if (memo.Keywords != null)
                new MemoKeywordClient().SaveUser(connString, memo.Keywords, result.MemoId, memo.UpdatedBy);

            if (memo.Attachments != null)
                new MemoAttachmentClient().PostAttachments(connString, "Memo", memo.Attachments, result.MemoId);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "Memo"),
                new SqlParameter("@P_Action", memo.Action),
                new SqlParameter("@P_ServiceID", memo.MemoID) };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            SqlParameter[] getApproverparam = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber) };

            SqlParameter[] getDestinationUserparam = {
                    new SqlParameter("@P_MemoID", result.MemoId), new SqlParameter("@P_Language", lang) };

            result.DestinationUserID = SqlHelper.ExecuteProcedureReturnData<List<MemoDestinationUsersGetModel>>(connString, "Get_MemoDestinationUsers", r => r.TranslateAsDestinationUserList(), getDestinationUserparam);

            SqlParameter[] getShareUserparam = {
                    new SqlParameter("@P_ServiceId", result.MemoId) };

            result.ShredUserID = SqlHelper.ExecuteProcedureReturnData<List<ShareparticipationUsersModel>>(connString, "Get_ShareparticipationUsers", r => r.TranslateAsGetShareUserList(), getShareUserparam);

            if (memo.Action == "Resubmit" || (memo.Action == "Submit") || (memo.Action == "Save"))
                result.ApproverID = memo.ApproverId;

            SqlParameter[] param1 = {
                new SqlParameter("@P_MemoID", memo.MemoID) };

            result.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<MemoGetModel>>(connString, "Get_MemoByID", r => r.TranslateAsMemoList(), param1).FirstOrDefault().Status);

            SqlParameter[] param2 = {
                new SqlParameter("@P_MemoID", memo.MemoID) };

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber) };

            result.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_MemoByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            result.Action = memo.Action;
            return result;
        }

        public string DeleteMemo(string connString, int id)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_MemoID", id)
             };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Delete_MemoByID", param);
        }

        public MemoPutModel GetPatchMemoByID(string connString, int memoID, string lang)
        {
            MemoPutModel memoDetails = new MemoPutModel();
            SqlParameter[] param = {
                new SqlParameter("@P_MemoID", memoID) };
            SqlParameter[] getDestinationUserparam = {
                    new SqlParameter("@P_MemoID", memoID), new SqlParameter("@P_Language", lang) };
            SqlParameter[] getDestinationDepartmentparam = {
                    new SqlParameter("@P_MemoID", memoID) };

            if (memoID != 0)
            {
                memoDetails = SqlHelper.ExecuteProcedureReturnData<List<MemoPutModel>>(connString, "Get_MemoByID", r => r.TranslateAsPutMemoList(), param).FirstOrDefault();
                memoDetails.DestinationUserId = SqlHelper.ExecuteProcedureReturnData<List<MemoDestinationUsersGetModel>>(connString, "Get_MemoDestinationUsers", r => r.TranslateAsDestinationUserList(), getDestinationUserparam);
                memoDetails.DestinationDepartmentId = SqlHelper.ExecuteProcedureReturnData<List<MemoDestinationDepartmentGetModel>>(connString, "Get_MemoDestinationDepartment", r => r.TranslateAsDestinationDepartmentList(), getDestinationDepartmentparam);
                memoDetails.Attachments = new MemoAttachmentClient().GetAttachmentById(connString, memoDetails.MemoID, "Memo");

                SqlParameter[] getKeywordparam = {
                    new SqlParameter("@P_MemoID", memoID),
                    new SqlParameter("@P_Type", 1) };

                memoDetails.Keywords = SqlHelper.ExecuteProcedureReturnData<List<MemoKeywordsModel>>(connString, "Get_MemoKeywords", r => r.TranslateAsKeywordsList(), getKeywordparam);
            }

            return memoDetails;
        }

        public string SaveMemoClone(string connString, int memoID, int userID, string lang)
        {
            SqlParameter[] cloneparam = {
            new SqlParameter("@P_UserID", userID),
            new SqlParameter("@P_MemoID", memoID),
            new SqlParameter("@P_Language", lang)
            };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_MemoClone", cloneparam);
        }

        public MemoPreviewModel GetMemoPreview(string connString, int memoID, int userID, string lang)
        {
            MemoPreviewModel memoDetails = new MemoPreviewModel();
            SqlParameter[] param = {
                new SqlParameter("@P_MemoID", memoID),
                new SqlParameter("@P_Language", lang)
            };

            SqlParameter[] getDestinationUserparam = {
                new SqlParameter("@P_MemoID", memoID),
                new SqlParameter("@P_Language", lang)
            };

            SqlParameter getDestinationDepartmentparam = new SqlParameter("@P_MemoID", memoID);

            if (memoID != 0)
            {
                memoDetails = SqlHelper.ExecuteProcedureReturnData<List<MemoPreviewModel>>(connString, "Get_MemoPreview", r => r.TranslateAsMemoPreviewList(), param).FirstOrDefault();

                memoDetails.DestinationUsernameID = SqlHelper.ExecuteProcedureReturnData<List<MemoDestinationUsersGetModel>>(connString, "Get_MemoDestinationUsers", r => r.TranslateAsDestinationUserList(), getDestinationUserparam);

                memoDetails.DestinationDepartmentID = SqlHelper.ExecuteProcedureReturnData<List<MemoDestinationDepartmentGetModel>>(connString, "Get_MemoDestinationDepartment", r => r.TranslateAsDestinationDepartmentList(), getDestinationDepartmentparam);

                memoDetails.Attachments = new MemoAttachmentClient().GetAttachmentById(connString, memoDetails.MemoID, "Memo");

                memoDetails.HistoryLog = new MemoHistoryLogClient().MemoHistoryLogByMemoID(connString, memoID, lang);

                SqlParameter[] getKeywordparam = {
                    new SqlParameter("@P_MemoID", memoID),
                    new SqlParameter("@P_Type", 1),
                    new SqlParameter("@P_UserID", userID) };

                memoDetails.Keywords = SqlHelper.ExecuteProcedureReturnData<List<MemoKeywordsModel>>(connString, "Get_MemoKeywords", r => r.TranslateAsKeywordsList(), getKeywordparam);

                SqlParameter[] getApproverparam = {
                    new SqlParameter("@P_ReferenceNumber", memoDetails.ReferenceNumber),
                    new SqlParameter("@P_UserID", userID) };

                memoDetails.CurrentApprover = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_MemoByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam);

                userID = memoDetails.CreatedBy.GetValueOrDefault();
            }

            Parallel.Invoke(
              () => memoDetails.M_OrganizationList = GetM_Organisation(connString, lang),
              () => memoDetails.M_LookupsList = GetM_Lookups(connString, lang),
              () => memoDetails.M_KeywordsList = GetM_Keyword(connString, memoID, userID));

            return memoDetails;
        }

        public void Postkeyword(string connString, List<MemoKeywordsModel> memo, int memoID, int? userID)
        {
            new MemoKeywordClient().SaveUser(connString, memo, memoID, userID);
        }

        internal MemoWorkflowModel PatchMemo(string connString, int id, JsonPatchDocument<MemoPutModel> value, string lang)
        {
            var result = GetPatchMemoByID(connString, id, lang);

            value.ApplyTo(result);
            var res = PutMemo(connString, result, lang);
            if (result.Action == "Escalate" || result.Action == "Redirect")
            {
                res.ApproverID = result.ApproverId;
            }

            if (result.Action == "Redirect")
            {
                res.DestinationRedirectedBy = result.DestinatoionRedirectedBy;
            }

            SqlParameter[] param1 = {
                new SqlParameter("@P_MemoID", result.MemoID) };

            res.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<MemoGetModel>>(connString, "Get_MemoByID", r => r.TranslateAsMemoList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", res.ReferenceNumber) };

            res.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_MemoByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            return res;
        }

        private List<OrganizationModel> GetM_Organisation(string connString, string lang)
        {
            SqlParameter[] orgParam = {
                new SqlParameter("@P_Language", lang) };
            var e = SqlHelper.ExecuteProcedureReturnData(connString, "Get_Organization", r => r.TranslateAsOrganizationList(), orgParam);
            return e;
        }

        private List<OrganizationModel> GetM_OrganisationDashboard(string connString, string lang)
        {
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
            SqlParameter[] orgParam = {
                new SqlParameter("@P_Language", lang) };
            List<OrganizationModel> e = SqlHelper.ExecuteProcedureReturnData(connString, "Get_Organization", r => r.TranslateAsOrganizationList(), orgParam);
            foreach (OrganizationModel m in e)
            {
                res.Add(m);
            }

            return res;
        }

        private List<M_LookupsModel> GetM_Lookups(string connString, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Type", "Memo"), new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        private List<MemoKeywordsModel> GetM_Keyword(string connString, int memoID, int userID)
        {
            SqlParameter[] getKeywordparama = {
                    new SqlParameter("@P_MemoID", memoID),
                    new SqlParameter("@P_Type", 2),
                    new SqlParameter("@P_UserID", userID) };

            return SqlHelper.ExecuteProcedureReturnData<List<MemoKeywordsModel>>(connString, "Get_MemoKeywords", r => r.TranslateAsKeywordsList(), getKeywordparama);
        }
    }
}
