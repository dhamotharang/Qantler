using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.Photographer;
using RulersCourt.Models.Protocol.Media.Photographer;
using RulersCourt.Translators;
using RulersCourt.Translators.Protocol.Media.Photographer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Protocol.Media.Photographer
{
    public class PhotographerClient
    {
        public PhotographerGetModel GetPhotographerByID(string connString, int photographerID, int userID, string lang)
        {
            PhotographerGetModel photographerDetails = new PhotographerGetModel();
            SqlParameter[] param = {
                new SqlParameter("@P_PhotographerID", photographerID) };
            if (photographerID != 0)
            {
                photographerDetails = SqlHelper.ExecuteProcedureReturnData<List<PhotographerGetModel>>(connString, "Get_MediaNewPhotoGrapherRequestByID", r => r.TranslateAsPhotographerList(), param).FirstOrDefault();

                photographerDetails.CommunicationHistory = new PhotographerCommunicationHistoryClient().PhotographerCommunicationHistoryByPhotographerID(connString, photographerID, lang);
            }

            Parallel.Invoke(
              () => photographerDetails.M_OrganizationList = GetM_Organisation(connString, lang),
              () => photographerDetails.M_LookupsList = GetM_Lookups(connString, lang),
              () => photographerDetails.M_ApproverDepartmentList = new GetApproverConfiguration().GetM_ApproverDeparment(connString, lang));

            SqlParameter[] getApproverparam = {
                    new SqlParameter("@P_ReferenceNumber", photographerDetails.ReferenceNumber),
                    new SqlParameter("@P_UserID", userID) };
            photographerDetails.CurrentApprover = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_MediaNewPhotographerRequestByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam);

            SqlParameter[] getAssignparam = {
                    new SqlParameter("@P_ReferenceNumber", photographerDetails.ReferenceNumber),
                    new SqlParameter("@P_Method", 0) };
            photographerDetails.AssigneeId = SqlHelper.ExecuteProcedureReturnData<List<CurrentMediaAssigneeModel>>(connString, "Get_MediaNewPhotographerByAssigneeandMediaId", r => r.TranslateAsCurrentMediaAssigneeList(), getAssignparam);

            SqlParameter[] getMediaUserparam = {
                    new SqlParameter("@P_ReferenceNumber", photographerDetails.ReferenceNumber),
                    new SqlParameter("@P_Method", 1) };

            List<CurrentMediaHeadModel> temp = SqlHelper.ExecuteProcedureReturnData<List<CurrentMediaHeadModel>>(connString, "Get_MediaNewPhotographerByAssigneeandMediaId", r => r.TranslateAsCurrentMediaHeadList(), getMediaUserparam);

            if (temp.Count > 0)
                photographerDetails.MediaHeadUserId = temp.FirstOrDefault().MediaHeadUsedId;

            userID = photographerDetails.CreatedBy.GetValueOrDefault();
            return photographerDetails;
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
                new SqlParameter("@P_Type", "MediaNewPhotoGrapherRequest"), new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        public PhotographerWorkflowModel PostPhotographer(string connString, PhotographerPostModel photographer)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Date", photographer.Date),
                new SqlParameter("@P_SourceOU", photographer.SourceOU),
                new SqlParameter("@P_SourceName", photographer.SourceName),
                new SqlParameter("@P_EventName", photographer.EventName),
                new SqlParameter("@P_EventDate", photographer.EventDate),
                new SqlParameter("@P_Location", photographer.Location),
                new SqlParameter("@P_CreatedBy", photographer.CreatedBy),
                new SqlParameter("@P_ApproverDepartmentID", photographer.ApproverDepartmentID),
                new SqlParameter("@P_CreatedDateTime", photographer.CreatedDateTime),
                new SqlParameter("@P_Action", photographer.Action),
                new SqlParameter("@P_Comment", photographer.Comments)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<PhotographerWorkflowModel>(connString, "Save_MediaNewPhotoGRapherRequest", r => r.TranslateAsPhotographerSaveResponseList(), param);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "MediaNewPhotoGrapherRequest"),
                new SqlParameter("@P_Action", photographer.Action),
                new SqlParameter("@P_ApproverId", photographer.ApproverID) };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.ApproverID = photographer.ApproverID;
            result.ApproverDepartmentID = photographer.ApproverDepartmentID;
            result.FromID = photographer.CreatedBy ?? default(int);
            result.Action = photographer.Action;

            SqlParameter[] parama = { new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber), new SqlParameter("@P_UserID", photographer.CreatedBy) };
            result.MediaHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_MediaApprovers", r => r.TranslateAsUserList(), parama);

            return result;
        }

        public PhotographerWorkflowModel PutPhotographer(string connString, PhotographerPutModel photographer)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_PhotographerID", photographer.PhotographerID),
                new SqlParameter("@P_Date", photographer.Date),
                new SqlParameter("@P_SourceOU", photographer.SourceOU),
                new SqlParameter("@P_SourceName", photographer.SourceName),
                new SqlParameter("@P_EventName", photographer.EventName),
                new SqlParameter("@P_EventDate", photographer.EventDate),
                new SqlParameter("@P_Location", photographer.Location),
                new SqlParameter("@P_UpdatedBy", photographer.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", photographer.UpdatedDateTime),
                new SqlParameter("@P_Action", photographer.Action),
                new SqlParameter("@P_ApproverDepartmentID", photographer.ApproverDepartmentID),
                new SqlParameter("@P_Comment", photographer.Comments)
            };
            var result = SqlHelper.ExecuteProcedureReturnData<PhotographerWorkflowModel>(connString, "Save_MediaNewPhotoGRapherRequest", r => r.TranslateAsPhotographerSaveResponseList(), param);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "MediaNewPhotoGrapherRequest"),
                new SqlParameter("@P_Action", photographer.Action),
                new SqlParameter("@P_ApproverId", photographer.ApproverID)
            };
            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.ApproverID = photographer.ApproverID;
            result.ApproverDepartmentID = photographer.ApproverDepartmentID;
            result.FromID = photographer.UpdatedBy ?? default(int);
            result.Action = photographer.Action;

            SqlParameter[] getAssignparam = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber) };

            if (result.Action == "AssignToMe")
            {
                result.AssigneeID = photographer.UpdatedBy;
            }
            else
            {
                result.AssigneeID = photographer.AssigneeID;
            }

            SqlParameter[] parama = { new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber) };
            result.MediaHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_MediaApprovers", r => r.TranslateAsUserList(), parama);
            SqlParameter[] param1 = {
                new SqlParameter("@P_PhotographerID", result.PhotoGrapherID) };

            result.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<PhotographerGetModel>>(connString, "Get_MediaNewPhotoGrapherRequestByID", r => r.TranslateAsPhotographerList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber) };

            result.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_MediaNewPhotographerRequestByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            return result;
        }

        public PhotographerPutModel GetPatchPhotographerByID(string connString, int photographerID)
        {
            PhotographerPutModel photographerDetails = new PhotographerPutModel();
            SqlParameter[] param = {
                new SqlParameter("@P_PhotographerID", photographerID) };
            if (photographerID != 0)
            {
                photographerDetails = SqlHelper.ExecuteProcedureReturnData<List<PhotographerPutModel>>(connString, "Get_MediaNewPhotoGrapherRequestByID", r => r.TranslateAsPutPhotographerList(), param).FirstOrDefault();
            }

            return photographerDetails;
        }

        public string DeletePhotoGraph(string connString, int id)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_PhotoGrapherID", id)
             };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Delete_MediaNewPhotographerRequestByID", param);
        }

        public int SaveCommunicationChat(string connString, PhotographerCommunicationHistory chat)
        {
            SqlParameter[] param =
                    { new SqlParameter("@P_CommunicationID", chat.CommunicationID),
                    new SqlParameter("@P_PhotographerID", chat.PhotographerID),
                    new SqlParameter("@P_Message", chat.Message),
                    new SqlParameter("@P_ParentCommunicationID", chat.ParentCommunicationID),
                    new SqlParameter("@P_Action", chat.Action),
                    new SqlParameter("@P_CreatedBy", chat.CreatedBy),
                    new SqlParameter("@P_CreatedDateTime", chat.CreatedDateTime)
                    };
            return int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Save_MediaNewPhotographerRequestCommunicationHistory", param));
        }

        internal PhotographerWorkflowModel PatchPhotographer(string connString, int id, JsonPatchDocument<PhotographerPutModel> value)
        {
            var result = GetPatchPhotographerByID(connString, id);
            value.ApplyTo(result);
            if ((result.Action == "Approve") || result.Action == "Reject" || result.Action == "Returnforinfo")
                result.ApproverID = result.UpdatedBy;
            var res = PutPhotographer(connString, result);
            if (result.Action == "Escalate" || result.Action == "Redirect")
            {
                res.ApproverID = result.ApproverID;
            }

            SqlParameter[] param = { new SqlParameter("@P_ReferenceNumber", res.ReferenceNumber) };
            res.AssigneeID = result.AssigneeID;
            res.MediaHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_MediaApprovers", r => r.TranslateAsUserList(), param);

            SqlParameter[] param1 = {
                new SqlParameter("@P_PhotographerID", result.PhotographerID) };

            res.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<PhotographerGetModel>>(connString, "Get_MediaNewPhotoGrapherRequestByID", r => r.TranslateAsPhotographerList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", res.ReferenceNumber) };

            res.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_MediaNewPhotographerRequestByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            return res;
        }
    }
}
