using RulersCourt.Models.DutyTask;
using RulersCourt.Models.DutyTasks;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Workflow;

namespace RulersCourt.Repository.DutyTask
{
    public class DutyTaskGetWorkflow : CommonWorkflow
    {
        public Workflow.WorkflowBO GetDutyTaskWorkflow(DutyTaskWorkflowModel responseModel, string conn)
        {
            WorkflowBO bo = new Workflow.WorkflowBO();
            bo.Service = "DutyTask";
            bo.Status = responseModel.Status;
            bo.ReferenceNumber = responseModel.ReferenceNumber;
            bo.ServiceRequestor = GetActor(responseModel.CreatorID, conn);
            string deletgateUser;

            switch (responseModel.Action)
            {
                case "Completed":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.CreatorID, conn) };
                    if (responseModel.CurrentStatus == 31 && responseModel.AssigneeUserId != responseModel.FromID)
                    {
                        bo.From = GetActor(responseModel.AssigneeUserId, conn);
                        bo.DelegateFrom = GetActor(responseModel.FromID, conn);
                    }
                    else
                    {
                        bo.From = GetActor(responseModel.FromID, conn);
                    }

                    bo.WorkflowProcess = Workflow.WorkflowType.DutyTaskCompleteWorkflow;
                    break;

                case "Submit":
                    List<Workflow.Actor> toActors = new List<Workflow.Actor>();
                    toActors.Add(GetActor(responseModel.AssigneeUserId, conn));
                    foreach (DutyTaskResponsibleUsersModel item in responseModel.ResponsibleUserId)
                    {
                        toActors.Add(GetActor(item.TaskResponsibleUsersID, conn));
                    }

                    bo.From = GetActor(responseModel.CreatorID, conn);
                    bo.To = toActors;
                    SqlParameter[] parameters = {
                    new SqlParameter("@P_UserID", responseModel.AssigneeUserId) };
                    deletgateUser = SqlHelper.ExecuteProcedureReturnString(conn, "Get_DelegateUser", parameters);
                    if (deletgateUser != null && deletgateUser != "0")
                        bo.DelegateTo = GetActor(Convert.ToInt32(deletgateUser), conn);

                    bo.WorkflowProcess = Workflow.WorkflowType.DutyTaskSubmissionWorkflow;
                    break;

                case "Close":
                    bo.From = GetActor(responseModel.CreatorID, conn);
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.CreatorID, conn) };
                    bo.WorkflowProcess = Workflow.WorkflowType.DutyTaskCloseWorkflow;
                    break;

                case "Delete":
                    List<Workflow.Actor> toActorss = new List<Workflow.Actor>();
                    toActorss.Add(GetActor(responseModel.AssigneeUserId, conn));
                    foreach (DutyTaskResponsibleUsersModel item in responseModel.ResponsibleUserId)
                    {
                        toActorss.Add(GetActor(item.TaskResponsibleUsersID, conn));
                    }

                    bo.From = GetActor(responseModel.CreatorID, conn);
                    bo.To = toActorss;
                    bo.WorkflowProcess = Workflow.WorkflowType.DutyTaskDeleteWorkflow;
                    break;
            }

            return bo;
        }

        public Workflow.WorkflowBO GetDutyTaskCommunicationWorkflow(DutyTaskCommunicationHistoryModel communicationBoard, string conn)
        {
            DutyTaskGetModel dutyTask = new DutyTaskGetModel();
            dutyTask = new DutyTaskClient().GetDutyTaskByID(conn, communicationBoard.TaskID, communicationBoard.CreatedBy, "EN");
            WorkflowBO bo = new Workflow.WorkflowBO();
            bo.Service = "DutyTask";
            bo.ReferenceNumber = dutyTask.TaskReferenceNumber;
            bo.ServiceRequestor = GetActor(communicationBoard.CreatedBy, conn);
            bo.From = GetActor(communicationBoard.CreatedBy, conn);
            List<Workflow.Actor> toActor = new List<Workflow.Actor>();
            foreach (DutyTaskTaggedUserIDModel item in communicationBoard.TaggedUserID)
            {
                toActor.Add(GetActor(item.TaggedUsersID, conn));
            }

            bo.To = toActor;
            bo.WorkflowProcess = Workflow.WorkflowType.DutyTaskCommunicationBoardWorkflow;
            return bo;
        }
    }
}