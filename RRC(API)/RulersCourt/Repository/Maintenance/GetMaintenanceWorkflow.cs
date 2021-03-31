using RulersCourt.Models;
using RulersCourt.Models.Maintenance;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Workflow;

namespace RulersCourt.Repository.Maintenance
{
    public class GetMaintenanceWorkflow : CommonWorkflow
    {
        public Workflow.WorkflowBO GetWorkflow(MaintenanceWorkflowModel responseModel, string conn)
        {
            WorkflowBO bo = new Workflow.WorkflowBO();
            bo.Service = "Maintenance";
            bo.Status = responseModel.Status;
            bo.ReferenceNumber = responseModel.ReferenceNumber;
            string deletgateUser;
            bo.From = GetActor(responseModel.FromID, conn);
            bo.ServiceRequestor = GetActor(responseModel.CreatorID, conn);
            switch (responseModel.Action)
            {
                case "Submit":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.ApproverID, conn) };
                    SqlParameter[] parameters = {
                    new SqlParameter("@P_UserID", responseModel.ApproverID) };
                    deletgateUser = SqlHelper.ExecuteProcedureReturnString(conn, "Get_DelegateUser", parameters);
                    if (deletgateUser != null && deletgateUser != "0")
                        bo.DelegateTo = GetActor(Convert.ToInt32(deletgateUser), conn);

                    bo.WorkflowProcess = Workflow.WorkflowType.SubmissionWorkflow;
                    break;

                case "Resubmit":

                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.ApproverID, conn) };
                    SqlParameter[] parama = {
                    new SqlParameter("@P_UserID", responseModel.ApproverID) };
                    deletgateUser = SqlHelper.ExecuteProcedureReturnString(conn, "Get_DelegateUser", parama);
                    if (deletgateUser != null && deletgateUser != "0")
                        bo.DelegateTo = GetActor(Convert.ToInt32(deletgateUser), conn);

                    bo.WorkflowProcess = Workflow.WorkflowType.SubmissionWorkflow;
                    break;

                case "Approve":
                    if (responseModel.CurrentStatus == 53)
                    {
                        List<Workflow.Actor> toActor = new List<Workflow.Actor>();
                        foreach (UserModel item in responseModel.MaintenanceHeadUsedID)
                        {
                            toActor.Add(GetActor(item.UserID, conn));
                        }

                        bo.To = toActor;
                        if (responseModel.CurrentStatus == 53 && responseModel.PreviousApproverID != responseModel.FromID)
                        {
                            bo.From = GetActor(responseModel.PreviousApproverID, conn);
                            bo.DelegateFrom = GetActor(responseModel.FromID, conn);
                        }

                        bo.WorkflowProcess = Workflow.WorkflowType.ApprovalWorkflow;
                    }
                    else if (responseModel.CurrentStatus == 55)
                    {
                        bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.RequestorID, conn) };
                        if (responseModel.CurrentStatus == 55 && responseModel.PreviousApproverID != responseModel.FromID)
                        {
                            bo.From = GetActor(responseModel.PreviousApproverID, conn);
                            bo.DelegateFrom = GetActor(responseModel.FromID, conn);
                        }

                        bo.WorkflowProcess = Workflow.WorkflowType.ApprovalWorkflow;
                    }
                    else
                    {
                        bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.FromID, conn) };
                        if (responseModel.CurrentStatus == 53 && responseModel.PreviousApproverID != responseModel.FromID)
                        {
                            bo.From = GetActor(responseModel.PreviousApproverID, conn);
                            bo.DelegateFrom = GetActor(responseModel.FromID, conn);
                        }

                        bo.WorkflowProcess = Workflow.WorkflowType.ApprovalWorkflow;
                    }

                    break;

                case "AssignTo":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.AssigneeID, conn) };
                    bo.WorkflowProcess = Workflow.WorkflowType.AssignWorkflow;
                    break;

                case "AssignToMe":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.FromID, conn) };
                    bo.WorkflowProcess = Workflow.WorkflowType.AssignToMeWorkflow;
                    break;

                case "Reject":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.CreatorID, conn) };
                    if (responseModel.CurrentStatus == 53 && responseModel.PreviousApproverID != responseModel.FromID)
                    {
                        bo.From = GetActor(responseModel.PreviousApproverID, conn);
                        bo.DelegateFrom = GetActor(responseModel.FromID, conn);
                    }

                    bo.WorkflowProcess = Workflow.WorkflowType.RejectWorkflow;
                    break;

                case "ReturnForInfo":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.CreatorID, conn) };
                    if (responseModel.CurrentStatus == 53 && responseModel.PreviousApproverID != responseModel.FromID)
                    {
                        bo.From = GetActor(responseModel.PreviousApproverID, conn);
                        bo.DelegateFrom = GetActor(responseModel.FromID, conn);
                    }

                    bo.WorkflowProcess = Workflow.WorkflowType.ReturnWorkflow;
                    break;

                case "ReOpen":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.CreatorID, conn) };

                    bo.WorkflowProcess = Workflow.WorkflowType.ReopenWorkflow;
                    break;

                case "Escalate":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.ApproverID, conn) };
                    SqlParameter[] parameter1 = {
                    new SqlParameter("@P_UserID", responseModel.ApproverID) };
                    deletgateUser = SqlHelper.ExecuteProcedureReturnString(conn, "Get_DelegateUser", parameter1);
                    if (deletgateUser != null && deletgateUser != "0")
                        bo.DelegateTo = GetActor(Convert.ToInt32(deletgateUser), conn);
                    if (responseModel.CurrentStatus == 53 && responseModel.PreviousApproverID != responseModel.FromID)
                    {
                        bo.From = GetActor(responseModel.PreviousApproverID, conn);
                        bo.DelegateFrom = GetActor(responseModel.FromID, conn);
                    }

                    bo.WorkflowProcess = Workflow.WorkflowType.EscalateWorkflow;
                    break;

                case "Close":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.CreatorID, conn) };
                    bo.WorkflowProcess = Workflow.WorkflowType.CloseWorkflow;
                    break;
            }

            return bo;
        }
    }
}
