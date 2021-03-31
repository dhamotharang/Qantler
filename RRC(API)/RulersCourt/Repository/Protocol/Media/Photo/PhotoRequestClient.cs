using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.Photo;
using RulersCourt.Models.Protocol.Media.Photo;
using RulersCourt.Translators;
using RulersCourt.Translators.Protocol.Media.PhotoRequestTranslators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Protocol.Media.PhotoRequest
{
    public class PhotoRequestClient
    {
        public PhotoRequestWorkflowModel PostPhoto(string connString, PhotoPostModel photoRequest)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_Date", photoRequest.Date),
                new SqlParameter("@P_SourceOU", photoRequest.SourceOU),
                new SqlParameter("@P_SourceName", photoRequest.SourceName),
                new SqlParameter("@P_Location", photoRequest.Location),
                new SqlParameter("@P_EventName", photoRequest.EventName),
                new SqlParameter("@P_EventDate", photoRequest.EventDate),
                new SqlParameter("@P_PhotoDescription", photoRequest.PhotoDescription),
                new SqlParameter("@P_Action", photoRequest.Action),
                new SqlParameter("@P_Comments", photoRequest.Comments),
                new SqlParameter("@P_ApproverDepartmentID", photoRequest.ApproverDepartmentID),
                new SqlParameter("@P_CreatedBy", photoRequest.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", photoRequest.CreatedDateTime)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<PhotoRequestWorkflowModel>(connString, "Save_PhotoRequest", r => r.TranslateAsPhotoRequestWorkSaveResponseList(), param);

            if (photoRequest.Attachments != null)                new MediaPhotoRequestAttachmentClient().PostPhotoAttachments(connString, "MediaPhotoRequest", photoRequest.Attachments, result.PhotoID);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "MediaPhotoRequest"),
                new SqlParameter("@P_Action", photoRequest.Action),
                new SqlParameter("@P_ApproverId", photoRequest.ApproverID) };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.ApproverID = photoRequest.ApproverID;
            result.ApproverDepartmentID = photoRequest.ApproverDepartmentID;
            result.FromID = photoRequest.CreatedBy ?? default(int);
            result.Action = photoRequest.Action;

            SqlParameter[] parama = { new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber), new SqlParameter("@P_UserID", photoRequest.CreatedBy) };

            result.MediaHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_MediaApprovers", r => r.TranslateAsUserList(), parama);

            return result;
        }

        public PhotoRequestWorkflowModel PutPhoto(string connString, PhotoPutModel photo)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_PhotoID", photo.PhotoID),
                new SqlParameter("@P_Date", photo.Date),
                new SqlParameter("@P_SourceOU", photo.SourceOU),
                new SqlParameter("@P_SourceName", photo.SourceName),
                new SqlParameter("@P_Location", photo.Location),
                new SqlParameter("@P_EventName", photo.EventName),
                new SqlParameter("@P_EventDate", photo.EventDate),
                new SqlParameter("@P_PhotoDescription", photo.PhotoDescription),
                new SqlParameter("@P_UpdatedBy", photo.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", photo.UpdatedDateTime),
                new SqlParameter("@P_ApproverDepartmentID", photo.ApproverDepartmentID),
                new SqlParameter("@P_Action", photo.Action),
                new SqlParameter("@P_Comments", photo.Comments)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<PhotoRequestWorkflowModel>(connString, "Save_PhotoRequest", r => r.TranslateAsPhotoRequestWorkSaveResponseList(), param);

            if (photo.Attachments != null)
                new MediaPhotoRequestAttachmentClient().PostPhotoAttachments(connString, "MediaPhotoRequest", photo.Attachments, result.PhotoID);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "MediaPhotoRequest"),
                new SqlParameter("@P_Action", photo.Action),
                new SqlParameter("@P_ApproverId", photo.ApproverID)
            };
            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));

            result.ApproverID = photo.ApproverID;
            result.ApproverDepartmentID = photo.ApproverDepartmentID;
            result.FromID = photo.UpdatedBy ?? default(int);
            result.Action = photo.Action;

            SqlParameter[] parama = { new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber) };

            result.MediaHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_MediaApprovers", r => r.TranslateAsUserList(), parama);

            SqlParameter[] getAssignparam = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber) };
            result.AssigneeID = photo.AssigneeID;

            SqlParameter[] param1 = {
                new SqlParameter("@P_PhotoID", result.PhotoID) };

            result.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<PhotoGetModel>>(connString, "Get_MediaPhotoRequestByID", r => r.TranslatePhotoGetModelList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber) };

            result.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_MediaPhotoRequestByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            return result;
        }

        public PhotoGetModel GetPhotoID(string connString, int photoID, int userID, string lang)
        {
            PhotoGetModel photoGetDetails = new PhotoGetModel();

            SqlParameter[] param = {
               new SqlParameter("@P_PhotoID", photoID) };

            if (photoID != 0)
            {
                photoGetDetails = SqlHelper.ExecuteProcedureReturnData<List<PhotoGetModel>>(connString, "Get_MediaPhotoRequestByID", r => r.TranslatePhotoGetModelList(), param).FirstOrDefault();
                photoGetDetails.Attachments = new MediaPhotoRequestAttachmentClient().GetPhotoAttachmentById(connString, photoGetDetails.PhotoID, "MediaPhotoRequest");
                photoGetDetails.CommunicationHistory = new PhotoRequestHistoryLogClient().PhotoRequestHistoryLogByPhotoID(connString, photoID, lang);
            }

            Parallel.Invoke(
            () => photoGetDetails.M_OrganizationList = GetM_Organisation(connString, lang),
            () => photoGetDetails.M_LookupsList = GetM_Lookups(connString, lang),
            () => photoGetDetails.M_ApproverDepartmentList = new GetApproverConfiguration().GetM_ApproverDeparment(connString, lang));

            SqlParameter[] getApproverparam = {
                      new SqlParameter("@P_ReferenceNumber", photoGetDetails.ReferenceNumber),
                      new SqlParameter("@P_UserId", userID) };
            photoGetDetails.CurrentApprover = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_MediaPhotoRequestByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam);

            SqlParameter[] getAssignparam = {
                      new SqlParameter("@P_ReferenceNumber", photoGetDetails.ReferenceNumber),
                      new SqlParameter("@P_Method", 0) };
            photoGetDetails.AssigneeID = SqlHelper.ExecuteProcedureReturnData<List<CurrentMediaAssigneeModel>>(connString, "Get_MediaPhotoRequestByAssigneeandMediaId", r => r.TranslateAsCurrentMediaAssigneeList(), getAssignparam);

            SqlParameter[] getMediaUserparam = {
                      new SqlParameter("@P_ReferenceNumber", photoGetDetails.ReferenceNumber),
                      new SqlParameter("@P_Method", 1) };
            photoGetDetails.MediaHeadUsedId = SqlHelper.ExecuteProcedureReturnData<List<CurrentMediaHeadModel>>(connString, "Get_MediaPhotoRequestByAssigneeandMediaId", r => r.TranslateAsCurrentMediaHeadList(), getMediaUserparam);
            userID = photoGetDetails.CreatedBy.GetValueOrDefault();

            return photoGetDetails;
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
                new SqlParameter("@P_Type", "MediaPhotoRequest"), new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        public string DeletePhoto(string connString, int id)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_PhotoID", id)
             };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Delete_PhotoRequestById", param);
        }

        public PhotoPutModel GetPatchPhotoByID(string connString, int photoID)
        {
            PhotoPutModel photoPutDetails = new PhotoPutModel();

            SqlParameter[] param = {
                new SqlParameter("@P_PhotoID", photoID) };

            if (photoID != 0)
            {
                photoPutDetails = SqlHelper.ExecuteProcedureReturnData<List<PhotoPutModel>>(connString, "Get_MediaPhotoRequestByID", r => r.TranslateAsPutPhotoRequestList(), param).FirstOrDefault();
                photoPutDetails.Attachments = new MediaPhotoRequestAttachmentClient().GetPhotoAttachmentById(connString, photoPutDetails.PhotoID, "MediaPhotoRequest");
            }

            return photoPutDetails;
        }

        public int SaveCommunicationChat(string connString, PhotoCommunicationHistory chat)
        {
            SqlParameter[] param =
                    { new SqlParameter("@P_CommunicationID", chat.CommunicationID),
                    new SqlParameter("@P_PhotoID", chat.PhotoID),
                    new SqlParameter("@P_Message", chat.Message),
                    new SqlParameter("@P_ParentCommunicationID", chat.ParentCommunicationID),
                    new SqlParameter("@P_CreatedBy", chat.CreatedBy),
                    new SqlParameter("@P_CreatedDateTime", chat.CreatedDateTime)
                    };
            return int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Save_PhotoCommunicationHistory", param));
        }

        internal PhotoRequestWorkflowModel PatchPhoto(string connString, int id, JsonPatchDocument<PhotoPutModel> value)
        {
            var result = GetPatchPhotoByID(connString, id);

            value.ApplyTo(result);

            if ((result.Action == "Approve") || result.Action == "Reject" || result.Action == "Returnforinfo")                result.ApproverID = result.UpdatedBy;

            var res = PutPhoto(connString, result);

            if (result.Action == "Escalate" || result.Action == "Redirect" || result.Action == "Approve")
            {
                res.ApproverID = result.ApproverID;
            }

            SqlParameter[] param = { new SqlParameter("@P_ReferenceNumber", res.ReferenceNumber) };
            res.MediaHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_MediaApprovers", r => r.TranslateAsUserList(), param);
            SqlParameter[] param1 = {
                new SqlParameter("@P_PhotoID", result.PhotoID) };

            res.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<PhotoGetModel>>(connString, "Get_MediaPhotoRequestByID", r => r.TranslatePhotoGetModelList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", res.ReferenceNumber) };

            res.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_MediaPhotoRequestByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            return res;
        }
    }
}
