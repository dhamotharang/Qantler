using RulersCourt.Models;
using RulersCourt.Models.Circular;
using RulersCourt.Translators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Workflow;

namespace RulersCourt.Repository.Circular
{
    public class GetCircularWorkflow : CommonWorkflow
    {
        public Workflow.WorkflowBO GetCircularsWorkflow(CircularWorkflowModel responseModel, string conn)
        {
            WorkflowBO bo = new Workflow.WorkflowBO();
            bo.Service = "Circular";
            bo.Status = responseModel.Status;
            bo.ReferenceNumber = responseModel.ReferenceNumber;
            string deletgateUser;

            bo.From = GetActor(responseModel.FromID, conn);
            bo.ServiceRequestor = GetActor(responseModel.CreatorID, conn);

            switch (responseModel.Action)
            {
                case "Save":
                    if (responseModel.ApproverID != 0)
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
                    List<Workflow.Actor> toActorsDep = new List<Workflow.Actor>();
                    foreach (CircularDestinationDepartmentGetModel item in responseModel.DestinationDepartmentID)
                    {
                        SqlParameter[] param = { new SqlParameter("@P_DepartmentID", item.CircularDestinationDepartmentID) };
                        var email = SqlHelper.ExecuteProcedureReturnString(conn, "GetDepartmentGroupMailID", param);
                        var name = item.CircularDestinationDepartmentName;
                        toActor.Add(new Workflow.Actor() { Name = name, Email = email, CanSendEmail = true });

                        SqlParameter[] parameter = {
                        new SqlParameter("@P_Department", item.CircularDestinationDepartmentID),
                        new SqlParameter("@P_Type", 0),
                        new SqlParameter("@P_Language", "EN")
                        };

                        List<UserModel> userList = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(conn, "Get_User", r => r.TranslateAsUserList(), parameter);
                        foreach (var users in userList)
                        {
                            toActorsDep.Add(new Workflow.Actor() { Name = users.EmployeeName, ARName = users.AREmployeeName, Email = users.OfficialEmailID, CanSendEmail = Convert.ToBoolean(users.CanSendEmail), CanSendSMS = Convert.ToBoolean(users.CanSendSMS), PhoneNumber = users.MobileNumber });
                        }
                    }

                    bo.SMSOrgDepUsers = toActorsDep;
                    bo.To = toActor;
                    if (responseModel.CurrentStatus == 13 && responseModel.PreviousApproverID != responseModel.FromID)
                    {
                        bo.From = GetActor(responseModel.PreviousApproverID, conn);
                        bo.DelegateFrom = GetActor(responseModel.FromID, conn);
                    }

                    bo.WorkflowProcess = Workflow.WorkflowType.ApprovalWorkflow;
                    break;

                case "Reject":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.CreatorID, conn) };
                    if (responseModel.CurrentStatus == 13 && responseModel.PreviousApproverID != responseModel.FromID)
                    {
                        bo.From = GetActor(responseModel.PreviousApproverID, conn);
                        bo.DelegateFrom = GetActor(responseModel.FromID, conn);
                    }

                    bo.WorkflowProcess = Workflow.WorkflowType.RejectWorkflow;
                    break;

                case "ReturnForInfo":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.CreatorID, conn) };
                    if (responseModel.CurrentStatus == 13 && responseModel.PreviousApproverID != responseModel.FromID)
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
                    if (responseModel.CurrentStatus == 13 && responseModel.PreviousApproverID != responseModel.FromID)
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
