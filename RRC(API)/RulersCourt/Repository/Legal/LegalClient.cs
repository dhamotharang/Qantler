using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.Legal;
using RulersCourt.Translators;
using RulersCourt.Translators.Legal;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Legal
{
    public class LegalClient
    {
        public LegalGetModel GetLegalByID(string connString, int legalID, int userID, string lang)
        {
            LegalGetModel legalDetails = new LegalGetModel();

            SqlParameter[] param = {
                new SqlParameter("@P_LegalID", legalID)
            };

            if (legalID != 0)
            {
                legalDetails = SqlHelper.ExecuteProcedureReturnData<List<LegalGetModel>>(connString, "Get_LegalByID", r => r.TranslateAsLegalList(), param).FirstOrDefault();
                legalDetails.Attachments = new LegalAttachmentClient().GetAttachmentById(connString, legalDetails.LegalID, "Legal");
                legalDetails.CommunicationHistory = new LegalCommunicationHistoryClient().LegalCommunicationHistoryByLegalID(connString, legalID, lang);
                userID = legalDetails.CreatedBy.GetValueOrDefault();

                SqlParameter[] getKeywordparam = {
                    new SqlParameter("@P_LegalID", legalID),
                    new SqlParameter("@P_Type", 1),
                    new SqlParameter("@P_UserID", userID)
                };

                legalDetails.Keywords = SqlHelper.ExecuteProcedureReturnData<List<LegalKeywordsModel>>(connString, "Get_LegalKeywords", r => r.TranslateAsGetLegalkeywordsList(), getKeywordparam);

                SqlParameter[] getAssignparam = {
                    new SqlParameter("@P_ReferenceNumber", legalDetails.ReferenceNumber),
                    new SqlParameter("@P_Method", "0")
                };

                legalDetails.AssigneeID = SqlHelper.ExecuteProcedureReturnData<List<CurrentLegalAssigneeModel>>(connString, "Get_LegalByAssigneeandHRId", r => r.TranslateAsCurrentLegalAssigneeList(), getAssignparam);

                SqlParameter[] getLegalUserparam = {
                    new SqlParameter("@P_ReferenceNumber", legalDetails.ReferenceNumber),
                    new SqlParameter("@P_Method", 1)
                };

                legalDetails.LegalHeadUserID = SqlHelper.ExecuteProcedureReturnData<List<CurrentLegalHeadModel>>(connString, "Get_LegalByAssigneeandHRId", r => r.TranslateAsCurrentLegalHeadList(), getLegalUserparam);
                userID = legalDetails.CreatedBy.GetValueOrDefault();
            }

            Parallel.Invoke(
              () => legalDetails.M_OrganizationList = GetM_Organisation(connString, lang),
              () => legalDetails.M_LookupsList = GetM_Lookups(connString, lang));

            return legalDetails;
        }

        public List<OrganizationModel> GetM_Organisation(string connString, string lang)
        {
            SqlParameter[] param = { new SqlParameter("@P_Language", lang) };
            var e = SqlHelper.ExecuteProcedureReturnData<List<OrganizationModel>>(connString, "Get_Organization", r => r.TranslateAsOrganizationList(), param);
            return e;
        }

        public List<M_LookupsModel> GetM_Lookups(string connString, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Type", "Legal"),
                new SqlParameter("@P_Language", lang)
            };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        public List<LegalKeywordsModel> GetL_Keyword(string connString, int legalID, int userID)
        {
            SqlParameter[] getKeywordparama = {
                    new SqlParameter("@P_LegalID", legalID),
                    new SqlParameter("@P_Type", 2),
                    new SqlParameter("@P_UserID", userID)
            };

            return SqlHelper.ExecuteProcedureReturnData<List<LegalKeywordsModel>>(connString, "Get_LegalKeywords", r => r.TranslateAsGetLegalkeywordsList(), getKeywordparama);
        }

        public LegalListModel GetLegal(string connString, int pageNumber, int pageSize, string type, string userID, string status, string userName, string sourceOU, string subject, DateTime? requestDateFrom, DateTime? requestDateTo, string label, string attendedBy, string smartSearch, string lang)
        {
            LegalListModel list = new LegalListModel();

            SqlParameter[] param = {
                   new SqlParameter("@P_PageNumber", pageNumber),
                   new SqlParameter("@P_PageSize", pageSize),
                   new SqlParameter("@P_Type", type),
                   new SqlParameter("@P_UserID", userID),
                   new SqlParameter("@P_Method", 0),
                   new SqlParameter("@P_Status", status),
                   new SqlParameter("@P_UserName", userName),
                   new SqlParameter("@P_SourceOU", sourceOU),
                   new SqlParameter("@P_Subject", subject),
                   new SqlParameter("@P_RequestDateFrom ", requestDateFrom),
                   new SqlParameter("@P_RequestDateTo", requestDateTo),
                   new SqlParameter("@P_Label", label),
                   new SqlParameter("@P_AttendedBy", attendedBy),
                   new SqlParameter("@P_Language", lang),
                   new SqlParameter("@P_SmartSearch", smartSearch)
            };

            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<LegalDashboardListModel>>(connString, "Get_LegalList", r => r.TranslateAsDashboardLegalList(), param);

            SqlParameter[] parama = {
                   new SqlParameter("@P_PageNumber", pageNumber),
                   new SqlParameter("@P_PageSize", pageSize),
                   new SqlParameter("@P_Type", type),
                   new SqlParameter("@P_UserID", userID),
                   new SqlParameter("@P_Method", 1),
                   new SqlParameter("@P_Status", status),
                   new SqlParameter("@P_UserName", userName),
                   new SqlParameter("@P_SourceOU", sourceOU),
                   new SqlParameter("@P_Subject", subject),
                   new SqlParameter("@P_RequestDateFrom", requestDateFrom),
                   new SqlParameter("@P_RequestDateTo", requestDateTo),
                   new SqlParameter("@P_Label", label),
                   new SqlParameter("@P_AttendedBy", attendedBy),
                   new SqlParameter("@P_Language", lang),
                   new SqlParameter("@P_SmartSearch", smartSearch)
            };

            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_LegalList", parama);

            Parallel.Invoke(
              () => list.OrganizationList = GetM_Organisation(connString, lang),
              () => list.LookupsList = GetM_Lookups(connString, lang));

            return list;
        }

        public LegalWorkflowModel PostLegal(string connString, LegalPostModel legal)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_SourceOU", legal.SourceOU),
                new SqlParameter("@P_SourceName", legal.SourceName),
                new SqlParameter("@P_Subject", legal.Subject),
                new SqlParameter("@P_RequestDetails", legal.RequestDetails),
                new SqlParameter("@P_CreatedBy", legal.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", legal.CreatedDateTime),
                new SqlParameter("@P_Action", legal.Action),
                new SqlParameter("@P_Comments", legal.Comments)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<LegalWorkflowModel>(connString, "Save_Legal", r => r.TranslateAsLegalSaveResponseList(), param);

            if (legal.Keywords != null)
            {
                new LegalKeywordClient().SaveUser(connString, legal.Keywords, result.LegalID, legal.CreatedBy);
            }

            if (legal.Attachments != null)
                new LegalAttachmentClient().PostAttachments(connString, "Legal", legal.Attachments, result.LegalID);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "Legal"),
                new SqlParameter("@P_Action", legal.Action)
            };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));
            result.FromID = legal.CreatedBy ?? default(int);
            result.Action = legal.Action;

            SqlParameter[] parama = { new SqlParameter("@P_Department", 16), new SqlParameter("@P_GetHead", 1) };
            result.LegalHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_UserByUnits", r => r.TranslateAsUserList(), parama);

            return result;
        }

        public LegalWorkflowModel PutLegal(string connString, LegalPutModel legal)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_LegalID", legal.LegalID),
                new SqlParameter("@P_SourceOU", legal.SourceOU),
                new SqlParameter("@P_SourceName", legal.SourceName),
                new SqlParameter("@P_Subject", legal.Subject),
                new SqlParameter("@P_RequestDetails", legal.RequestDetails),
                new SqlParameter("@P_UpdatedBy", legal.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", legal.UpdatedDateTime),
                new SqlParameter("@P_Action", legal.Action),
                new SqlParameter("@P_Comments", legal.Comments),
                new SqlParameter("@P_DeleteFlag", legal.DeleteFlag)
            };
            var result = SqlHelper.ExecuteProcedureReturnData<LegalWorkflowModel>(connString, "Save_Legal", r => r.TranslateAsLegalSaveResponseList(), param);

            if (legal.Keywords != null)
            {
                new LegalKeywordClient().SaveUser(connString, legal.Keywords, result.LegalID, legal.UpdatedBy);
            }

            if (legal.Attachments != null)
                new LegalAttachmentClient().PostAttachments(connString, "Legal", legal.Attachments, result.LegalID);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "Legal"),
                new SqlParameter("@P_Action", legal.Action)
            };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));
            result.FromID = legal.UpdatedBy ?? default(int);
            result.Action = legal.Action;

            SqlParameter[] getAssignparam = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber) };
            result.AssigneeID = legal.AssigneeID;

            SqlParameter[] parama = { new SqlParameter("@P_Department", 16), new SqlParameter("@P_GetHead", 1) };
            result.LegalHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_UserByUnits", r => r.TranslateAsUserList(), parama);
            return result;
        }

        public LegalPutModel GetPatchLegalByID(string connString, int legalID)
        {
            LegalPutModel legalDetails = new LegalPutModel();

            SqlParameter[] param = {
                new SqlParameter("@P_LegalID", legalID)
            };

            if (legalID != 0)
            {
                legalDetails = SqlHelper.ExecuteProcedureReturnData<List<LegalPutModel>>(connString, "Get_LegalByID", r => r.TranslateAsPutLegalList(), param).FirstOrDefault();

                SqlParameter[] getKeywordparam = {
                    new SqlParameter("@P_LegalID", legalID),
                    new SqlParameter("@P_Type", 1)
                };

                legalDetails.Keywords = SqlHelper.ExecuteProcedureReturnData<List<LegalKeywordsModel>>(connString, "Get_LegalKeywords", r => r.TranslateAsGetLegalkeywordsList(), getKeywordparam);
            }

            return legalDetails;
        }

        public int SaveCommunicationChat(string connString, LegalCommunicationHistory chat)
        {
            SqlParameter[] param = {
                    new SqlParameter("@P_CommunicationID", chat.CommunicationID),
                    new SqlParameter("@P_LegalID", chat.LegalID),
                    new SqlParameter("@P_Message", chat.Message),
                    new SqlParameter("@P_ParentCommunicationID", chat.ParentCommunicationID),
                    new SqlParameter("@P_CreatedBy", chat.CreatedBy),
                    new SqlParameter("@P_CreatedDateTime", chat.CreatedDateTime)
            };
            return int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Save_LegalCommunicationHistory", param));
        }

        public LegalHomeDashboardModel GetAllModulesPendingTasksCount(string connString, int userID)
        {
            LegalHomeDashboardModel list = new LegalHomeDashboardModel();
            SqlParameter[] myRequestparam = {
                       new SqlParameter("@P_UserID", userID),
                        };

            list = SqlHelper.ExecuteProcedureReturnData<LegalHomeDashboardModel>(connString, "LegalDashboardCount", r => r.TranslateaslegalDashboardcount(), myRequestparam);
            return list;
        }

        public DocumentListModel GetDocument(string connString, int userID, string type, int pageNumber, int pageSize, string creator, string smartSearch, string lang)
        {
            var result = new DocumentListModel();

            SqlParameter[] parama = {
            new SqlParameter("@P_PageNumber", pageNumber),
            new SqlParameter("@P_PageSize", pageSize),
            new SqlParameter("@P_Type", type),
            new SqlParameter("@P_Creator", creator),
            new SqlParameter("@P_SmartSearch", smartSearch),
            new SqlParameter("@P_Method", 0),
            new SqlParameter("@P_Language", lang)
            };

            result.Collection = SqlHelper.ExecuteProcedureReturnData<List<DocumentGetModel>>(connString, "Get_DocumentByID", r => r.TranslateAsCocumentList(), parama);

            SqlParameter[] param = {
            new SqlParameter("@P_PageNumber", pageNumber),
            new SqlParameter("@P_PageSize", pageSize),
            new SqlParameter("@P_Type", type),
            new SqlParameter("@P_Creator", creator),
            new SqlParameter("@P_SmartSearch", smartSearch),
            new SqlParameter("@P_Method", 1),
            new SqlParameter("@P_Language", lang),
            };

            result.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_DocumentByID", param);

            return result;
        }

        public string PostDocument(string connString, DocumentPostModel value)
        {
            var result = new DocumentListModel();

            SqlParameter[] parama = {
            new SqlParameter("@P_Type", value.Type),
            new SqlParameter("@P_AttachmentGuid", value.AttachmentGuid),
            new SqlParameter("@P_AttachmentsName", value.AttachmentsName),
            new SqlParameter("@P_CreatedBy", value.CreatedBy),
            new SqlParameter("@P_CreatedDateTime", value.CreatedDateTime)
            };

            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_Documents", parama);
        }

        public string PutDocument(string connString, DocumentPutModel value)
        {
            var result = new DocumentListModel();

            SqlParameter[] parama = {
            new SqlParameter("@P_Type", value.Type),
            new SqlParameter("@P_AttachmentID", value.AttachmentID),
            new SqlParameter("@P_AttachmentGuid", value.AttachmentGuid),
            new SqlParameter("@P_AttachmentsName", value.AttachmentsName),
            new SqlParameter("@P_UpdatedBy", value.UpdatedBy),
            new SqlParameter("@P_UpdatedDateTime", value.UpdatedDateTime)
            };

            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_Documents", parama);
        }

        public string DeleteDocument(string connString, int attachmentId, string userID)
        {
            var result = new DocumentListModel();

            SqlParameter[] parama = {
            new SqlParameter("@P_Type", "Delete"),
            new SqlParameter("@P_AttachmentID", attachmentId),
            new SqlParameter("@P_UpdatedBy", userID)
            };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_Documents", parama);
        }

        public void Postlabel(string connString, List<LegalKeywordsModel> legal, int legalID, int? userID)
        {
            new LegalKeywordClient().SaveUser(connString, legal, legalID, userID);
        }

        internal LegalWorkflowModel PatchLegal(string connString, int id, JsonPatchDocument<LegalPutModel> value)
        {
            var result = GetPatchLegalByID(connString, id);

            value.ApplyTo(result);
            var res = PutLegal(connString, result);
            return res;
        }
    }
}
