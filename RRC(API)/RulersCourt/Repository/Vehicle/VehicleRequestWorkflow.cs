using RulersCourt.Models;
using RulersCourt.Models.Vehicle.VehicleFine;
using RulersCourt.Models.Vehicle.VehicleRequest;
using RulersCourt.Translators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Workflow;

namespace RulersCourt.Repository.Vehicle
{
    public class VehicleRequestWorkflow : CommonWorkflow
    {
        public Workflow.WorkflowBO GetVehicleWorkflow(VehicleRequestModel responseModel, string conn, bool? bulkAction = false)
        {
            WorkflowBO bo = new Workflow.WorkflowBO();
            bo.Service = "Vehicle";
            bo.ServiceID = responseModel.VehicleReqID ?? 0;
            bo.Status = responseModel.Status;
            bo.ReferenceNumber = responseModel.ReferenceNumber;
            string deletgateUser;
            bo.From = GetActor(responseModel.FromID, conn);
            bo.ServiceRequestor = GetActor(responseModel.CreatorID, conn);
            List<Workflow.Actor> toActor = new List<Workflow.Actor>();

            List<M_UnitModel> unit = DbClientFactory<M_MasterLookupClient>.Instance.GetUnits(conn, 13, "EN");
            var orgName = unit.Find(item => item.UnitID == 13).UnitName;

            SqlParameter[] orgNaram = { new SqlParameter("@P_OrganisationID", 13) };
            var orgEmail = SqlHelper.ExecuteProcedureReturnString(conn, "GetOrganisationGroupMailID", orgNaram);

            List<Workflow.Actor> toActorsOrg = new List<Workflow.Actor>();

            SqlParameter[] parametersOrg = {
                        new SqlParameter("@P_Department", 13),
                        new SqlParameter("@P_Type", 2),
                        new SqlParameter("@P_Language", "EN"),
                        };

            List<UserModel> userList = SqlHelper.ExecuteProcedureReturnData<List<UserModel>>(conn, "Get_User", r => r.TranslateAsUserList(), parametersOrg);

            foreach (var users in userList)
            {
                toActorsOrg.Add(new Workflow.Actor() { Name = users.EmployeeName, ARName = users.AREmployeeName, Email = users.OfficialEmailID, CanSendEmail = Convert.ToBoolean(users.CanSendEmail), CanSendSMS = Convert.ToBoolean(users.CanSendSMS), PhoneNumber = users.MobileNumber });
            }

            switch (responseModel.Action)
            {
                case "Submit":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>()
                    { GetActor(responseModel.ApproverID, conn) };
                    SqlParameter[] parameters = {
                    new SqlParameter("@P_UserID", responseModel.ApproverID) };
                    deletgateUser = SqlHelper.ExecuteProcedureReturnString(conn, "Get_DelegateUser", parameters);
                    if (deletgateUser != null && deletgateUser != "0")
                        bo.DelegateTo = GetActor(Convert.ToInt32(deletgateUser), conn);

                    bo.WorkflowProcess = Workflow.WorkflowType.SubmissionWorkflow;
                    break;

                case "Approve":
                    List<Workflow.Actor> toActorID = new List<Workflow.Actor>();

                    toActorID.Add(new Workflow.Actor() { Name = orgName, Email = orgEmail, CanSendEmail = true });

                    bo.To = toActorID;

                    bo.SMSOrgDepUsers = toActorsOrg;

                    bo.WorkflowProcess = Workflow.WorkflowType.ApprovalWorkflow;
                    break;

                case "Assign":
                    List<Workflow.Actor> toActorsID = new List<Workflow.Actor>();

                    toActorsID.Add(new Workflow.Actor() { Name = orgName, Email = orgEmail, CanSendEmail = true });
                    toActorsID.Add(GetActor(responseModel.DriverID, conn));
                    bo.To = toActorsID;

                    bo.SMSOrgDepUsers = toActorsOrg;

                    bo.WorkflowProcess = Workflow.WorkflowType.AssignWorkflow;
                    break;

                case "Release":
                    List<Workflow.Actor> toReleaseID = new List<Workflow.Actor>();

                    if (responseModel.RequestorType == 2 || responseModel.RequestorType == 3)
                    {
                        toReleaseID.Add(GetActor(responseModel.RequestorID, conn));
                    }
                    else
                    {
                        toReleaseID.Add(new Workflow.Actor() { Name = orgName, Email = orgEmail, CanSendEmail = true });

                        bo.SMSOrgDepUsers = toActorsOrg;
                    }

                    bo.To = toReleaseID;
                    bo.WorkflowProcess = Workflow.WorkflowType.VehicleReleaseWorkflow;
                    break;

                case "Return":
                    List<Workflow.Actor> toReturnID = new List<Workflow.Actor>();
                    if (responseModel.RequestorType == 2 || responseModel.RequestorType == 3)
                    {
                        toReturnID.Add(GetActor(responseModel.RequestorID, conn));
                    }
                    else
                    {
                        toReturnID.Add(new Workflow.Actor() { Name = orgName, Email = orgEmail, CanSendEmail = true });

                        bo.SMSOrgDepUsers = toActorsOrg;
                    }

                    bo.To = toReturnID;
                    bo.WorkflowProcess = Workflow.WorkflowType.VehicleReturnWorkflow;
                    break;

                case "ReleaseConfirm":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { new Workflow.Actor() { Name = orgName, Email = orgEmail, CanSendEmail = true } };

                    bo.SMSOrgDepUsers = toActorsOrg;

                    if (bulkAction == true)
                        bo.From = GetActor(responseModel.RequestorID, conn);
                    bo.WorkflowProcess = Workflow.WorkflowType.VehicleReleaseConfirmWorkflow;
                    break;

                case "ReturnConfirm":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { new Workflow.Actor() { Name = orgName, Email = orgEmail, CanSendEmail = true } };

                    bo.SMSOrgDepUsers = toActorsOrg;
                    bo.WorkflowProcess = Workflow.WorkflowType.VehicleReturnConfirmWorkflow;
                    break;

                case "Reject":
                    if (responseModel.CurrentStatus == 214)
                    {
                        if (responseModel.PreviousApproverID != responseModel.FromID && responseModel.PreviousApproverID != null)
                        {
                            bo.From = GetActor(responseModel.PreviousApproverID, conn);
                            bo.DelegateFrom = GetActor(responseModel.FromID, conn);
                        }

                        bo.Status = 211;
                        List<Workflow.Actor> toRejectID = new List<Workflow.Actor>();
                        toRejectID.Add(new Workflow.Actor() { Name = orgName, Email = orgEmail, CanSendEmail = true });

                        bo.SMSOrgDepUsers = toActorsOrg;

                        bo.To = toRejectID;
                        bo.WorkflowProcess = Workflow.WorkflowType.VehicleReleaseConfirmationRejectWorkflow;
                    }
                    else
                    {
                        bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.CreatorID, conn) };
                        if (responseModel.PreviousApproverID != responseModel.FromID && responseModel.PreviousApproverID != null)
                        {
                            bo.From = GetActor(responseModel.PreviousApproverID, conn);
                            bo.DelegateFrom = GetActor(responseModel.FromID, conn);
                        }

                        bo.WorkflowProcess = Workflow.WorkflowType.RejectWorkflow;
                    }

