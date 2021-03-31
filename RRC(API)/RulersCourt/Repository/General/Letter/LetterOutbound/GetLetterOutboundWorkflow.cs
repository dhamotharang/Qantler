using RulersCourt.Models;
using RulersCourt.Models.Letter;
using RulersCourt.Translators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Workflow;

namespace RulersCourt.Repository.Letter
{
    public class GetLetterOutboundWorkflow : CommonWorkflow
    {
        public Workflow.WorkflowBO GetLetterWorkflow(LetterOutboundWorkflowModel responseModel, string conn)
        {
            WorkflowBO bo = new Workflow.WorkflowBO();
            bo.Service = "Outbound Letter";
            bo.Status = responseModel.Status;
            bo.ReferenceNumber = responseModel.ReferenceNumber;
            string deletgateUser;
            bo.From = GetActor(responseModel.FromID, conn);
            bo.ServiceRequestor = GetActor(responseModel.CreatorID, conn);
            switch (responseModel.Action)
            {
                case "Save":
                    if (responseModel.ApproverID != 0 && responseModel.ApproverID != null)
                    {
                        bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.ApproverID, conn) };
                    }

                    bo.WorkflowProcess = Workflow.WorkflowType.DraftWorkflow;
                    break;

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

                    List<Workflow.Actor> toActor = new List<Workflow.Actor>();

                    SqlParameter[] param = { new SqlParameter("@P_OrganisationID", 14) };
                    SqlParameter[] archParam = { new SqlParameter("@P_Department", 14), new SqlParameter("@P_Type", 2) };

                    var archevingTeam = SqlHelper.ExecuteProcedureReturnData<List<WorkflowUserModel>>(conn, "Get_User_Workflow", r => r.TranslateAsWorkflowUserList(), archParam);

                    foreach (WorkflowUserModel item in archevingTeam)
                    {
                        toActor.Add(GetActor(item.UserID, conn));
                    }

                    bo.To = toActor;
                    if (responseModel.CurrentStatus == 19 && responseModel.PreviousApproverID != responseModel.FromID)
                    {
                        bo.From = GetActor(responseModel.PreviousApproverID, conn);
                        bo.DelegateFrom = GetActor(responseModel.FromID, conn);
                    }

                    bo.WorkflowProcess = Workflow.WorkflowType.ApprovalWorkflow;
                    break;

                case "Reject":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.CreatorID, conn) };
                    if (responseModel.CurrentStatus == 19 && responseModel.PreviousApproverID != responseModel.FromID)
                    {
                        bo.From = GetActor(responseModel.PreviousApproverID, conn);
                        bo.DelegateFrom = GetActor(responseModel.FromID, conn);
                    }

                    bo.WorkflowProcess = Workflow.WorkflowType.RejectWorkflow;
                    break;

                case "ReturnForInfo":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.CreatorID, conn) };
                    if (responseModel.CurrentStatus == 19 && responseModel.PreviousApproverID != responseModel.FromID)
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
                    if (responseModel.CurrentStatus == 19 && responseModel.PreviousApproverID != responseModel.FromID)
                    {
                        bo.From = GetActor(responseModel.PreviousApproverID, conn);
                        bo.DelegateFrom = GetActor(responseModel.FromID, conn);
                    }

                    bo.WorkflowProcess = Workflow.WorkflowType.EscalateWorkflow;
                    break;

                case "Redirect":
                    if (responseModel.CurrentStatus == 19)
                    {
                        bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.ApproverID, conn) };
                        SqlParameter[] parameter2 = {
                    new SqlParameter("@P_UserID", responseModel.ApproverID) };
                        deletgateUser = SqlHelper.ExecuteProcedureReturnString(conn, "Get_DelegateUser", parameter2);
                        if (deletgateUser != null && deletgateUser != "0")
                            bo.DelegateTo = GetActor(Convert.ToInt32(deletgateUser), conn);
                        if (responseModel.CurrentStatus == 19 && responseModel.PreviousApproverID != responseModel.FromID)
                        {
                            bo.From = GetActor(responseModel.PreviousApproverID, conn);
                            bo.DelegateFrom = GetActor(responseModel.FromID, conn);
                        }
                    }
                    else if (responseModel.CurrentStatus == 20)
                    {
                        List<Workflow.Actor> toActors = new List<Workflow.Actor>();
                        toActors.Add(GetActor(responseModel.DestinationRedirectedBy, conn));
                        bo.To = toActors;
                    }

                    bo.WorkflowProcess = Workflow.WorkflowType.RedirectWorkflow;
                    break;

                case "Close":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.CreatorID, conn) };
                    bo.WorkflowProcess = Workflow.WorkflowType.CloseWorkflow;
                    break;
            }

            return bo;
        }

        public Workflow.WorkflowBO GetLetterBulkApprovalWorkflow(LetterOutboundWorkflowModel responseModel, string conn)
        {
            WorkflowBO bo = new Workflow.WorkflowBO();
            bo.Service = "Outbound Letter";
            bo.Status = responseModel.Status;
            bo.ReferenceNumber = responseModel.ReferenceNumber;
            bo.From = GetActor(responseModel.FromID, conn);
            bo.ServiceRequestor = GetActor(responseModel.CreatorID, conn);
            List<Workflow.Actor> toActor = new List<Workflow.Actor>();
            SqlParameter[] archParam = { new SqlParameter("@P_Department", 14), new SqlParameter("@P_Type", 2) };

            var archevingTeam = SqlHelper.ExecuteProcedureReturnData<List<WorkflowUserModel>>(conn, "Get_User_Workflow", r => r.TranslateAsWorkflowUserList(), archParam);

            foreach (WorkflowUserModel item in archevingTeam)
            {
                toActor.Add(GetActor(item.UserID, conn));
            }

            bo.To = toActor;
            if (responseModel.CurrentStatus == 19 && responseModel.PreviousApproverID != responseModel.FromID)
            {
                bo.From = GetActor(responseModel.PreviousApproverID, conn);
                bo.DelegateFrom = GetActor(responseModel.FromID, conn);
            }

            bo.WorkflowProcess = Workflow.WorkflowType.ApprovalWorkflow;
            return bo;
        }
    }
}
