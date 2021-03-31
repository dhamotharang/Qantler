using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.Campaign;
using RulersCourt.Models.Protocol.Media.Campaign;
using RulersCourt.Translators;
using RulersCourt.Translators.Protocol.Media.Campaign;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Protocol.Media.Campaign
{
    public class CampaignClient
    {
        public CampaignGetModel GetCampaignRequestByID(string connString, int id, int userID, string lang)
        {
            CampaignGetModel design = new CampaignGetModel();

            SqlParameter[] param = {
                new SqlParameter("@P_CampaignID", id) };
            if (id != 0)
            {
                design = SqlHelper.ExecuteProcedureReturnData<List<CampaignGetModel>>(connString, "Get_MediaNewCampaignRequestById", r => r.TranslateAsCampaignList(), param).FirstOrDefault();

                design.Attachments = new CampaignAttachmentClient().GetCampaignAttachmentById(connString, design.CampaignID, "MediaNewCampaignRequest");

                design.CommunicationHistory = new CampaignHistoryLogClient().CampaignHistoryLogID(connString, id, lang);
            }

            Parallel.Invoke(
              () => design.M_OrganizationList = GetM_Organisation(connString, lang),
              () => design.M_LookupsList = GetM_Lookups(connString, lang),
              () => design.M_ApproverDepartmentList = new GetApproverConfiguration().GetM_ApproverDeparment(connString, lang));

            SqlParameter[] getApproverparam = {
                    new SqlParameter("@P_ReferenceNumber", design.ReferenceNumber),
                    new SqlParameter("@P_UserID", userID) };
            design.CurrentApprover = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_MediaNewCampaignRequestByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam);

            SqlParameter[] getAssignparam = {
                    new SqlParameter("@P_ReferenceNumber", design.ReferenceNumber),
                    new SqlParameter("@P_Method", 0) };
            design.AssigneeId = SqlHelper.ExecuteProcedureReturnData<List<CurrentMediaAssigneeModel>>(connString, "Get_MediaNewCampaignRequesyByAssigneeandMediaId", r => r.TranslateAsCurrentMediaAssigneeList(), getAssignparam);

            SqlParameter[] getMediaUserparam = {
                    new SqlParameter("@P_ReferenceNumber", design.ReferenceNumber),
                    new SqlParameter("@P_Method", 1) };

            design.MediaHeadUsedId = SqlHelper.ExecuteProcedureReturnData<List<CurrentMediaHeadModel>>(connString, "Get_MediaNewCampaignRequesyByAssigneeandMediaId", r => r.TranslateAsCurrentMediaHeadList(), getMediaUserparam);

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
                new SqlParameter("@P_Type", "MediaNewCampaignRequest"), new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        public string DeleteCampaign(string connString, int id)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_CampaignID", id)
             };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Delete_MediaNewCampaignRequestByID", param);
        }

        public CampaignResponseModel PostCampaign(string connString, CampaignPostModel value)
        {
            SqlParameter[] param = { new SqlParameter("@P_SourceOU", value.SourceOU),
                                     new SqlParameter("@P_SourceName", value.SourceName),
                                     new SqlParameter("@P_Date", value.Date),
                                     new SqlParameter("@P_CampaignPeriod", value.CampaignPeriod),
                                     new SqlParameter("@P_InitiativeProjectActivity", value.InitiativeProjectActivity),
                                     new SqlParameter("@P_CampaignStartDate", value.CampaignStartDate),
                                     new SqlParameter("@P_DiwansRole", value.DiwansRole),
                                     new SqlParameter("@P_OtherEntities", value.OtherEntities),
                                     new SqlParameter("@P_TargetGroup", value.TargetGroup),
                                     new SqlParameter("@P_TargetAudience", value.TargetAudience),
                                     new SqlParameter("@P_Location", value.Location),
                                     new SqlParameter("@P_MediaChannels", value.MediaChannels),
                                     new SqlParameter("@P_Notes", value.Notes),
                                     new SqlParameter("@P_RequestDetails", value.RequestDetails),
                                     new SqlParameter("@P_Languages", value.Languages),
                                     new SqlParameter("@P_GeneralInformation", value.GeneralInformation),
                                     new SqlParameter("@P_MainObjective", value.MainObjective),
                                     new SqlParameter("@P_MainIdea", value.MainIdea),
                                     new SqlParameter("@P_StrategicGoals", value.StrategicGoals),
                                     new SqlParameter("@P_CreatedBy", value.CreatedBy),
                                     new SqlParameter("@P_CreatedDateTime", value.CreatedDateTime),
                                     new SqlParameter("@P_Action", value.Action),
                                     new SqlParameter("@P_ApproverDepartmentID", value.ApproverDepartmentID),
                                     new SqlParameter("@P_Comment", value.Comments)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<CampaignResponseModel>(connString, "Save_MediaNewCampaignRequest", r => r.TranslateAsCampaignSaveResponseList(), param);

            if (value.Attachments != null)
                new CampaignAttachmentClient().PostDesignAttachments(connString, "MediaNewCampaignRequest", value.Attachments, result.CampaignID);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "MediaNewCampaignRequest"),
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

        public CampaignResponseModel PutCampaign(string connString, CampaignPutModel value)
        {
            SqlParameter[] param = {
                                     new SqlParameter("@P_CampaignID", value.CampaignID),
                                     new SqlParameter("@P_SourceOU", value.SourceOU),
                                     new SqlParameter("@P_SourceName", value.SourceName),
                                     new SqlParameter("@P_Date", value.Date),
                                     new SqlParameter("@P_CampaignPeriod", value.CampaignPeriod),
                                     new SqlParameter("@P_InitiativeProjectActivity", value.InitiativeProjectActivity),
                                     new SqlParameter("@P_CampaignStartDate", value.CampaignStartDate),
                                     new SqlParameter("@P_DiwansRole", value.DiwansRole),
                                     new SqlParameter("@P_OtherEntities", value.OtherEntities),
                                     new SqlParameter("@P_TargetGroup", value.TargetGroup),
                                     new SqlParameter("@P_TargetAudience", value.TargetAudience),
                                     new SqlParameter("@P_Location", value.Location),
                                     new SqlParameter("@P_MediaChannels", value.MediaChannels),
                                     new SqlParameter("@P_Notes", value.Notes),
                                     new SqlParameter("@P_RequestDetails", value.RequestDetails),
                                     new SqlParameter("@P_Languages", value.Languages),
                                     new SqlParameter("@P_GeneralInformation", value.GeneralInformation),
                                     new SqlParameter("@P_MainObjective", value.MainObjective),
                                     new SqlParameter("@P_MainIdea", value.MainIdea),
                                     new SqlParameter("@P_StrategicGoals", value.StrategicGoals),
                                     new SqlParameter("@P_UpdatedBy", value.UpdatedBy),
                                     new SqlParameter("@P_UpdatedDateTime", value.UpdatedDateTime),
                                     new SqlParameter("@P_Action", value.Action),
                                     new SqlParameter("@P_ApproverDepartmentID", value.ApproverDepartmentID),
                                     new SqlParameter("@P_Comment", value.Comments)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<CampaignResponseModel>(connString, "Save_MediaNewCampaignRequest", r => r.TranslateAsCampaignSaveResponseList(), param);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "MediaNewCampaignRequest"),
                new SqlParameter("@P_Action", value.Action),
                new SqlParameter("@P_ApproverId", value.ApproverID) };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            if (value.Attachments != null)
                new CampaignAttachmentClient().PostDesignAttachments(connString, "MediaNewCampaignRequest", value.Attachments, result.CampaignID);

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
                new SqlParameter("@P_CampaignID", result.CampaignID) };

            result.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<CampaignGetModel>>(connString, "Get_MediaNewCampaignRequestById", r => r.TranslateAsCampaignList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber) };

            result.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_MediaNewCampaignRequestByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            return result;
        }

        public int SaveCommunicationChat(string connString, CampaignCommunicationHistory chat)
        {
            SqlParameter[] param =
                    { new SqlParameter("@P_CommunicationID", chat.CommunicationID),
                    new SqlParameter("@P_CampaignID", chat.CampaignID),
                    new SqlParameter("@P_Message", chat.Message),
                    new SqlParameter("@P_ParentCommunicationID", chat.ParentCommunicationID),
                    new SqlParameter("@P_Action", chat.Action),
                    new SqlParameter("@P_CreatedBy", chat.CreatedBy),
                    new SqlParameter("@P_CreatedDateTime", chat.CreatedDateTime)
                    };
            return int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Save_MediaNewCampaignRequestCommunicationHistory", param));
        }

        public CampaignPutModel GetPatchDesignByID(string connString, int campaignID)
        {
            CampaignPutModel designDetails = new CampaignPutModel();

            SqlParameter[] param = {
                new SqlParameter("@P_CampaignID", campaignID) };

            if (campaignID != 0)
            {
                designDetails = SqlHelper.ExecuteProcedureReturnData<List<CampaignPutModel>>(connString, "Get_MediaNewCampaignRequestById", r => r.TranslateAsCampaignPatchList(), param).FirstOrDefault();

                designDetails.Attachments = new CampaignAttachmentClient().GetCampaignAttachmentById(connString, designDetails.CampaignID, "MediaNewCampaignRequest");
            }

            return designDetails;
        }

        internal CampaignResponseModel PatchCampaign(string connString, int id, JsonPatchDocument<CampaignPutModel> value)
        {
            var result = GetPatchDesignByID(connString, id);

            value.ApplyTo(result);
            if ((result.Action == "Approve") || result.Action == "Reject" || result.Action == "Returnforinfo")
                result.ApproverID = result.UpdatedBy;

            var res = PutCampaign(connString, result);
            if (result.Action == "Escalate" || result.Action == "Redirect")
            {
                res.ApproverID = result.ApproverID;
            }

            SqlParameter[] param = { new SqlParameter("@P_ReferenceNumber", res.ReferenceNumber) };
            res.AssigneeID = result.AssigneeID;
            res.MediaHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_MediaApprovers", r => r.TranslateAsUserList(), param);

            SqlParameter[] param1 = {
                new SqlParameter("@P_CampaignID", result.CampaignID) };

            res.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<CampaignGetModel>>(connString, "Get_MediaNewCampaignRequestById", r => r.TranslateAsCampaignList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", res.ReferenceNumber) };

            res.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_MediaNewCampaignRequestByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            return res;
        }
    }
}
