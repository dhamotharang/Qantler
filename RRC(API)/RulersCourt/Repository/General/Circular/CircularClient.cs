using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.Circular;
using RulersCourt.Models.General.Circular;
using RulersCourt.Translators;
using RulersCourt.Translators.Circular;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Circular
{
    public class CircularClient
    {
        public CircularGetModel GetCircularByID(string connString, int circularID, int userID, string lang)
        {
            CircularGetModel circularDetails = new CircularGetModel();

            SqlParameter[] param = {
                new SqlParameter("@P_CircularID", circularID)
            };

            SqlParameter[] getDestinationDepartmentparam = {
                    new SqlParameter("@P_CircularID", circularID)
            };

            if (circularID != 0)
            {
                circularDetails = SqlHelper.ExecuteProcedureReturnData<List<CircularGetModel>>(connString, "Get_CircularByID", r => r.TranslateAsCircularList(), param).FirstOrDefault();

                circularDetails.DestinationDepartmentID = SqlHelper.ExecuteProcedureReturnData<List<CircularDestinationDepartmentGetModel>>(connString, "Get_CircularDestinationDepartment", r => r.CircularTranslateAsDestinationDepartmentList(), getDestinationDepartmentparam);

                circularDetails.Attachments = new CircularAttachmentClient().GetCircularAttachmentById(connString, circularDetails.CircularID, "Circular");

                circularDetails.HistoryLog = new CircularHistoryLogClient().CircularHistoryLogByCircularID(connString, circularID, lang);

                SqlParameter[] getApproverparam = {
                    new SqlParameter("@P_ReferenceNumber", circularDetails.ReferenceNumber),
                    new SqlParameter("@P_UserID", userID)
                };

                circularDetails.CurrentApprover = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_CircularByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam);

                userID = circularDetails.CreatedBy.GetValueOrDefault();
            }

            Parallel.Invoke(
              () => circularDetails.M_OrganizationList = GetM_Organisation(connString, lang),
              () => circularDetails.M_LookupsList = GetM_Lookups(connString, lang),
              () => circularDetails.M_ApproverDepartmentList = new GetApproverConfiguration().GetM_ApproverDeparment(connString, lang));

            return circularDetails;
        }

        public List<OrganizationModel> GetM_Organisation(string connString, string lang)
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
            SqlParameter[] param = {
                new SqlParameter("@P_Language", lang)
            };
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
                new SqlParameter("@P_Type", "Circular"),
                new SqlParameter("@P_Language", lang)
            };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        public CircularListModel GetCircular(string connString, int pageNumber, int pageSize, string type, string username, string status, string sourceOU, string stringDestinationOU, string priority, DateTime? dateFrom, DateTime? dateTo, string smartSearch, string lang)
        {
            EnumCircular e = (EnumCircular)Enum.ToObject(typeof(EnumCircular), Convert.ToInt32(type));

            CircularListModel list = new CircularListModel();

            SqlParameter[] param = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_Type", e),
                new SqlParameter("@P_Username", username),
                new SqlParameter("@P_Method", 0),
                new SqlParameter("@P_Status", status),
                new SqlParameter("@P_SourceOU", sourceOU),
                new SqlParameter("@P_DestinationOU", stringDestinationOU),
                new SqlParameter("@P_Priority", priority),
                new SqlParameter("@P_DateFrom", dateFrom),
                new SqlParameter("@P_DateTo", dateTo),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_SmartSearch", smartSearch)
            };

            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<CircularGetModel>>(connString, "Get_CircularList", r => r.TranslateAsCircularList(), param);

            SqlParameter[] parama = {
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_Type", e),
                new SqlParameter("@P_Username", username),
                new SqlParameter("@P_Method", 1),
                new SqlParameter("@P_Status", status),
                new SqlParameter("@P_SourceOU", sourceOU),
                new SqlParameter("@P_DestinationOU", stringDestinationOU),
                new SqlParameter("@P_Priority", priority),
                new SqlParameter("@P_DateFrom", dateFrom),
                new SqlParameter("@P_DateTo", dateTo),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_SmartSearch", smartSearch)
            };

            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_CircularList", parama);

            Parallel.Invoke(
              () => list.OrganizationList = GetM_OrganisationDashboard(connString, lang),
              () => list.LookupsList = GetM_Lookups(connString, lang),
              () => list.M_ApproverDepartmentList = new GetApproverConfiguration().GetM_ApproverDeparment(connString, lang));

            return list;
        }

        public CircularWorkflowModel PostCircular(string connString, CircularPostModel circular)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Title", circular.Title),
                new SqlParameter("@P_SourceOU", circular.SourceOU),
                new SqlParameter("@P_SourceName", circular.SourceName),
                new SqlParameter("@P_Details", circular.Details),
                new SqlParameter("@P_Priority", circular.Priority),
                new SqlParameter("@P_CreatedBy", circular.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", circular.CreatedDateTime),
                new SqlParameter("@P_Action", circular.Action),
                new SqlParameter("@P_Comment", circular.Comments),
                new SqlParameter("@P_ApproverDepartmentId", circular.ApproverDepartmentId)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<CircularWorkflowModel>(connString, "Save_Circular", r => r.TranslateAsCircularSaveResponseList(), param);

            if (circular.DestinationDepartmentId != null)
                new CircularDestinationClient().SaveDepartment(connString, circular.DestinationDepartmentId, result.CircularId);

            if (circular.Attachments != null)
                new CircularAttachmentClient().PostCircularAttachments(connString, "Circular", circular.Attachments, result.CircularId);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "Circular"),
                new SqlParameter("@P_Action", circular.Action)
            };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.ApproverID = circular.ApproverId;
            result.Action = circular.Action;

            return result;
        }

        public CircularWorkflowModel PutCircular(string connString, CircularPutModel circular)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_CircularID", circular.CircularID),
                new SqlParameter("@P_Title", circular.Title),
                new SqlParameter("@P_SourceOU", circular.SourceOU),
                new SqlParameter("@P_SourceName", circular.SourceName),
                new SqlParameter("@P_Details", circular.Details),
                new SqlParameter("@P_Priority", circular.Priority),
                new SqlParameter("@P_UpdatedBy", circular.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", circular.UpdatedDateTime),
                new SqlParameter("@P_Action", circular.Action),
                new SqlParameter("@P_Comment", circular.Comments),
                new SqlParameter("@P_DeleteFlag", circular.DeleteFlag),
                new SqlParameter("@P_ApproverDepartmentId", circular.ApproverDepartmentId)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<CircularWorkflowModel>(connString, "Save_Circular", r => r.TranslateAsCircularSaveResponseList(), param);

            if (circular.DestinationDepartmentId != null)
                new CircularDestinationClient().SaveDepartment(connString, circular.DestinationDepartmentId, result.CircularId);

            if (circular.Attachments != null)
                new CircularAttachmentClient().PostCircularAttachments(connString, "Circular", circular.Attachments, result.CircularId);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "Circular"),
                new SqlParameter("@P_Action", circular.Action)
            };
            if (circular.Action != null)
            {
                try
                {
                    result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));
                }
                catch (Exception)
                {
                }
            }

            SqlParameter[] getApproverparam = { new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber) };

            result.ApproverID = SqlHelper.ExecuteProcedureReturnData(connString, "Get_CircularByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam).FirstOrDefault().ApproverId;

            SqlParameter[] getDestinationUserparam = {
                    new SqlParameter("@P_CircularID", result.CircularId)
            };

            result.DestinationDepartmentID = SqlHelper.ExecuteProcedureReturnData(connString, "Get_CircularDestinationDepartment", r => r.CircularTranslateAsDestinationDepartmentList(), getDestinationUserparam);

            result.Action = circular.Action;

            if (result.Action == "Resubmit" || result.Action == "Submit" || result.Action == "Save")
                result.ApproverID = circular.ApproverId;

            SqlParameter[] param1 = {
                new SqlParameter("@P_CircularID", circular.CircularID)
            };

            result.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData(connString, "Get_CircularByID", r => r.TranslateAsCircularList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber)
            };

            result.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData(connString, "Get_CircularByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            return result;
        }

        public string DeleteCircular(string connString, int id)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_CircularID", id)
             };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Delete_CircularByID", param);
        }

        public CircularPutModel GetPatchCircularByID(string connString, int circularID)
        {
            CircularPutModel circularDetails = new CircularPutModel();

            SqlParameter[] param = {
                new SqlParameter("@P_CircularID", circularID)
            };

            SqlParameter[] getDestinationUserparam = {
                    new SqlParameter("@P_CircularID", circularID)
            };

            SqlParameter[] getDestinationDepartmentparam = {
                    new SqlParameter("@P_CircularID", circularID)
            };

            if (circularID != 0)
            {
                circularDetails = SqlHelper.ExecuteProcedureReturnData<List<CircularPutModel>>(connString, "Get_CircularByID", r => r.TranslateAsPutCircularList(), param).FirstOrDefault();

                circularDetails.DestinationDepartmentId = SqlHelper.ExecuteProcedureReturnData<List<CircularDestinationDepartmentGetModel>>(connString, "Get_CircularDestinationDepartment", r => r.CircularTranslateAsDestinationDepartmentList(), getDestinationDepartmentparam);

                circularDetails.Attachments = new CircularAttachmentClient().GetCircularAttachmentById(connString, circularDetails.CircularID, "Circular");
            }

            return circularDetails;
        }

        public List<CircularReport> GetCircularReportExportList(string connString, CircularReportRequestModel report, string lang)
        {
            List<CircularReport> list = new List<CircularReport>();
            if (!string.IsNullOrEmpty(report.SourceOU) & !string.IsNullOrEmpty(report.SourceOU))
            {
                report.SourceOU = report.SourceOU.Replace("amp;", "&");
            }

            if (!string.IsNullOrEmpty(report.DestinationOU) & !string.IsNullOrEmpty(report.DestinationOU))
            {
                report.DestinationOU = report.DestinationOU.Replace("amp;", "&");
            }

            SqlParameter[] param = {
            new SqlParameter("@P_PageNumber", 1),
            new SqlParameter("@P_PageSize", 10),
            new SqlParameter("@P_Method", 2),
            new SqlParameter("@P_UserID", report.UserID),
            new SqlParameter("@P_Status", report.Status),
            new SqlParameter("@P_SourceOU", report.SourceOU),
            new SqlParameter("@P_DestinationOU", report.DestinationOU),
            new SqlParameter("@P_DateFrom", report.DateRangeFrom),
            new SqlParameter("@P_DateTo", report.DateRangeTo),
            new SqlParameter("@P_Priority", report.Priority),
            new SqlParameter("@P_SmartSearch", report.SmartSearch),
            new SqlParameter("@P_Language", lang)
            };

            list = SqlHelper.ExecuteProcedureReturnData<List<CircularReport>>(connString, "CircularReport", r => r.TranslateAsCircularReportList(), param);

            return list;
        }

        public string SaveCircularClone(string connString, int circularID, int userID)
        {
            SqlParameter[] cloneparam = {
            new SqlParameter("@P_UserID", userID),
            new SqlParameter("@P_CircularID", circularID)
            };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_CircularClone", cloneparam);
        }

        public CircularPreviewModel GetCircularPreview(string connString, int circularID, int userID, string lang)
        {
            CircularPreviewModel circularDetails = new CircularPreviewModel();

            SqlParameter[] param = {
                new SqlParameter("@P_CircularID", circularID),
                new SqlParameter("@P_Language", lang),
            };

            SqlParameter[] getDestinationDepartmentparam = {
                    new SqlParameter("@P_CircularID", circularID)
            };

            if (circularID != 0)
            {
                circularDetails = SqlHelper.ExecuteProcedureReturnData<List<CircularPreviewModel>>(connString, "Get_CircularPreview", r => r.TranslateAsCircularPreviewList(), param).FirstOrDefault();

                circularDetails.DestinationDepartmentID = SqlHelper.ExecuteProcedureReturnData<List<CircularDestinationDepartmentGetModel>>(connString, "Get_CircularDestinationDepartment", r => r.CircularTranslateAsDestinationDepartmentList(), getDestinationDepartmentparam);

                circularDetails.Attachments = new CircularAttachmentClient().GetCircularAttachmentById(connString, circularDetails.CircularID, "Circular");

                circularDetails.HistoryLog = new CircularHistoryLogClient().CircularHistoryLogByCircularID(connString, circularID, lang);

                SqlParameter[] getApproverparam = {
                    new SqlParameter("@P_ReferenceNumber", circularDetails.ReferenceNumber),
                    new SqlParameter("@P_UserID", userID)
                };

                circularDetails.CurrentApprover = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_CircularByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam);

                userID = circularDetails.CreatedBy.GetValueOrDefault();
            }

            Parallel.Invoke(
              () => circularDetails.M_OrganizationList = GetM_Organisation(connString, lang),
              () => circularDetails.M_LookupsList = GetM_Lookups(connString, lang));

            return circularDetails;
        }

        internal CircularWorkflowModel PatchCircular(string connString, int id, JsonPatchDocument<CircularPutModel> value)
        {
            var result = GetPatchCircularByID(connString, id);

            value.ApplyTo(result);

            var res = PutCircular(connString, result);

            if (result.Action == "Escalate")
            {
                res.ApproverID = result.ApproverId;
            }

            SqlParameter[] param1 = {
                new SqlParameter("@P_CircularID", result.CircularID)
            };

            res.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<CircularGetModel>>(connString, "Get_CircularByID", r => r.TranslateAsCircularList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", res.ReferenceNumber)
            };

            res.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_CircularByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            return res;
        }
    }
}
