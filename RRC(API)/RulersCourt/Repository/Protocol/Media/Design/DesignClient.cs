using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.Design;
using RulersCourt.Models.Protocol.Media.Design;
using RulersCourt.Translators;
using RulersCourt.Translators.Protocol.Media.Design;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Protocol.Media.Design
{
    public class DesignClient
    {
        public DesignResponseModel PostDesign(string connString, DesignPostModel value)
        {
            SqlParameter[] param = { new SqlParameter("@P_SourceOU", value.SourceOU),
                                     new SqlParameter("@P_Date", value.Date),
                                     new SqlParameter("@P_SourceName", value.SourceName),
                                     new SqlParameter("@P_Title", value.Title),
                                     new SqlParameter("@P_InitiativeProjectActivity", value.Project),
                                     new SqlParameter("@P_DeliverableDate", value.DateofDeliverable),
                                     new SqlParameter("@P_DiwansRole", value.DiwansRole),
                                     new SqlParameter("@P_OtherParties", value.OtherParties),
                                     new SqlParameter("@P_TargetGroup", value.TargetGroups),
                                     new SqlParameter("@P_TypeOfDesignRequired", value.TypeofDesignRequired),
                                     new SqlParameter("@P_Languages", value.Languages),
                                     new SqlParameter("@P_GeneralObjective", value.GeneralObjective),
                                     new SqlParameter("@P_MainObjective", value.MainObjective),
                                     new SqlParameter("@P_StrategicObjective", value.StrategicObjective),
                                     new SqlParameter("@P_CreatedBy", value.CreatedBy),
                                     new SqlParameter("@P_CreatedDateTime", value.CreatedDateTime),
                                     new SqlParameter("@P_Action", value.Action),
                                     new SqlParameter("@P_ApproverDepartmentID", value.ApproverDepartmentID),
                                     new SqlParameter("@P_Comment", value.Comments)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<DesignResponseModel>(connString, "Save_MediaDesignRequest", r => r.TranslateAsDesignSaveResponseList(), param);

            if (value.Attachments != null)
                new DesignAttachmentClient().PostDesignAttachments(connString, "MediaDesignRequest", value.Attachments, result.DesignID);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "MediaDesignRequest"),
                new SqlParameter("@P_Action", value.Action),
                new SqlParameter("@P_ApproverId", value.ApproverID) };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.ApproverID = value.ApproverID;
            result.ApproverDepartmentID = value.ApproverDepartmentID;
            result.FromID = value.CreatedBy ?? default(int);
            result.Action = value.Action;

            SqlParameter[] parama = { new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber), new SqlParameter("@P_UserID", value.CreatedBy) };

            result.MediaHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_MediaApprovers", r => r.TranslateAsUserList(), parama);

            return result;
        }

        public DesignResponseModel PutDesign(string connString, Models.Design.DesignPutModel value)
        {
            SqlParameter[] param = {
                                     new SqlParameter("@P_DesignID", value.DesignID),
                                     new SqlParameter("@P_SourceOU", value.SourceOU),
                                     new SqlParameter("@P_SourceName", value.SourceName),
                                     new SqlParameter("@P_Date", value.Date),
                                     new SqlParameter("@P_Title", value.Title),
                                     new SqlParameter("@P_InitiativeProjectActivity", value.Project),
                                     new SqlParameter("@P_DeliverableDate", value.DeliverableDate),
                                     new SqlParameter("@P_DiwansRole", value.DiwansRole),
                                     new SqlParameter("@P_OtherParties", value.OtherParties),
                                     new SqlParameter("@P_TargetGroup", value.TargetGroups),
                                     new SqlParameter("@P_TypeOfDesignRequired", value.TypeofDesignRequired),
                                     new SqlParameter("@P_Languages", value.Languages),
                                     new SqlParameter("@P_GeneralObjective", value.GeneralObjective),
                                     new SqlParameter("@P_MainObjective", value.MainObjective),
                                     new SqlParameter("@P_StrategicObjective", value.StrategicObjective),
                                     new SqlParameter("@P_UpdatedBy", value.UpdatedBy),
                                     new SqlParameter("@P_UpdatedDateTime", value.UpdatedDateTime),
                                     new SqlParameter("@P_Action", value.Action),
                                     new SqlParameter("@P_ApproverDepartmentID", value.ApproverDepartmentID),
                                     new SqlParameter("@P_Comment", value.Comments)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<DesignResponseModel>(connString, "Save_MediaDesignRequest", r => r.TranslateAsDesignSaveResponseList(), param);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "MediaDesignRequest"),
                new SqlParameter("@P_Action", value.Action),
                new SqlParameter("@P_ApproverId", value.ApproverID) };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            if (value.Attachments != null)
                new DesignAttachmentClient().PostDesignAttachments(connString, "MediaDesignRequest", value.Attachments, result.DesignID);

            result.ApproverID = value.ApproverID;
            result.ApproverDepartmentID = value.ApproverDepartmentID;
            result.FromID = value.UpdatedBy ?? default(int);
            result.Action = value.Action;

            SqlParameter[] getAssignparam = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber) };
            if (result.Action == "AssignToMe")
            {
                result.AssigneeID = value.UpdatedBy;
            }
            else
            {
                result.AssigneeID = value.AssigneeID;
            }

            SqlParameter[] parama = { new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber) };

            result.MediaHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_MediaApprovers", r => r.TranslateAsUserList(), parama);
            SqlParameter[] param1 = {
                new SqlParameter("@P_DesignID", result.DesignID) };

            result.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<DesignGetModel>>(connString, "Get_MediaDesignRequestById", r => r.TranslateAsDesignList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber) };

            result.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_MediaDesignRequestByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            return result;
        }

        public int SaveCommunicationChat(string connString, DesignCommunicationHistory chat)
        {
            SqlParameter[] param =
                    { new SqlParameter("@P_CommunicationID", chat.CommunicationID),
                    new SqlParameter("@P_DesignID", chat.DesignID),
                    new SqlParameter("@P_Message", chat.Message),
                    new SqlParameter("@P_ParentCommunicationID", chat.ParentCommunicationID),
                    new SqlParameter("@P_Action", chat.Action),
                    new SqlParameter("@P_CreatedBy", chat.CreatedBy),
                    new SqlParameter("@P_CreatedDateTime", chat.CreatedDateTime)
                    };
            return int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Save_MediaDesignRequestCommunicationHistory", param));
        }

        public string DeleteDesign(string connString, int id)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_DesignID", id)
             };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Delete_MediaDesignRequestByID", param);
        }

        public DesignGetModel GetDesignRequestByID(string connString, int id, int userID, string lang)
        {
            DesignGetModel design = new DesignGetModel();

            SqlParameter[] param = {
                new SqlParameter("@P_DesignID", id) };
            if (id != 0)
            {
                design = SqlHelper.ExecuteProcedureReturnData<List<DesignGetModel>>(connString, "Get_MediaDesignRequestById", r => r.TranslateAsDesignList(), param).FirstOrDefault();

                design.Attachments = new DesignAttachmentClient().GetDesignAttachmentById(connString, design.DesignId, "MediaDesignRequest");

                design.CommunicationHistory = new DesignHistoryLogClient().DesignHistoryLogByCircularID(connString, id, lang);
            }

            Parallel.Invoke(
              () => design.M_OrganizationList = GetM_Organisation(connString, lang),
              () => design.M_LookupsList = GetM_Lookups(connString, lang),
              () => design.M_ApproverDepartmentList = new GetApproverConfiguration().GetM_ApproverDeparment(connString, lang));

            SqlParameter[] getApproverparam = {
                    new SqlParameter("@P_ReferenceNumber", design.ReferenceNumber),
                    new SqlParameter("@P_UserID", userID) };
            design.CurrentApprover = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_MediaDesignRequestByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam);

            SqlParameter[] getAssignparam = {
                    new SqlParameter("@P_ReferenceNumber", design.ReferenceNumber),
                    new SqlParameter("@P_Method", 0) };
            design.AssigneeID = SqlHelper.ExecuteProcedureReturnData<List<CurrentMediaAssigneeModel>>(connString, "Get_MediaDesignRequesyByAssigneeandMediaId", r => r.TranslateAsCurrentMediaAssigneeList(), getAssignparam);

            SqlParameter[] getMediaUserparam = {
                    new SqlParameter("@P_ReferenceNumber", design.ReferenceNumber),
                    new SqlParameter("@P_Method", 1) };

            design.MediaHeadUsedId = SqlHelper.ExecuteProcedureReturnData<List<CurrentMediaHeadModel>>(connString, "Get_MediaDesignRequesyByAssigneeandMediaId", r => r.TranslateAsCurrentMediaHeadList(), getMediaUserparam);

            userID = design.CreatedBy.GetValueOrDefault();

            return design;
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
                new SqlParameter("@P_Type", "MediaDesignRequest"), new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        public DesignPutModel GetPatchDesignByID(string connString, int designID)
        {
            DesignPutModel designDetails = new DesignPutModel();

            SqlParameter[] param = {
                new SqlParameter("@P_DesignID", designID) };

            if (designID != 0)
            {
                designDetails = SqlHelper.ExecuteProcedureReturnData<List<DesignPutModel>>(connString, "Get_MediaDesignRequestById", r => r.TranslateAsDesignPatchList(), param).FirstOrDefault();

                designDetails.Attachments = new DesignAttachmentClient().GetDesignAttachmentById(connString, designDetails.DesignID, "MediaDesignRequest");
            }

            return designDetails;
        }

        internal DesignResponseModel PatchDesign(string connString, int id, JsonPatchDocument<DesignPutModel> value)
        {
            var result = GetPatchDesignByID(connString, id);

            value.ApplyTo(result);
            if ((result.Action == "Approve") || result.Action == "Reject" || result.Action == "Returnforinfo")
                result.ApproverID = result.UpdatedBy;

            var res = PutDesign(connString, result);
            if (result.Action == "Escalate" || result.Action == "Redirect")
            {
                res.ApproverID = result.ApproverID;
            }

            SqlParameter[] param = { new SqlParameter("@P_ReferenceNumber", res.ReferenceNumber) };
            res.AssigneeID = result.AssigneeID;
            res.MediaHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_MediaApprovers", r => r.TranslateAsUserList(), param);

            SqlParameter[] param1 = {
                new SqlParameter("@P_DesignID", result.DesignID) };

            res.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<DesignGetModel>>(connString, "Get_MediaDesignRequestById", r => r.TranslateAsDesignList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber) };

            res.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_MediaDesignRequestByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            return res;
        }
    }
}