                    break;

                case "Close":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.CreatorID, conn) };
                    bo.WorkflowProcess = Workflow.WorkflowType.CloseWorkflow;
                    break;

                case "Cancel":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.CreatorID == responseModel.FromID ? responseModel.ApproverID : responseModel.CreatorID, conn) };
                    bo.WorkflowProcess = Workflow.WorkflowType.VehicleCancelWorkflow;
                    break;
            }

            return bo;
        }

        public Workflow.WorkflowBO GetVehicleRemainderWorkflow(VehicleFineGetModel responseModel, string conn, SendRemainderModel remainder, int userID)
        {
            WorkflowBO bo = new Workflow.WorkflowBO();
            bo.Service = "VehicleFine";
            bo.Status = responseModel.Status.GetValueOrDefault();
            bo.ReferenceNumber = responseModel.ReferenceNumber;
            bo.From = GetActor(userID, conn);
            bo.ServiceRequestor = GetActor(remainder.EmailCCUserID, conn);
            List<Workflow.Actor> toAct = new List<Workflow.Actor>();
            toAct.Add(GetActor(responseModel.DriverID, conn));
            bo.To = toAct;
            bo.WorkflowProcess = Workflow.WorkflowType.VehicleReturnWorkflow;
            return bo;
        }
    }
}
