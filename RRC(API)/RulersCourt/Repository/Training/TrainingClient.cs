using Microsoft.AspNetCore.JsonPatch;
using RulersCourt.Models;
using RulersCourt.Models.HR.Training;
using RulersCourt.Models.Training;
using RulersCourt.Translators;
using RulersCourt.Translators.HR.Training;
using RulersCourt.Translators.Training;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Training
{
    public class TrainingClient
    {
        public TrainingGetModel GetTrainingByID(string connString, int trainingID, int userID, string lang)
        {
            TrainingGetModel trainingDetails = new TrainingGetModel();
            SqlParameter[] param = {
                new SqlParameter("@P_TrainingID", trainingID) };

            if (trainingID != 0)
            {
                trainingDetails = SqlHelper.ExecuteProcedureReturnData<List<TrainingGetModel>>(connString, "Get_TrainingByID", r => r.TranslateAsTrainingList(), param).FirstOrDefault();
                trainingDetails.CommunicationHistory = new TrainingCommunicationHistoryClient().TrainingCommunicationHistoryByLeaveID(connString, trainingID, lang);
            }

            SqlParameter[] getAssignparam = {
                    new SqlParameter("@P_ReferenceNumber", trainingDetails.ReferenceNumber) };

            trainingDetails.AssigneeID = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_TrainingAssigneeID", getAssignparam));

            Parallel.Invoke(
              () => trainingDetails.M_OrganizationList = GetM_Organisation(connString, lang),
              () => trainingDetails.M_LookupsList = GetM_Lookups(connString, lang),
              () => trainingDetails.M_ApproverDepartmentList = new GetApproverConfiguration().GetM_ApproverDeparment(connString, lang),
              () => trainingDetails.Attachments = GetAttachments(connString, trainingID, "Training"));

            SqlParameter[] getApproverparam = {
                    new SqlParameter("@P_ReferenceNumber", trainingDetails.ReferenceNumber),
                    new SqlParameter("@P_UserID", userID) };

            trainingDetails.CurrentApprover = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_TrainingByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam);

            userID = trainingDetails.CreatedBy.GetValueOrDefault();

            return trainingDetails;
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
                new SqlParameter("@P_Type", "Training"),
                new SqlParameter("@P_Language", lang)
            };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LookupsModel>>(connString, "Get_M_Lookups", r => r.TranslateAsM_LookupsList(), param);
        }

        public TrainingWorkflowModel SaveTraining(string connString, TrainingPostModel training)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_SourceOU", training.SourceOU),
                new SqlParameter("@P_SourceName", training.SourceName),
                new SqlParameter("@P_TrainingFor", training.TrainingFor),
                new SqlParameter("@P_TraineeName", training.TraineeName),
                new SqlParameter("@P_TrainingName", training.TrainingName),
                new SqlParameter("@P_ApproverDepartmentID", training.ApproverDepartmentID),
                new SqlParameter("@P_StartDate", training.StartDate),
                new SqlParameter("@P_EndDate", training.EndDate),
                new SqlParameter("@P_Action", training.Action),
                new SqlParameter("@P_Comment", training.Comments),
                new SqlParameter("@P_CreatedBy", training.CreatedBy),
                new SqlParameter("@P_CreatedDateTime", training.CreatedDateTime)
           };
            var result = SqlHelper.ExecuteProcedureReturnData<TrainingWorkflowModel>(connString, "Save_Training", r => r.TranslateAsTrainingSaveResponseList(), param);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "Training"),
                new SqlParameter("@P_Action", training.Action)
            };
            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));
            result.ApproverID = training.ApproverID;
            result.CreatorID = training.CreatedBy;
            result.FromID = training.CreatedBy;
            result.Action = training.Action;

            SqlParameter[] parama = {
                new SqlParameter("@P_Department", 9),
                new SqlParameter("@P_GetHead", 1)
            };
            result.HRHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_UserByUnits", r => r.TranslateAsUserList(), parama);

            return result;
        }

        public TrainingWorkflowModel PutTraining(string connString, TrainingPutModel training)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_TrainingID", training.TrainingID),
                new SqlParameter("@P_SourceOU", training.SourceOU),
                new SqlParameter("@P_SourceName", training.SourceName),
                new SqlParameter("@P_TrainingFor", training.TrainingFor),
                new SqlParameter("@P_TraineeName", training.TraineeName),
                new SqlParameter("@P_TrainingName", training.TrainingName),
                new SqlParameter("@P_ApproverDepartmentID", training.ApproverDepartmentID),
                new SqlParameter("@P_StartDate", training.StartDate),
                new SqlParameter("@P_EndDate", training.EndDate),
                new SqlParameter("@P_Action", training.Action),
                new SqlParameter("@P_Comment", training.Comments),
                new SqlParameter("@P_DeleteFlag", training.DeleteFlag),
                new SqlParameter("@P_UpdatedBy", training.UpdatedBy),
                new SqlParameter("@P_UpdatedDateTime", training.UpdatedDateTime)
            };

            var result = SqlHelper.ExecuteProcedureReturnData<TrainingWorkflowModel>(connString, "Save_Training", r => r.TranslateAsTrainingSaveResponseList(), param);

            Console.WriteLine(result);

            SqlParameter[] paramStatus = {
                new SqlParameter("@P_Service", "Training"),
                new SqlParameter("@P_Action", training.Action)
            };

            result.Status = int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Get_StatusByID", paramStatus));
            result.ApproverID = training.ApproverID;
            result.FromID = training.UpdatedBy ?? default(int);
            result.Action = training.Action;
            result.AssigneeID = training.AssigneeID;

            SqlParameter[] parama = {
                new SqlParameter("@P_Department", 9),
                new SqlParameter("@P_GetHead", 1)
            };

            result.HRHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_UserByUnits", r => r.TranslateAsUserList(), parama);

            SqlParameter[] param1 = {
                new SqlParameter("@P_TrainingID", result.TrainingID) };

            result.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<TrainingGetModel>>(connString, "Get_TrainingByID", r => r.TranslateAsTrainingList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", result.ReferenceNumber) };

            result.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_TrainingByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;

            return result;
        }

        public string DeleteTraining(string connString, int id)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_TraniningID", id) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Delete_TrainingByID", param);
        }

        public TrainingWorkflowModel UpdateTrainingCertificate(string connString, int id, List<TrainingAttachmentModel> attachment, int userID, string lang)
        {
            var res = GetTrainingByID(connString, id, userID, lang);
            TrainingWorkflowModel result = new TrainingWorkflowModel();
            result.ReferenceNumber = res.ReferenceNumber;
            result.Action = "Save";
            result.TraineeID = int.Parse(res.TraineeName);
            SqlParameter[] param = { new SqlParameter("@P_Department", 9), new SqlParameter("@P_GetHead", 1) };
            result.HRHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_UserByUnits", r => r.TranslateAsUserList(), param);
            new TrainingClient().SaveAttachments(connString, "Training", attachment, id, userID);
            SqlParameter[] param1 = {
                    new SqlParameter("@P_TrainingID", id),
                    new SqlParameter("@P_Message", null),
                    new SqlParameter("@P_CreatedBy", userID),
                    new SqlParameter("@P_Type", 1)
            };
            SqlHelper.ExecuteProcedureReturnString(connString, "Save_TrainingCommunicationHistory", param1);
            return result;
        }

        public TrainingPutModel GetPatchTrainingByID(string connString, int trainingID)
        {
            TrainingPutModel letterDetails = new TrainingPutModel();

            SqlParameter[] param = {
                new SqlParameter("@P_TrainingID", trainingID) };
            if (trainingID != 0)
            {
                letterDetails = SqlHelper.ExecuteProcedureReturnData<List<TrainingPutModel>>(connString, "Get_TrainingByID", r => r.TranslateAsPutTrainingList(), param).FirstOrDefault();
            }

            return letterDetails;
        }

        public int SaveCommunicationChat(string connString, TrainingCommunicationHistory chat)
        {
            SqlParameter[] param = {
                    new SqlParameter("@P_CommunicationID", chat.CommunicationID),
                    new SqlParameter("@P_TrainingID", chat.TrainingID),
                    new SqlParameter("@P_Message", chat.Message),
                    new SqlParameter("@P_ParentCommunicationID", chat.ParentCommunicationID),
                    new SqlParameter("@P_CreatedBy", chat.CreatedBy),
                    new SqlParameter("@P_CreatedDateTime", chat.CreatedDateTime)
            };
            return int.Parse(SqlHelper.ExecuteProcedureReturnString(connString, "Save_TrainingCommunicationHistory", param));
        }

        public string SaveAttachments(string connString, string type, List<TrainingAttachmentModel> datas, int? serviceID, int userID)
        {
            string result = string.Empty;
            if (datas.Count != 0)
            {
                SqlParameter[] param = {
                    new SqlParameter("@P_Type", "Delete"),
                    new SqlParameter("@P_UpdatedBy", userID),
                    new SqlParameter("@P_ServiceType", type),
                    new SqlParameter("@P_ServiceID", serviceID),
                    new SqlParameter("@P_AttachmentGuid", string.Join(",", from item in datas select item.AttachmentGuid)),
                    new SqlParameter("@P_AttachmentsName", string.Join(",", from item in datas select item.AttachmentsName))
                };

                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_Attachments", param);
            }
            else
            {
                SqlParameter[] parama1 = {
                    new SqlParameter("@P_Type", "Delete"),
                    new SqlParameter("@P_UpdatedBy", userID),
                    new SqlParameter("@P_ServiceType", type),
                    new SqlParameter("@P_ServiceID", serviceID)
                };
                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_Attachments", parama1);
            }

            foreach (TrainingAttachmentModel data in datas)
            {
                SqlParameter[] param = {
                    new SqlParameter("@P_Type", type),
                    new SqlParameter("@P_CreatedBy", data.CreatedBy),
                    new SqlParameter("@P_ServiceID", serviceID),
                    new SqlParameter("@P_AttachmentGuid", data.AttachmentGuid),
                    new SqlParameter("@P_AttachmentsName", data.AttachmentsName)
                 };

                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_Attachments", param);
            }

            return result;
        }

        public List<TrainingAttachmentModel> GetAttachments(string connString, int? serviceID, string type)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_ServiceID", serviceID),
                new SqlParameter("@Type", type)
            };

            return SqlHelper.ExecuteProcedureReturnData<List<TrainingAttachmentModel>>(connString, "Get_AttachmentByID", r => r.TranslateAsTrainingAttachmentList(), param);
        }

        internal TrainingWorkflowModel PatchTraining(string connString, int id, JsonPatchDocument<TrainingPutModel> value)
        {
            var result = GetPatchTrainingByID(connString, id);

            value.ApplyTo(result);
            var res = PutTraining(connString, result);
            res.TraineeFor = result.TrainingFor;

            if (result.Action == "Escalate")
            {
                res.ApproverID = result.ApproverID;
            }

            res.TraineeID = int.Parse(result.TraineeName);
            res.AssigneeID = result.AssigneeID;
            SqlParameter[] param = { new SqlParameter("@P_Department", 9), new SqlParameter("@P_GetHead", 1) };
            res.HRHeadUsedID = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(connString, "Get_UserByUnits", r => r.TranslateAsUserList(), param);
            SqlParameter[] param1 = {
                new SqlParameter("@P_TrainingID", result.TrainingID) };

            SqlParameter[] param2 = {
                new SqlParameter("@P_TrainingID", result.TrainingID) };

            res.CurrentStatus = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnData<List<TrainingGetModel>>(connString, "Get_TrainingByID", r => r.TranslateAsTrainingList(), param1).FirstOrDefault().Status);

            SqlParameter[] getApproverparam1 = {
                    new SqlParameter("@P_ReferenceNumber", res.ReferenceNumber) };

            res.PreviousApproverID = SqlHelper.ExecuteProcedureReturnData<List<CurrentApproverModel>>(connString, "Get_TrainingByApproverId", r => r.TranslateAsCurrentApproverList(), getApproverparam1).FirstOrDefault().ApproverId;
            res.HRManagerUserID = SqlHelper.ExecuteProcedureReturnData<List<TrainingGetModel>>(connString, "Get_TrainingByID", r => r.TranslateAsTrainingList(), param2).FirstOrDefault().HRManagerUserID;
            return res;
        }
    }
}
