using RulersCourt.Models;
using RulersCourt.Models.Calendar;
using RulersCourt.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Workflow;

namespace RulersCourt.Translators.Calendar
{
    public class CalendarWorkflowTranslator : CommonWorkflow
    {
        public WorkflowBO CalendarRequestGetWorkFlow(CalendarWorkFlowModel responseModel, string conn)
        {
            WorkflowBO bo = new Workflow.WorkflowBO();
            bo.Service = "Calendar";
            bo.Status = responseModel.Status;
            bo.ReferenceNumber = responseModel.ReferenceNumber;
            string deletgateUser;
            bo.From = GetActor(responseModel.FromID, conn);
            bo.ServiceRequestor = GetActor(responseModel.CreatorID, conn);

            switch (responseModel.Action)
            {
                case "Submit":
                    if (responseModel.ParentReferenceNumber is null || responseModel.ParentReferenceNumber == string.Empty)

                        bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.ApproverID, conn) };
                    SqlParameter[] parameters = {
                    new SqlParameter("@P_UserID", responseModel.ApproverID) };
                    deletgateUser = SqlHelper.ExecuteProcedureReturnString(conn, "Get_DelegateUser", parameters);
                    if (deletgateUser != null && deletgateUser != "0")
                        bo.DelegateTo = GetActor(Convert.ToInt32(deletgateUser), conn);

                    bo.WorkflowProcess = Workflow.WorkflowType.SubmissionWorkflow;
                    break;

                case "Resubmit":

                    if (responseModel.ApproverID is 0 || responseModel.ApproverID is null)
                    {
                        List<Workflow.Actor> toActor = new List<Workflow.Actor>();
                        foreach (UserModel item in responseModel.MediaHeadUsedID)
                        {
                            toActor.Add(GetActor(item.UserID, conn));
                        }

                        bo.To = toActor;
                        SqlParameter[] parama = {
                    new SqlParameter("@P_UserID", responseModel.ApproverID) };
                        deletgateUser = SqlHelper.ExecuteProcedureReturnString(conn, "Get_DelegateUser", parama);
                        if (deletgateUser != null && deletgateUser != "0")
                            bo.DelegateTo = GetActor(Convert.ToInt32(deletgateUser), conn);

                        bo.WorkflowProcess = Workflow.WorkflowType.SubmissionWorkflow;
                        break;
                    }
                    else
                    {
                        bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.ApproverID, conn) };
                        SqlParameter[] parama = {
                    new SqlParameter("@P_UserID", responseModel.ApproverID) };
                        deletgateUser = SqlHelper.ExecuteProcedureReturnString(conn, "Get_DelegateUser", parama);
                        if (deletgateUser != null && deletgateUser != "0")
                            bo.DelegateTo = GetActor(Convert.ToInt32(deletgateUser), conn);
                        bo.WorkflowProcess = Workflow.WorkflowType.SubmissionWorkflow;
                        break;
                    }

                case "Approve":
                    if (responseModel.ParentReferenceNumber is null)
                    {
                        bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.CreatorID, conn) };
                    }

                    if (responseModel.CurrentStatus == 120 && responseModel.PreviousApproverID != responseModel.FromID)
                    {
                        bo.From = GetActor(responseModel.PreviousApproverID, conn);
                        bo.DelegateFrom = GetActor(responseModel.FromID, conn);
                    }

                    bo.WorkflowProcess = Workflow.WorkflowType.ApprovalWorkflow;
                    break;

                case "Reject":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.CreatorID, conn) };
                    if (responseModel.CurrentStatus == 120 && responseModel.PreviousApproverID != responseModel.FromID)
                    {
                        bo.From = GetActor(responseModel.PreviousApproverID, conn);
                        bo.DelegateFrom = GetActor(responseModel.FromID, conn);
                    }

                    bo.WorkflowProcess = Workflow.WorkflowType.RejectWorkflow;
                    break;

                case "ReturnForInfo":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.CreatorID, conn) };
                    if (responseModel.CurrentStatus == 120 && responseModel.PreviousApproverID != responseModel.FromID)
                    {
                        bo.From = GetActor(responseModel.PreviousApproverID, conn);
                        bo.DelegateFrom = GetActor(responseModel.FromID, conn);
                    }

                    bo.WorkflowProcess = Workflow.WorkflowType.ReturnWorkflow;
                    break;

                case "Escalate":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.ApproverID, conn) };
                    SqlParameter[] parameter1 = {
                    new SqlParameter("@P_UserID", responseModel.ApproverID) };
                    deletgateUser = SqlHelper.ExecuteProcedureReturnString(conn, "Get_DelegateUser", parameter1);
                    if (deletgateUser != null && deletgateUser != "0")
                        bo.DelegateTo = GetActor(Convert.ToInt32(deletgateUser), conn);
                    if (responseModel.CurrentStatus == 120 && responseModel.PreviousApproverID != responseModel.FromID)
                    {
                        bo.From = GetActor(responseModel.PreviousApproverID, conn);
                        bo.DelegateFrom = GetActor(responseModel.FromID, conn);
                    }

                    bo.WorkflowProcess = Workflow.WorkflowType.EscalateWorkflow;
                    break;

                case "Redirect":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.ApproverID, conn) };
                    SqlParameter[] parameter2 = {
                    new SqlParameter("@P_UserID", responseModel.ApproverID) };
                    deletgateUser = SqlHelper.ExecuteProcedureReturnString(conn, "Get_DelegateUser", parameter2);
                    if (deletgateUser != null && deletgateUser != "0")
                        bo.DelegateTo = GetActor(Convert.ToInt32(deletgateUser), conn);
                    if (responseModel.CurrentStatus == 120 && responseModel.PreviousApproverID != responseModel.FromID)
                    {
                        bo.From = GetActor(responseModel.PreviousApproverID, conn);
                        bo.DelegateFrom = GetActor(responseModel.FromID, conn);
                    }

                    bo.WorkflowProcess = Workflow.WorkflowType.RedirectWorkflow;
                    break;

                case "Close":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.CreatorID, conn) };
                    bo.WorkflowProcess = Workflow.WorkflowType.CloseWorkflow;
                    break;

                case "MarkAsCompleted":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.CreatorID, conn) };
                    bo.WorkflowProcess = Workflow.WorkflowType.CloseWorkflow;
                    break;
            }

            return bo;
        }

        public Workflow.WorkflowBO GetCalendarBulkApprovalWorkflow(CalendarWorkFlowModel responseModel, string conn)
        {
            WorkflowBO bo = new Workflow.WorkflowBO();
            bo.Service = "Calendar";
            bo.Status = responseModel.Status;
            bo.ReferenceNumber = responseModel.ReferenceNumber;
            bo.From = GetActor(responseModel.FromID, conn);
            bo.ServiceRequestor = GetActor(responseModel.CreatorID, conn);
            List<Workflow.Actor> toActor = new List<Workflow.Actor>();
            toActor.Add(GetActor(responseModel.CreatorID, conn));
            bo.To = toActor;
            bo.WorkflowProcess = Workflow.WorkflowType.ApprovalWorkflow;
            return bo;
        }
    }
}