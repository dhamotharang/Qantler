using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.HR;
using RulersCourt.Models.Maintenance;
using RulersCourt.Translators;
using RulersCourt.Translators.Maintenance;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Maintenance
{
    public class MaintenanceClient
    {
        public MaintenanceGetModel GetMaintenanceByID(string connString, int maintenanceID, int userID, string lang)
        {
            MaintenanceGetModel maintenanceDetails = new MaintenanceGetModel();
            SqlParameter[] param = {
                new SqlParameter("@P_MaintenanceID", maintenanceID) };

            if (maintenanceID != 0)
            {
                maintenanceDetails = SqlHelper.ExecuteProcedureReturnData<List<MaintenanceGetModel>>(connString, "Get_MaintenanceByID", r => r.TranslateAsMaintenanceList(), param).FirstOrDefault();

                maintenanceDetails.Attachments = new MaintenanceAttachmentClient().GetAttachmentById(connString, maintenanceDetails.MaintenanceID, "Maintenance");

                maintenanceDetails.CommunicationHistory = new MaintenanceCommunicationHistoryClient().MaintenanceCommunicationHistoryByID(connString, maintenanceID, lang);

                SqlParameter[] getApproverparam = {
                    new SqlParameter("@P_ReferenceNumber", maintenanceDetails.ReferenceNumber),
                    new SqlParameter("@P_UserID", userID) };

                maintenanceDetails.CurrentApprover = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_MaintenanceByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam);

                SqlParameter[] getAssignparam = {
                    new SqlParameter("@P_ReferenceNumber", maintenanceDetails.ReferenceNumber) };

                maintenanceDetails.AssigneeID = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_MaintenanceAssigneeID", getAssignparam));

                userID = maintenanceDetails.CreatedBy.GetValueOrDefault();
            }

            Parallel.Invoke(
              () => maintenanceDetails.M_OrganizationList = GetL_Organisation(connString, lang),
              () => maintenanceDetails.M_LookupsList = GetM_Lookups(connString, lang),
              () => maintenanceDetails.M_ApproverDepartmentList = new GetApproverConfiguration().GetM_ApproverDeparment(connString, lang));

            return maintenanceDetails;
        }

        public List<OrganizationModel> GetL_Organisation(string connString, string lang)
        {
            SqlParameter[] param = { new SqlParameter("@P_Language", lang) };
            var e = SqlHelper.ExecuteProcedureReturnData<List<OrganizationModel>>(connString, "Get_Organization", r => r.TranslateAsOrganizationList(), param);
            return e;
        }

        public List<M_LookupsModel> GetM_Lookups(string connString, string lang)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Type", "Maintenance"),
                new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        public MaintenanceHomeListModel GetMaintenance(string connString, int pageNumber, int pageSize, string type, string userID, string status, string source, string subject, string stringUserName, string priority, string attendedBy, DateTime? dateFrom, DateTime? dateTo, string smartSearch, string lang)
        {
            MaintenanceHomeListModel list = new MaintenanceHomeListModel();
            SqlParameter[] param = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Type", type),
                new SqlParameter("@P_Method", 0),
                new SqlParameter("@P_Username", stringUserName),
                new SqlParameter("@P_Status", status),
                new SqlParameter("@P_SourceOU", source),
                new SqlParameter("@P_Subject", subject),
                new SqlParameter("@P_AttendedBy", attendedBy),
                new SqlParameter("@P_Priority", priority),
                new SqlParameter("@P_RequestDateFrom", dateFrom),
                new SqlParameter("@P_RequestDateTo", dateTo),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_SmartSearch", smartSearch) };
            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<MaintenanceHomeDashboardListModel>>(connString, "Get_MaintenanceList", r => r.TranslateAsMaintenanceDashboardList(), param);

            SqlParameter[] parama = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Type", type),
                new SqlParameter("@P_Method", 1),
                new SqlParameter("@P_Username", stringUserName),
                new SqlParameter("@P_Status", status),
                new SqlParameter("@P_SourceOU", source),
                new SqlParameter("@P_Subject", subject),
                new SqlParameter("@P_AttendedBy", attendedBy),
                new SqlParameter("@P_Priority", priority),
                new SqlParameter("@P_RequestDateFrom", dateFrom),
                new SqlParameter("@P_RequestDateTo", dateTo),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_SmartSearch", smartSearch) };
            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_MaintenanceList", parama);
            Parallel.Invoke(
                () => list.M_OrganizationList = GetL_Organisation(connString, lang),
                () => list.LookupsList = GetM_Lookups(connString, lang),
                () => list.M_ApproverDepartmentList = new GetApproverConfiguration().GetM_ApproverDeparment(connString, lang));
            return list;
        }

        public MaintenanceWorkflowModel PostMaintenance(string connString, MaintenancePostModel maintenance)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_SourceOU", maintenance.SourceOU),
                new SqlParameter("@P_SourceName", maintenance.SourceName),
                new SqlParameter("@P_Subject", maintenance.Subject),
                new SqlParameter("@P_RequestDetails", maintenance.RequestDetails),
                new SqlParameter("@P_Priority", maintenance.Priority),
                new SqlParameter("@P_ApproverDepartmentID", maintenance.ApproverDepartmentID),
                new SqlParameter("@P_RequestorID", maintenance.RequestorID),
                new SqlParameter("@P_RequestorDepartmentID", maintenance.RequestorDepartmentID),
                new SqlParameter("@P_Action", maintenance.Action),
                new SqlParameter("@P_Comment", maintenance.Comments),
                new SqlParameter("@P_CreatedBy", maintenance.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", maintenance.CreatedDateTime)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<MaintenanceWorkflowModel>(connString, "Save_Maintenance", r => r.TranslateAsMaintenanceSaveResponseList(), param);

            if (maintenance.Attachments != null)
            {
                new MaintenanceAttachmentClient().MaintenancePostAttachments(connString, "Maintenance", maintenance.Attachments, result.MaintenanceID);
            }

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "Maintenance"),
                new SqlParameter("@P_Action", maintenance.Action) };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            SqlParameter[] parama = { new SqlParameter("@P_Department", 12), new SqlParameter("@P_GetHead", 1) };
            result.MaintenanceHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_UserByUnits", r => r.TranslateAsUserList(), parama);

            result.ApproverID = maintenance.ApproverID;
            result.Action = maintenance.Action;

            return result;
        }

        public MaintenanceWorkflowModel PutMaintenance(string connString, MaintenancePutModel maintenance)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_MaintenanceID", maintenance.MaintenanceID),
                new SqlParameter("@P_SourceOU", maintenance.SourceOU),
                new SqlParameter("@P_SourceName", maintenance.SourceName),
                new SqlParameter("@P_Subject", maintenance.Subject),
                new SqlParameter("@P_RequestDetails", maintenance.RequestDetails),
                new SqlParameter("@P_Priority", maintenance.Priority),
                new SqlParameter("@P_ApproverDepartmentID", maintenance.ApproverDepartmentID),
                new SqlParameter("@P_RequestorID", maintenance.RequestorID),
                new SqlParameter("@P_RequestorDepartmentID", maintenance.RequestorDepartmentID),
                new SqlParameter("@P_Action", maintenance.Action),
                new SqlParameter("@P_Comment", maintenance.Comments),
                new SqlParameter("@P_DeleteFlag", maintenance.DeleteFlag),
                new SqlParameter("@P_UpdatedBy", maintenance.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", maintenance.UpdatedDateTime)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<MaintenanceWorkflowModel>(connString, "Save_Maintenance", r => r.TranslateAsMaintenanceSaveResponseList(), param);

            if (maintenance.Attachments != null)
            {
                new MaintenanceAttachmentClient().MaintenancePostAttachments(connString, "Maintenance", maintenance.Attachments, result.MaintenanceID);
            }

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "Maintenance"),
                new SqlParameter("@P_Action", maintenance.Action) };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            SqlParameter[] parama = { new SqlParameter("@P_Department", 12), new SqlParameter("@P_GetHead", 1) };
            result.MaintenanceHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_UserByUnits", r => r.TranslateAsUserList(), parama);

            result.ApproverID = maintenance.ApproverID;
            result.Action = maintenance.Action;
            result.AssigneeID = maintenance.AssigneeID;
            result.RequestorID = maintenance.RequestorID;
            SqlParameter[] param1 = {
                new SqlParameter("@P_MaintenanceID", result.MaintenanceID) };

            result.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<MaintenanceGetModel>>(connString, "Get_MaintenanceByID", r => r.TranslateAsMaintenanceList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber) };

            result.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_MaintenanceByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            return result;
        }

        public void PutMaintenanceAttachments(string connString, List<MaintenanceAttachmentGetModel> attachments, int id)
        {
            new MaintenanceAttachmentClient().MaintenancePostAttachments(connString, "Maintenance", attachments, id);
        }

        public string DeleteMaintenance(string connString, int id)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_MaintenanceID", id)
             };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Delete_MaintenanceByID", param);
        }

        public MaintenancePutModel GetPatchLetterByID(string connString, int maintenanceID)
        {
            MaintenancePutModel maintenanceDetails = new MaintenancePutModel();

            SqlParameter[] param = {
                new SqlParameter("@P_MaintenanceID", maintenanceID) };

            if (maintenanceID != 0)
            {
                maintenanceDetails = SqlHelper.ExecuteProcedureReturnData<List<MaintenancePutModel>>(connString, "Get_MaintenanceByID", r => r.TranslateAsPutMaintenanceList(), param).FirstOrDefault();

                maintenanceDetails.Attachments = new MaintenanceAttachmentClient().GetAttachmentById(connString, maintenanceDetails.MaintenanceID, "Maintenance");
            }

            return maintenanceDetails;
        }

        public int SaveCommunicationChat(string connString, MaintenanceCommunicationHistory chat)
        {
            SqlParameter[] param =
                    { new SqlParameter("@P_CommunicationID", chat.CommunicationID),
                    new SqlParameter("@P_MaintenanceID", chat.MaintenanceID),
                    new SqlParameter("@P_Message", chat.Message),
                    new SqlParameter("@P_ParentCommunicationID", chat.ParentCommunicationID),
                    new SqlParameter("@P_CreatedBy", chat.CreatedBy),
                    new SqlParameter("@P_CreatedDateTime", chat.CreatedDateTime)
                    };
            return int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Save_MaintenanceCommunicationHistory", param));
        }

        public List<MaintenanceReportModel> GetReportExportList(string connString, MaintenanceReportRequestModel report, string lang)
        {
            List<MaintenanceReportModel> list = new List<MaintenanceReportModel>();
            SqlParameter[] param = {
            new SqlParameter("@P_PageNumber", 0),
            new SqlParameter("@P_PageSize", 0),
            new SqlParameter("@P_Method", 2),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_Status", report.Status),
            new SqlParameter("@P_SourceOU", report.SourceOU),
            new SqlParameter("@P_Subject", report.Subject),
            new SqlParameter("@P_RequestDateFrom", report.RequestDateRangeFrom),
            new SqlParameter("@P_RequestDateTo", report.RequestDateRangeTo),
            new SqlParameter("@P_AttendedBy", report.AttendedBy),
            new SqlParameter("@P_Language", lang),
            new SqlParameter("@P_Priority", report.Priority),
            new SqlParameter("@P_SmartSearch", report.SmartSearch)
            };

            list = SqlHelper.ExecuteProcedureReturnData<List<MaintenanceReportModel>>(connString, "MaintenanceReport", r => r.TranslateAsMaintenanceReportList(), param);

            return list;
        }

        public MaintenanceHomeCountModel GetAllModulesPendingTasksCount(string connString, int userID)
        {
            MaintenanceHomeCountModel list = new MaintenanceHomeCountModel();
            SqlParameter[] myRequestparam = {
                       new SqlParameter("@P_UserID", userID),
                        };
            list = SqlHelper.ExecuteProcedureReturnData<MaintenanceHomeCountModel>(connString, "MaintananceDashboardCount", r => r.TranslateasmaintananceDashboardcount(), myRequestparam);
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
            new SqlParameter("@P_Language", lang)
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
            new SqlParameter("@P_CreatedDateTime", value.CreatedDateTime) };

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
            new SqlParameter("@P_UpdatedDateTime", value.UpdatedDateTime) };

            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_Documents", parama);
        }

        public string DeleteDocument(string connString, int attachmentId, string userID)
        {
            var result = new DocumentListModel();

            SqlParameter[] parama = {
            new SqlParameter("@P_Type", "Delete"),
            new SqlParameter("@P_AttachmentID", attachmentId),
            new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_Documents", parama);
        }

        internal MaintenanceWorkflowModel PatchMaintenance(string connString, int id, JsonPatchDocument<MaintenancePutModel> value)
        {
            var result = GetPatchLetterByID(connString, id);

            value.ApplyTo(result);
            var res = PutMaintenance(connString, result);
            if (result.Action == "Escalate")
            {
                res.ApproverID = result.ApproverID;
            }

            res.Action = result.Action;

            SqlParameter[] parama = { new SqlParameter("@P_Department", 12), new SqlParameter("@P_GetHead", 1) };
            res.MaintenanceHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_UserByUnits", r => r.TranslateAsUserList(), parama);
            SqlParameter[] param1 = {
                new SqlParameter("@P_MaintenanceID", result.MaintenanceID) };

            res.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<MaintenanceGetModel>>(connString, "Get_MaintenanceByID", r => r.TranslateAsMaintenanceList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", res.ReferenceNumber) };

            res.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_MaintenanceByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;
            res.MaintenanceManagerUserID = result.MaintenanceManagerUserID;
            return res;
        }
    }
}
