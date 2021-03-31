using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.General.Letter;
using RulersCourt.Models.General.Letter.LetterInbound;
using RulersCourt.Models.Letter;
using RulersCourt.Models.Letter.LetterInbound;
using RulersCourt.Translators;
using RulersCourt.Translators.General.Letter;
using RulersCourt.Translators.Letter;
using RulersCourt.Translators.Letter.LetterInbound;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Letter.LetterInbound
{
    public class LetterInboundClient
    {
        public LetterInboundGetModel GetLetterByID(string connString, int letterID, int userID, string lang)
        {
            LetterInboundGetModel letterDetails = new LetterInboundGetModel();
            SqlParameter[] param = {
                new SqlParameter("@P_LetterID", letterID),
                new SqlParameter("@P_UserID", userID) };
            SqlParameter[] destinationUserparam = {
                    new SqlParameter("@P_LetterID", letterID),
                    new SqlParameter("@P_Language", lang),
            };
            SqlParameter[] destinationDepartmentparam = {
                    new SqlParameter("@P_LetterID", letterID) };
            SqlParameter[] getRelatedOutgoingparam = {
                    new SqlParameter("@P_LetterID", letterID),
                    new SqlParameter("@P_LetterType", 0)
            };

            SqlParameter[] getRelatedImcomingparam = {
                    new SqlParameter("@P_LetterID", letterID),
                    new SqlParameter("@P_LetterType", 1)
            };

            if (letterID != 0)
            {
                letterDetails = SqlHelper.ExecuteProcedureReturnData<List<LetterInboundGetModel>>(connString, "Get_LetterInboundByID", r => r.TranslateAsLetterInboundList(), param).FirstOrDefault();

                letterDetails.DestinationUserId = SqlHelper.ExecuteProcedureReturnData<List<LetterInboundDestinationUsersModel>>(connString, "Get_LetterInboundDestinationUsers", r => r.TranslateAsLetterInboundDestinationUserList(), destinationUserparam);

                letterDetails.DestinationDepartmentId = SqlHelper.ExecuteProcedureReturnData<List<LetterInboundDestinationDepartmentModel>>(connString, "Get_LetterInboundDestinationDepartment", r => r.TranslateAsLetterInboundDestinationDepartmentList(), destinationDepartmentparam);
                letterDetails.RelatedOutgoingLetters = SqlHelper.ExecuteProcedureReturnData<List<LetterInboundRelatedOutgoingModel>>(connString, "Get_LetterInboundRelatedOutgoingReferenceNo", r => r.TranslateAsLetterInboundRefNoList(), getRelatedOutgoingparam);
                letterDetails.RelatedIncomingLetters = SqlHelper.ExecuteProcedureReturnData<List<LetterInboundRelatedOutgoingModel>>(connString, "Get_LetterInboundRelatedOutgoingReferenceNo", r => r.TranslateAsLetterInboundRefNoList(), getRelatedImcomingparam);

                letterDetails.Attachments = new LetterAttachmentClient().GetAttachmentById(connString, letterDetails.LetterID, "InboundLetter");

                letterDetails.HistoryLog = new LetterInboundHistoryLogClient().LetterInboundHistoryLogByLetterID(connString, letterID, lang);

                SqlParameter[] getKeywordparam = {
                    new SqlParameter("@P_LetterID", letterID),
                    new SqlParameter("@P_Type", 1),
                    new SqlParameter("@P_UserID", userID) };

                letterDetails.Keywords = SqlHelper.ExecuteProcedureReturnData<List<LetterInboundKeywordsModel>>(connString, "Get_LetterInboundKeywords", r => r.TranslateAsLetterInboundKeywordsList(), getKeywordparam);

                SqlParameter[] getApproverparam = {
                    new SqlParameter("@P_ReferenceNumber", letterDetails.LetterReferenceNumber),
                    new SqlParameter("@P_UserID", userID)
                };

                letterDetails.CurrentApprover = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_LetterInboundByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam);

                userID = letterDetails.CreatedBy.GetValueOrDefault();
            }

            Parallel.Invoke(
              () => letterDetails.L_OrganizationList = GetL_Organisation(connString, lang),
              () => letterDetails.M_LookupsList = GetM_Lookups(connString, lang),
              () => letterDetails.L_KeywordsList = GetL_Keyword(connString, letterID, userID),
              () => letterDetails.OrganisationEntity = new LetterInboundOrganizationEntityClient().GetLetterInboundOrgEntity(connString, userID),
              () => letterDetails.M_ApproverDepartmentList = new GetApproverConfiguration().GetM_ApproverDeparment(connString, lang));
            return letterDetails;
        }

        public List<OrganizationModel> GetL_Organisation(string connString, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Language", lang)
            };
            var e = SqlHelper.ExecuteProcedureReturnData<List<OrganizationModel>>(connString, "Get_Organization", r => r.TranslateAsOrganizationList(), param);
            return e;
        }

        public List<OrganizationModel> GetM_OrganisationDashboard(string connString, string lang)
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
            List<OrganizationModel> e = SqlHelper.ExecuteProcedureReturnData<List<OrganizationModel>>(connString, "Get_Organization", r => r.TranslateAsOrganizationList(), orgParam);
            foreach (OrganizationModel m in e)
            {
                res.Add(m);
            }

            return res;
        }

        public List<M_LookupsModel> GetM_Lookups(string connString, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Type", "InboundLetter"),
                new SqlParameter("@P_Language", lang)
            };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsL_LookupsList(), param);
        }

        public List<LetterInboundKeywordsModel> GetL_Keyword(string connString, int letterID, int userID)
        {
            SqlParameter[] getKeywordparama = {
                    new SqlParameter("@P_LetterID", letterID),
                    new SqlParameter("@P_Type", 2),
                    new SqlParameter("@P_UserID", userID) };
            return SqlHelper.ExecuteProcedureReturnData<List<LetterInboundKeywordsModel>>(connString, "Get_LetterInboundKeywords", r => r.TranslateAsLetterInboundKeywordsList(), getKeywordparama);
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

        public LetterInboundListModel GetInboundLetter(string connString, int pageNumber, int pageSize, string type, string userID, string status, string sourceOU, string stringDestinationOU, string stringUserName, string priority, DateTime? dateFrom, DateTime? dateTo, string smartSearch, string lang)
        {
            EnumLetter e = (EnumLetter)Enum.ToObject(typeof(EnumLetter), Convert.ToInt32(type));
            LetterInboundListModel list = new LetterInboundListModel();
            SqlParameter[] param = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_Type", e),
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Username", stringUserName),
                new SqlParameter("@P_Method", 0),
                new SqlParameter("@P_Status", status),
                new SqlParameter("@P_SourceOU", sourceOU),
                new SqlParameter("@P_DestinationOU", stringDestinationOU),
                new SqlParameter("@P_Priority", priority),
                new SqlParameter("@P_DateFrom", dateFrom),
                new SqlParameter("@P_DateTo", dateTo),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_SmartSearch", smartSearch) };
            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<LetterInboundDashboardListModel>>(connString, "Get_LetterList", r => r.TranslateAsLetterInboundDashboardList(), param);
            SqlParameter[] parama = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_Type", e),
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Username", stringUserName),
                new SqlParameter("@P_Method", 1),
                new SqlParameter("@P_Status", status),
                new SqlParameter("@P_SourceOU", sourceOU),
                new SqlParameter("@P_DestinationOU", stringDestinationOU),
                new SqlParameter("@P_Priority", priority),
                new SqlParameter("@P_DateFrom", dateFrom),
                new SqlParameter("@P_DateTo", dateTo),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_SmartSearch", smartSearch) };
            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_LetterList", parama);
            Parallel.Invoke(
                () => list.OrganizationList = GetM_OrganisationDashboard(connString, lang),
                () => list.LookupsList = GetM_Lookups(connString, lang),
                () => list.UserName = GetUserName(connString, int.Parse(userID), lang),
                () => list.Creator = GetCreator(connString, int.Parse(userID), lang),
                () => list.M_ApproverDepartmentList = new GetApproverConfiguration().GetM_ApproverDeparment(connString, lang));
            return list;
        }

        public LetterInboundWorkflowModel PostLetter(string connString, LetterInboundPostModel letter, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Title", letter.Title),
                new SqlParameter("@P_SourceOU", letter.SourceOU),
                new SqlParameter("@P_SourceName", letter.SourceName),
                new SqlParameter("@P_ReceivingDate", letter.ReceivingDate),
                new SqlParameter("@P_ReceivedFromGovernmentEntity", letter.ReceivedFromGovernmentEntity),
                new SqlParameter("@P_ReceivedFromName", letter.ReceivedFromName),
                new SqlParameter("@P_RelatedToIncomingLetter", letter.RelatedToIncomingLetter),
                new SqlParameter("@P_LetterDetails", letter.LetterDetails),
                new SqlParameter("@P_Notes", letter.Notes),
                new SqlParameter("@P_DocumentClassification", letter.DocumentClassification),
                new SqlParameter("@P_Priority", letter.Priority),
                new SqlParameter("@P_NeedReply", letter.NeedReply),
                new SqlParameter("@P_LetterPhysicallySend", letter.LetterPhysicallySend),
                new SqlParameter("@P_ApproverDepartmentId", letter.ApproverDepartmentId),
                new SqlParameter("@P_Action", letter.Action),
                new SqlParameter("@P_Comment", letter.Comments),
                new SqlParameter("@P_CreatedBy", letter.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", letter.CreatedDateTime),
                new SqlParameter("@P_IsGovernmentEntity", letter.IsGovernmentEntity),
                new SqlParameter("@P_ReceivedFromEntityID", letter.ReceivedFromEntityID)
           };
            var result = SqlHelper.ExecuteProcedureReturnData<LetterInboundWorkflowModel>(connString, "Save_LetterInbound", r => r.TranslateAsLetterInboundSaveResponseList(), param);

            if (letter.DestinationUserId != null)
            {
                new LetterInboundDestinationClient().SaveUser(connString, letter.DestinationUserId, result.LetterID, lang);
            }

            if (letter.DestinationDepartmentId != null)
            {
                new LetterInboundDestinationClient().SaveDepartment(connString, letter.DestinationDepartmentId, result.LetterID);
            }

            if (letter.Keywords != null)
            {
                new LetterInboundKeywordClient().SaveUser(connString, letter.Keywords, result.LetterID, letter.CreatedBy);
            }

            if (letter.RelatedOutgoingLetters != null)
            {
                new LetterInboundRelatedOutgoingClient().Save(connString, letter.RelatedOutgoingLetters, result.LetterID, 0);
            }

            if (letter.RelatedIncomingLetters != null)
            {
                new LetterInboundRelatedOutgoingClient().Save(connString, letter.RelatedIncomingLetters, result.LetterID, 1);
            }

            if (letter.Attachments != null)
            {
                new LetterAttachmentClient().InboundLetterPostAttachments(connString, "InboundLetter", letter.Attachments, result.LetterID);
            }

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "InboundLetter"),
                new SqlParameter("@P_Action", letter.Action) };
            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.ApproverID = letter.ApproverId;

            SqlParameter[] getDestinationUserparam = {
                    new SqlParameter("@P_LetterID", result.LetterID),
                    new SqlParameter("@P_Language", lang),
            };
            result.DestinationUserID = SqlHelper.ExecuteProcedureReturnData<List<LetterInboundDestinationUsersModel>>(connString, "Get_LetterInboundDestinationUsers", r => r.TranslateAsLetterInboundDestinationUserList(), getDestinationUserparam);

            result.Action = letter.Action;

            return result;
        }

        public LetterInboundWorkflowModel PutLetter(string connString, LetterInboundPutModel letter, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_LetterID", letter.LetterID),
                new SqlParameter("@P_Title", letter.Title),
                new SqlParameter("@P_SourceOU", letter.SourceOU),
                new SqlParameter("@P_SourceName", letter.SourceName),
                new SqlParameter("@P_ReceivingDate", letter.ReceivingDate),
                new SqlParameter("@P_ReceivedFromGovernmentEntity", letter.ReceivedFromGovernmentEntity),
                new SqlParameter("@P_ReceivedFromName", letter.ReceivedFromName),
                new SqlParameter("@P_RelatedToIncomingLetter", letter.RelatedToIncomingLetter),
                new SqlParameter("@P_LetterDetails", letter.LetterDetails),
                new SqlParameter("@P_Notes", letter.Notes),
                new SqlParameter("@P_DocumentClassification", letter.DocumentClassification),
                new SqlParameter("@P_Priority", letter.Priority),
                new SqlParameter("@P_NeedReply", letter.NeedReply),
                new SqlParameter("@P_LetterPhysicallySend", letter.LetterPhysicallySend),
                new SqlParameter("@P_ApproverDepartmentId", letter.ApproverDepartmentId),
                new SqlParameter("@P_Action", letter.Action),
                new SqlParameter("@P_Comment", letter.Comments),
                new SqlParameter("@P_UpdatedBy", letter.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", letter.UpdatedDateTime),
                new SqlParameter("@P_IsGovernmentEntity", letter.IsGovernmentEntity),
                new SqlParameter("@P_ReceivedFromEntityID", letter.ReceivedFromEntityID),
                new SqlParameter("@P_DestinationRedirected_EscalatedUserID", letter.Action == "Escalate" ? letter.ApproverId : letter.Action == "Redirect" ? letter.DestinatoionRedirectedBy : 0)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<LetterInboundWorkflowModel>(connString, "Save_LetterInbound", r => r.TranslateAsLetterInboundSaveResponseList(), param);

            if (letter.DestinationUserId != null)
            {
                new LetterInboundDestinationClient().SaveUser(connString, letter.DestinationUserId, result.LetterID, lang);
            }

            if (letter.DestinationDepartmentId != null)
            {
                new LetterInboundDestinationClient().SaveDepartment(connString, letter.DestinationDepartmentId, result.LetterID);
            }

            if (letter.Keywords != null)
            {
                new LetterInboundKeywordClient().SaveUser(connString, letter.Keywords, result.LetterID, letter.UpdatedBy);
            }

            if (letter.RelatedOutgoingLetters != null)
            {
                new LetterInboundRelatedOutgoingClient().Save(connString, letter.RelatedOutgoingLetters, result.LetterID, 0);
            }

            if (letter.RelatedIncomingLetters != null)
            {
                new LetterInboundRelatedOutgoingClient().Save(connString, letter.RelatedIncomingLetters, result.LetterID, 1);
            }

            if (letter.Attachments != null)
            {
                new LetterAttachmentClient().InboundLetterPostAttachments(connString, "InboundLetter", letter.Attachments, result.LetterID);
            }

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "InboundLetter"),
                new SqlParameter("@P_Action", letter.Action),
                new SqlParameter("@P_ServiceID", letter.LetterID) };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.ApproverID = letter.ApproverId;

            SqlParameter[] getDestinationUserparam = {
                    new SqlParameter("@P_LetterID", result.LetterID),
                    new SqlParameter("@P_Language", lang),
            };

            result.DestinationUserID = SqlHelper.ExecuteProcedureReturnData<List<LetterInboundDestinationUsersModel>>(connString, "Get_LetterInboundDestinationUsers", r => r.TranslateAsLetterInboundDestinationUserList(), getDestinationUserparam);
            result.Action = letter.Action;

            if (result.Action == "Resubmit" || (result.Action == "Submit") || result.Action == "Save")
                result.ApproverID = letter.ApproverId;
            SqlParameter[] param1 = {
                new SqlParameter("@P_LetterID", letter.LetterID) };

            result.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<LetterInboundGetModel>>(connString, "Get_LetterInboundByID", r => r.TranslateAsLetterInboundList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber) };

            result.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_LetterInboundByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            return result;
        }

        public string DeleteLetter(string connString, int id)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_LetterID", id) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Delete_LetterInboundByID", param);
        }

        public LetterInboundPutModel GetPatchLetterByID(string connString, int letterID, string lang)
        {
            LetterInboundPutModel letterDetails = new LetterInboundPutModel();

            SqlParameter[] param = {
                new SqlParameter("@P_LetterID", letterID),
                new SqlParameter("@P_UserID", 0)
            };

            SqlParameter[] getDestinationUserparam = {
                    new SqlParameter("@P_LetterID", letterID),
                    new SqlParameter("@P_Language", lang),
            };

            SqlParameter[] getDestinationDepartmentparam = {
                    new SqlParameter("@P_LetterID", letterID) };

            if (letterID != 0)
            {
                letterDetails = SqlHelper.ExecuteProcedureReturnData<List<LetterInboundPutModel>>(connString, "Get_LetterInboundByID", r => r.TranslateAsPutLetterInboundList(), param).FirstOrDefault();

                letterDetails.DestinationUserId = SqlHelper.ExecuteProcedureReturnData<List<LetterInboundDestinationUsersModel>>(connString, "Get_LetterInboundDestinationUsers", r => r.TranslateAsLetterInboundDestinationUserList(), getDestinationUserparam);

                letterDetails.DestinationDepartmentId = SqlHelper.ExecuteProcedureReturnData<List<LetterInboundDestinationDepartmentModel>>(connString, "Get_LetterInboundDestinationDepartment", r => r.TranslateAsLetterInboundDestinationDepartmentList(), getDestinationDepartmentparam);

                letterDetails.Attachments = new LetterAttachmentClient().GetAttachmentById(connString, letterDetails.LetterID, "InboundLetter");

                SqlParameter[] getKeywordparam = {
                    new SqlParameter("@P_LetterID", letterID),
                    new SqlParameter("@P_Type", 1) };

                letterDetails.Keywords = SqlHelper.ExecuteProcedureReturnData<List<LetterInboundKeywordsModel>>(connString, "Get_LetterInboundKeywords", r => r.TranslateAsLetterInboundKeywordsList(), getKeywordparam);
            }

            return letterDetails;
        }

        public List<GovernmentEntityModel> GetEntity(string connString, int entity, string entityName, int entityID)
        {
            SqlParameter[] relatedEntityParam = {
                new SqlParameter("@P_Entity", entity), new SqlParameter("@P_EntityName", entityName),
                new SqlParameter("@P_EntityID", entityID) };
            return SqlHelper.ExecuteProcedureReturnData<List<GovernmentEntityModel>>(connString, "Get_LetterEntityList", r => r.TranslateAsEntity(), relatedEntityParam);
        }

        public List<LetterRelatedOutgoingLetterModel> GetRelatedLetter(string connString, int userID, int type)
        {
            SqlParameter[] relatedOutgoingLetterParam = {
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Type", type)
             };
            return SqlHelper.ExecuteProcedureReturnData<List<LetterRelatedOutgoingLetterModel>>(connString, "Get_LetterOutboundLinkToLetter", r => r.TranslateAsRelatedLetterList(), relatedOutgoingLetterParam);
        }

        public List<LetterRelatedOutgoingLetterModel> GetRelatedLetterinout(string connString, int userID, int type, string refno)
        {
            SqlParameter[] relatedOutgoingLetterParam = {
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Type", type),
                new SqlParameter("@P_RefNo", refno)
             };
            return SqlHelper.ExecuteProcedureReturnData<List<LetterRelatedOutgoingLetterModel>>(connString, "Get_LetterOutboundLinkToLetterwithRef", r => r.TranslateAsRelatedLetterList(), relatedOutgoingLetterParam);
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

        public string SaveInboundClone(string connString, int letterID, int userID)
        {
            SqlParameter[] cloneparam = {
            new SqlParameter("@P_UserID", userID),
            new SqlParameter("@P_LetterID", letterID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_LetterInboundClone", cloneparam);
        }

        public LetterInBoundPreviewModel GetLetterPreview(string connString, int letterID, int userID, string lang)
        {
            LetterInBoundPreviewModel letterDetails = new LetterInBoundPreviewModel();
            SqlParameter[] param = {
                new SqlParameter("@P_LetterID", letterID),
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Language", lang)
            };
            SqlParameter[] destinationUserparam = {
                    new SqlParameter("@P_LetterID", letterID),
                    new SqlParameter("@P_Language", lang),
            };
            SqlParameter[] destinationDepartmentparam = {
                    new SqlParameter("@P_LetterID", letterID) };
            SqlParameter[] getRelatedOutgoingparam = {
                    new SqlParameter("@P_LetterID", letterID) };

            if (letterID != 0)
            {
                letterDetails = SqlHelper.ExecuteProcedureReturnData<List<LetterInBoundPreviewModel>>(connString, "Get_LetterInboundPreview", r => r.TranslateAsLetterInboundPreviewList(), param).FirstOrDefault();

                letterDetails.DestinationUserId = SqlHelper.ExecuteProcedureReturnData<List<LetterInboundDestinationUsersModel>>(connString, "Get_LetterInboundDestinationUsers", r => r.TranslateAsLetterInboundDestinationUserList(), destinationUserparam);

                letterDetails.DestinationDepartmentId = SqlHelper.ExecuteProcedureReturnData<List<LetterInboundDestinationDepartmentModel>>(connString, "Get_LetterInboundDestinationDepartment", r => r.TranslateAsLetterInboundDestinationDepartmentList(), destinationDepartmentparam);
            }

            return letterDetails;
        }

        internal LetterInboundWorkflowModel PatchLetter(string connString, int id, JsonPatchDocument<LetterInboundPutModel> value, string lang)
        {
            var result = GetPatchLetterByID(connString, id, lang);

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
                res.ApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_LetterInboundByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam).FirstOrDefault().ApproverId;
            }

            if (result.Action == "Redirect")
            {
                res.DestinationRedirectedBy = result.DestinatoionRedirectedBy;
            }

            res.Action = result.Action;
            SqlParameter[] param1 = {
                new SqlParameter("@P_LetterID", result.LetterID) };

            res.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<LetterInboundGetModel>>(connString, "Get_LetterInboundByID", r => r.TranslateAsLetterInboundList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", result.LetterReferenceNumber) };

            res.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_LetterInboundByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            return res;
        }
    }
}
