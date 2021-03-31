using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.General.Letter;
using RulersCourt.Models.General.Letter.LetterOutbound;
using RulersCourt.Models.Letter;
using RulersCourt.Repository.Letter;
using RulersCourt.Translators;
using RulersCourt.Translators.General.Letter;
using RulersCourt.Translators.Letter;
using RulersCourt.Translators.Letter.LetterInbound;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository
{
    public class LetterOutboundClient
    {
        public LetterOutboundGetModel GetLetterByID(string connString, int letterID, int userID, string lang)
        {
            LetterOutboundGetModel letterDetails = new LetterOutboundGetModel();
            SqlParameter[] param = {
                new SqlParameter("@P_LetterID", letterID),
                new SqlParameter("@P_UserID", userID) };
            SqlParameter[] destinationUserparam = {
                    new SqlParameter("@P_LetterID", letterID) };
            SqlParameter[] destinationDepartmentparam = {
                    new SqlParameter("@P_LetterID", letterID) };
            SqlParameter[] getRelatedOutgoingparam = {
                    new SqlParameter("@P_LetterID", letterID),
                    new SqlParameter("@P_LetterType", 0) };
            SqlParameter[] getRelatedIncomingparam = {
                    new SqlParameter("@P_LetterID", letterID),
                    new SqlParameter("@P_LetterType", 1) };

            if (letterID != 0)
            {
                letterDetails = SqlHelper.ExecuteProcedureReturnData<List<LetterOutboundGetModel>>(connString, "Get_LetterOutboundByID", r => r.TranslateAsLetterList(), param).FirstOrDefault();

                letterDetails.DestinationEntity = SqlHelper.ExecuteProcedureReturnData<List<LetterOutboundDestinationDepartmentModel>>(connString, "Get_LetterOutboundDestinationEntity", r => r.TranslateAsLetterDestinationDepartmentList(), destinationDepartmentparam);

                letterDetails.RelatedOutgoingLetters = SqlHelper.ExecuteProcedureReturnData<List<LetterOutboundRelatedOutgoingModel>>(connString, "Get_LetterOutboundRelatedOutgoingReferenceNo", r => r.TranslateAsLetterOutgoingRefNoList(), getRelatedOutgoingparam);

                letterDetails.RelatedIncomingLetters = SqlHelper.ExecuteProcedureReturnData<List<LetterOutboundRelatedOutgoingModel>>(connString, "Get_LetterOutboundRelatedOutgoingReferenceNo", r => r.TranslateAsLetterOutgoingRefNoList(), getRelatedIncomingparam);

                letterDetails.Attachments = new LetterAttachmentClient().GetAttachmentById(connString, letterDetails.LetterID, "OutboundLetter");

                letterDetails.HistoryLog = new LetterOutboundHistoryLogClient().LetterHistoryLogByLetterID(connString, letterID, lang);

                SqlParameter[] getKeywordparam = {
                    new SqlParameter("@P_LetterID", letterID),
                    new SqlParameter("@P_Type", 1),
                    new SqlParameter("@P_UserID", userID) };

                letterDetails.Keywords = SqlHelper.ExecuteProcedureReturnData<List<LetterOutboundKeywordsModel>>(connString, "Get_LetterOutboundKeywords", r => r.TranslateAsLetterKeywordsList(), getKeywordparam);

                SqlParameter[] getApproverparam = {
                    new SqlParameter("@P_ReferenceNumber", letterDetails.LetterReferenceNumber),
                    new SqlParameter("@P_UserID", userID)
                };

                letterDetails.CurrentApprover = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_LetterOutboundByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam);

                userID = letterDetails.CreatedBy.GetValueOrDefault();
            }

            Parallel.Invoke(
              () => letterDetails.L_OrganizationList = GetL_Organisation(connString, lang),
              () => letterDetails.M_LookupsList = GetM_Lookups(connString, lang),
              () => letterDetails.L_KeywordsList = GetL_Keyword(connString, letterID, userID),
              () => letterDetails.OrganisationEntity = new LetterOutboundOrganizationEntityClient().GetLetterOrgEntity(connString, userID),
              () => letterDetails.M_ApproverDepartmentList = new GetApproverConfiguration().GetM_ApproverDeparment(connString, lang));
            return letterDetails;
        }

        public List<OrganizationModel> GetL_Organisation(string connString, string lang)
        {
            SqlParameter[] param = {
                 new SqlParameter("@P_Language", lang) };

            var e = SqlHelper.ExecuteProcedureReturnData<List<OrganizationModel>>(connString, "Get_Organization", r => r.TranslateAsOrganizationList(), param);
            return e;
        }

        public List<M_LookupsModel> GetM_Lookups(string connString, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Type", "OutboundLetter"),
                new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsL_LookupsList(), param);
        }

        public List<LetterOutboundKeywordsModel> GetL_Keyword(string connString, int letterID, int userID)
        {
            SqlParameter[] getKeywordparama = {
                    new SqlParameter("@P_LetterID", letterID),
                    new SqlParameter("@P_Type", 2),
                    new SqlParameter("@P_UserID", userID) };
            return SqlHelper.ExecuteProcedureReturnData<List<LetterOutboundKeywordsModel>>(connString, "Get_LetterOutboundKeywords", r => r.TranslateAsLetterKeywordsList(), getKeywordparama);
        }

        public List<LetterUserNameAndCreatorModel> GetUserName(string connString, int userID, string lang)
        {
            SqlParameter[] getKeywordparama = {
                    new SqlParameter("@P_UserId", userID),
                    new SqlParameter("@P_Type", 1),
                    new SqlParameter("@P_Language", lang),
            };
            return SqlHelper.ExecuteProcedureReturnData<List<LetterUserNameAndCreatorModel>>(connString, "Get_LetterCreatorAndUserName", r => r.TranslateAsLetterGetUserList(), getKeywordparama);
        }

        public List<LetterUserNameAndCreatorModel> GetCreator(string connString, int userID, string lang)
        {
            SqlParameter[] getKeywordparama = {
                    new SqlParameter("@P_UserId", userID),
                    new SqlParameter("@P_Type", 2),
                    new SqlParameter("@P_Language", lang),
            };
            return SqlHelper.ExecuteProcedureReturnData<List<LetterUserNameAndCreatorModel>>(connString, "Get_LetterCreatorAndUserName", r => r.TranslateAsLetterGetUserList(), getKeywordparama);
        }

        public LetterOutboundListModel GetLetter(string connString, int pageNumber, int pageSize, string type, string userID, string status, string sourceOU, string stringDestinationOU, string stringUserName, string priority, string senderName, DateTime? dateFrom, DateTime? dateTo, string smartSearch, string lang)
        {
            EnumLetter e = (EnumLetter)Enum.ToObject(typeof(EnumLetter), Convert.ToInt32(type));
            LetterOutboundListModel list = new LetterOutboundListModel();
            SqlParameter[] param = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_Type", e),
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Method", 0),
                new SqlParameter("@P_Status", status),
                new SqlParameter("@P_SourceOU", sourceOU),
                new SqlParameter("@P_DestinationOU", stringDestinationOU),
                new SqlParameter("@P_SenderName", senderName),
                new SqlParameter("@P_Username", stringUserName),
                new SqlParameter("@P_Priority", priority),
                new SqlParameter("@P_DateFrom", dateFrom),
                new SqlParameter("@P_DateTo", dateTo),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_SmartSearch", smartSearch) };
            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<LetterOutboundDashboardListModel>>(connString, "Get_LetterList", r => r.TranslateAsLetterDashboardList(), param);
            SqlParameter[] parama = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_Type", e),
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Method", 1),
                new SqlParameter("@P_Status", status),
                new SqlParameter("@P_SourceOU", sourceOU),
                new SqlParameter("@P_DestinationOU", stringDestinationOU),
                new SqlParameter("@P_SenderName", senderName),
                new SqlParameter("@P_Username", stringUserName),
                new SqlParameter("@P_Priority", priority),
                new SqlParameter("@P_DateFrom", dateFrom),
                new SqlParameter("@P_DateTo", dateTo),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_SmartSearch", smartSearch) };
            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_LetterList", parama);
            Parallel.Invoke(
                () => list.OrganizationList = GetL_Organisation(connString, lang),
                () => list.LookupsList = GetM_Lookups(connString, lang),
                () => list.UserName = GetUserName(connString, int.Parse(userID), lang),
                () => list.Creator = GetCreator(connString, int.Parse(userID), lang),
                () => list.M_ApproverDepartmentList = new GetApproverConfiguration().GetM_ApproverDeparment(connString, lang));
            return list;
        }

        public LetterOutboundWorkflowModel PostLetter(string connString, LetterOutboundPostModel letter, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Title", letter.Title),
                new SqlParameter("@P_SourceOU", letter.SourceOU),
                new SqlParameter("@P_SourceName", letter.SourceName),
                new SqlParameter("@P_LetterDetails", letter.LetterDetails),
                new SqlParameter("@P_DocumentClassification", letter.DocumentClassification),
                new SqlParameter("@P_RelatedToIncomingLetter", letter.RelatedToIncomingLetter),
                new SqlParameter("@P_Priority", letter.Priority),
                new SqlParameter("@P_NeedReply", letter.NeedReply),
                new SqlParameter("@P_LetterType", letter.LetterType),
                new SqlParameter("@P_ApproverDepartmentId", letter.ApproverDepartmentId),
                new SqlParameter("@P_Action", letter.Action),
                new SqlParameter("@P_Comment", letter.Comments),
                new SqlParameter("@P_CreatedBy", letter.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", letter.CreatedDateTime)
           };
            var result = SqlHelper.ExecuteProcedureReturnData<LetterOutboundWorkflowModel>(connString, "Save_LetterOutbound", r => r.TranslateAsLetterSaveResponseList(), param);

            if (letter.DestinationEntity != null)
            {
                new LetterOutboundDestinationClient().SaveDepartment(connString, letter.DestinationEntity, result.LetterID);
            }

            if (letter.Keywords != null)
            {
                new LetterOutboundKeywordClient().SaveUser(connString, letter.Keywords, result.LetterID, letter.CreatedBy);
            }

            if (letter.RelatedOutgoingLetters != null)
            {
                new LetterOutboundRelatedOutgoingClient().Save(connString, letter.RelatedOutgoingLetters, result.LetterID, 0);
            }

            if (letter.RelatedIncomingLetters != null)
            {
                new LetterOutboundRelatedOutgoingClient().Save(connString, letter.RelatedIncomingLetters, result.LetterID, 1);
            }

            if (letter.Attachments != null)
            {
                new LetterAttachmentClient().OutboundLetterPostAttachments(connString, "OutboundLetter", letter.Attachments, result.LetterID);
            }

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "OutboundLetter"),
                new SqlParameter("@P_Action", letter.Action) };
            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.ApproverID = letter.ApproverId;

            SqlParameter[] getDestinationUserparam = {
                    new SqlParameter("@P_LetterID", result.LetterID),
                    new SqlParameter("@P_Language", lang),
            };
            result.DestinationUserID = SqlHelper.ExecuteProcedureReturnData<List<LetterOutboundDestinationUsersModel>>(connString, "Get_LetterOutboundDestinationUsers", r => r.TranslateAsLetterDestinationUserList(), getDestinationUserparam);

            result.Action = letter.Action;

            return result;
        }

        public LetterOutboundWorkflowModel PutLetter(string connString, LetterOutboundPutModel letter, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_LetterID", letter.LetterID),
                new SqlParameter("@P_Title", letter.Title),
                new SqlParameter("@P_SourceOU", letter.SourceOU),
                new SqlParameter("@P_SourceName", letter.SourceName),
                new SqlParameter("@P_RelatedToIncomingLetter", letter.RelatedToIncomingLetter),
                new SqlParameter("@P_LetterDetails", letter.LetterDetails),
                new SqlParameter("@P_DocumentClassification", letter.DocumentClassification),
                new SqlParameter("@P_Priority", letter.Priority),
                new SqlParameter("@P_NeedReply", letter.NeedReply),
                new SqlParameter("@P_LetterType", letter.LetterType),
                new SqlParameter("@P_ApproverDepartmentId", letter.ApproverDepartmentId),
                new SqlParameter("@P_Action", letter.Action),
                new SqlParameter("@P_Comment", letter.Comments),
                new SqlParameter("@P_DeleteFlag", letter.DeleteFlag),
                new SqlParameter("@P_UpdatedBy", letter.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", letter.UpdatedDateTime),
                new SqlParameter("@P_DestinationRedirected_EscalatedUserID", letter.Action == "Escalate" ? letter.ApproverId : letter.Action == "Redirect" ? letter.DestinatoionRedirectedBy : 0)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<LetterOutboundWorkflowModel>(connString, "Save_LetterOutbound", r => r.TranslateAsLetterSaveResponseList(), param);

            if (letter.DestinationEntity != null)
            {
                new LetterOutboundDestinationClient().SaveDepartment(connString, letter.DestinationEntity, result.LetterID);
            }

            if (letter.Keywords != null)
            {
                new LetterOutboundKeywordClient().SaveUser(connString, letter.Keywords, result.LetterID, letter.UpdatedBy);
            }

            if (letter.RelatedOutgoingLetters != null)
            {
                new LetterOutboundRelatedOutgoingClient().Save(connString, letter.RelatedOutgoingLetters, result.LetterID, 0);
            }

            if (letter.RelatedIncomingLetters != null)
            {
                new LetterOutboundRelatedOutgoingClient().Save(connString, letter.RelatedIncomingLetters, result.LetterID, 1);
            }

            if (letter.Attachments != null)
            {
                new LetterAttachmentClient().OutboundLetterPostAttachments(connString, "OutboundLetter", letter.Attachments, result.LetterID);
            }

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "OutboundLetter"),
                new SqlParameter("@P_Action", letter.Action),
                new SqlParameter("@P_ServiceID", letter.LetterID) };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.ApproverID = letter.ApproverId;

            SqlParameter[] getDestinationUserparam = {
                    new SqlParameter("@P_LetterID", result.LetterID),
                    new SqlParameter("@P_Language", lang),
            };

            result.DestinationUserID = SqlHelper.ExecuteProcedureReturnData<List<LetterOutboundDestinationUsersModel>>(connString, "Get_LetterOutboundDestinationUsers", r => r.TranslateAsLetterDestinationUserList(), getDestinationUserparam);
            result.Action = letter.Action;

            if (result.Action == "Resubmit" || (result.Action == "Submit") || result.Action == "Save")
                result.ApproverID = letter.ApproverId;

            SqlParameter[] param1 = {
                new SqlParameter("@P_LetterID", letter.LetterID) };

            result.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<LetterOutboundGetModel>>(connString, "Get_LetterOutboundByID", r => r.TranslateAsLetterList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber) };

            result.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_LetterOutboundByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            return result;
        }

        public string DeleteLetter(string connString, int id)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_LetterID", id)
             };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Delete_LetterOutboundByID", param);
        }

        public LetterOutboundPutModel GetPatchLetterByID(string connString, int letterID)
        {
            LetterOutboundPutModel letterDetails = new LetterOutboundPutModel();

            SqlParameter[] param = {
                new SqlParameter("@P_LetterID", letterID),
                new SqlParameter("@P_UserID", 0) };

            SqlParameter[] getDestinationUserparam = {
                    new SqlParameter("@P_LetterID", letterID) };

            SqlParameter[] getDestinationDepartmentparam = {
                    new SqlParameter("@P_LetterID", letterID) };

            if (letterID != 0)
            {
                letterDetails = SqlHelper.ExecuteProcedureReturnData<List<LetterOutboundPutModel>>(connString, "Get_LetterOutboundByID", r => r.TranslateAsPutLetterList(), param).FirstOrDefault();

                letterDetails.DestinationEntity = SqlHelper.ExecuteProcedureReturnData<List<LetterOutboundDestinationDepartmentModel>>(connString, "Get_LetterOutboundDestinationEntity", r => r.TranslateAsLetterDestinationDepartmentList(), getDestinationDepartmentparam);

                letterDetails.Attachments = new LetterAttachmentClient().GetAttachmentById(connString, letterDetails.LetterID, "OutboundLetter");

                SqlParameter[] getKeywordparam = {
                    new SqlParameter("@P_LetterID", letterID),
                    new SqlParameter("@P_Type", 1) };

                letterDetails.Keywords = SqlHelper.ExecuteProcedureReturnData<List<LetterOutboundKeywordsModel>>(connString, "Get_LetterOutboundKeywords", r => r.TranslateAsLetterKeywordsList(), getKeywordparam);
            }

            return letterDetails;
        }

        public LetterOutboundWorkflowModel GetLetterBulkWorkflowByID(string connString, int letterID, string actionBy, DateTime actionDateTime, string lang)
        {
            LetterOutboundWorkflowModel letterDetails = new LetterOutboundWorkflowModel();

            SqlParameter[] param = {
                new SqlParameter("@P_LetterID", letterID),
                new SqlParameter("@P_UserID", 0) };

            SqlParameter[] destinationUserparam = {
                    new SqlParameter("@P_LetterID", letterID),
                    new SqlParameter("@P_Language", lang),
            };

            SqlParameter[] destinationDepartmentparam = {
                    new SqlParameter("@P_LetterID", letterID) };

            if (letterID != 0)
            {
                letterDetails = SqlHelper.ExecuteProcedureReturnData<List<LetterOutboundWorkflowModel>>(connString, "Get_LetterOutboundByID", r => r.TranslateAsLetterBulkWorkflowList(), param).FirstOrDefault();

                letterDetails.DestinationUserID = SqlHelper.ExecuteProcedureReturnData<List<LetterOutboundDestinationUsersModel>>(connString, "Get_LetterOutboundDestinationUsers", r => r.TranslateAsLetterDestinationUserList(), destinationUserparam);

                SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "OutboundLetter"),
                new SqlParameter("@P_Action", "Approve") };

                letterDetails.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

                SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", letterDetails.ReferenceNumber) };

                letterDetails.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_LetterOutboundByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

                var result = GetPatchLetterByID(connString, letterID);
                result.Action = "Approve";
                result.UpdatedBy = Convert.ToInt32(actionBy);
                result.UpdatedDateTime = DateTime.UtcNow;
                result.ApproverId = Convert.ToInt32(actionBy);
                result.Comments = string.Empty;
                var res = PutLetter(connString, result, lang);
            }

            return letterDetails;
        }

        public List<GovernmentEntityModel> GetEntity(string connString, int entity, string entityname, int entityID)
        {
            SqlParameter[] relatedEntityParam = {
                new SqlParameter("@P_Entity", entity), new SqlParameter("@P_EntityName", entityname), new SqlParameter("@P_EntityID", entityID) };
            return SqlHelper.ExecuteProcedureReturnData<List<GovernmentEntityModel>>(connString, "Get_LetterEntityList", r => r.TranslateAsEntity(), relatedEntityParam);
        }

        public List<LetterRelatedOutgoingLetterModel> GetRelatedLetter(string connString, int userID, int type)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Type", type)
             };
            return SqlHelper.ExecuteProcedureReturnData(connString, "Get_LetterOutboundLinkToLetter", r => r.TranslateAsRelatedLetterList(), param);
        }

        public List<LetterReportModel> GetReporExporttList(string connString, LetterReportRequestModel report)
        {
            List<LetterReportModel> list = new List<LetterReportModel>();
            SqlParameter[] param = {
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_UserName", report.UserName),
            new SqlParameter("@P_Status", report.Status),
            new SqlParameter("@P_SourceOU", report.SourceOU),
            new SqlParameter("@P_DestinationOU", report.Destination),
            new SqlParameter("@P_DateFrom", report.DateRangeForm),
            new SqlParameter("@P_DateTo", report.DateRangeTo),
            new SqlParameter("@P_Priority", report.Priority),
            new SqlParameter("@P_SmartSearch", report.SmartSearch)
            };
            list = SqlHelper.ExecuteProcedureReturnData<List<LetterReportModel>>(connString, "Get_LetterReportList", r => r.TranslateAsLetterReportList(), param);
            return list;
        }

        public string SaveOutboundClone(string connString, int letterID, int userID, string lang)
        {
            SqlParameter[] cloneparam = {
            new SqlParameter("@P_UserID", userID),
            new SqlParameter("@P_LetterID", letterID),
            new SqlParameter("@P_Lang", lang)
            };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_LetterOutboundClone", cloneparam);
        }

        public LetterOutboundPreviewModel GetLetterPreview(string connString, int letterID, int userID, string lang)
        {
            LetterOutboundPreviewModel letterDetails = new LetterOutboundPreviewModel();
            SqlParameter[] param = {
                new SqlParameter("@P_LetterID", letterID), new SqlParameter("@P_UserID", userID),new SqlParameter("@P_Language", lang) };
            SqlParameter[] destinationUserparam = {
                    new SqlParameter("@P_LetterID", letterID) };
            SqlParameter[] destinationDepartmentparam = {
                    new SqlParameter("@P_LetterID", letterID) };
            SqlParameter[] getRelatedOutgoingparam = {
                    new SqlParameter("@P_LetterID", letterID) };

            if (letterID != 0)
            {
                letterDetails = SqlHelper.ExecuteProcedureReturnData<List<LetterOutboundPreviewModel>>(connString, "Get_LetterOutboundPreview", r => r.TranslateAsLetterPreviewList(), param).FirstOrDefault();

                letterDetails.DestinationEntity = SqlHelper.ExecuteProcedureReturnData<List<LetterOutboundDestinationDepartmentModel>>(connString, "Get_LetterOutboundDestinationEntity", r => r.TranslateAsLetterDestinationDepartmentList(), destinationDepartmentparam);
            }

            return letterDetails;
        }

        internal LetterOutboundWorkflowModel PatchLetter(string connString, int id, JsonPatchDocument<LetterOutboundPutModel> value, string lang)
        {
            var result = GetPatchLetterByID(connString, id);

            value.ApplyTo(result);
            var res = PutLetter(connString, result, lang);
            if (result.Action == "Escalate" || result.Action == "Redirect")
            {
                res.ApproverID = result.ApproverId;
            }
            else
            {
                SqlParameter[] getApproverparam = {
                        new SqlParameter("@P_ReferenceNumber", result.LetterReferenceNumber) };

                res.ApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_LetterOutboundByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam).FirstOrDefault().ApproverId;
            }

            if (result.Action == "Redirect")
            {
                res.DestinationRedirectedBy = result.DestinatoionRedirectedBy;
            }

            res.Action = result.Action;

            SqlParameter[] param1 = {
                new SqlParameter("@P_LetterID", result.LetterID) };

            res.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<LetterOutboundGetModel>>(connString, "Get_LetterOutboundByID", r => r.TranslateAsLetterList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", res.ReferenceNumber) };

            res.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_LetterOutboundByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            SqlParameter[] param = { new SqlParameter("@P_Department", 14), new SqlParameter("@P_Type", 2), new SqlParameter("@P_Language", lang) };
            res.DestinationDepartmentUserID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_User", r => r.TranslateAsUserList(), param);

            return res;
        }
    }
}