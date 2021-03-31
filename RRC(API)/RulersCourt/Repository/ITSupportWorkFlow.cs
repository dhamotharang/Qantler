using RulersCourt.Models;
using RulersCourt.Models.ITSupport;
using RulersCourt.Translators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Workflow;

namespace RulersCourt.Repository
{
    public class ITSupportWorkFlow
    {
        public Workflow.WorkflowBO GetITSupportWorkflow(ITSupportTranslatorModel responseModel, string conn)
        {
            WorkflowBO bo = new Workflow.WorkflowBO();
            bo.Service = "ITSupport";
            bo.Status = responseModel.Status;
            bo.ReferenceNumber = responseModel.ReferenceNumber;

            bo.From = new Workflow.Actor() { Email = GetEmail(responseModel.FromID, conn), Name = GetName(responseModel.FromID, conn) };
            bo.ServiceRequestor = new Workflow.Actor() { Email = GetEmail(responseModel.CreatorID, conn), Name = GetName(responseModel.CreatorID, conn) };

            switch (responseModel.Action)
            {
                case "Submit":

                    List<Workflow.Actor> toActor = new List<Workflow.Actor>();
                    foreach (UserModel item in responseModel.HRITSupportUsedId)
                    {
                        toActor.Add(new Workflow.Actor() { Name = item.EmployeeName, Email = item.OfficialEmailID });
                    }

                    bo.To = toActor;
                    bo.WorkflowProcess = Workflow.WorkflowType.SubmissionWorkflow;
                    break;

                case "Close":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() {
                        new Workflow.Actor() { Name = GetName(responseModel.CreatorID, conn), Email = GetEmail(responseModel.CreatorID, conn) } };
                    bo.WorkflowProcess = Workflow.WorkflowType.CloseWorkflow;
                    break;
            }

            return bo;
        }

        private static string GetEmail(int? userID, string conn)
        {
            SqlParameter[] param = { new SqlParameter("@P_UserID", userID), new SqlParameter("@P_Department", null) };
            var temp = SqlHelper.ExecuteProcedureReturnData<List<WorkflowUserModel>>(conn, "Get_User_Workflow", r => r.TranslateAsWorkflowUserList(), param);
            return temp.FirstOrDefault().OfficialEmailID;
        }

        private static string GetName(int? userID, string conn)
        {
            SqlParameter[] param = { new SqlParameter("@P_UserID", userID), new SqlParameter("@P_Department", null) };
            var temp = SqlHelper.ExecuteProcedureReturnData<List<WorkflowUserModel>>(conn, "Get_User_Workflow", r => r.TranslateAsWorkflowUserList(), param);
            return temp.FirstOrDefault().EmployeeName;
        }

        private static List<OrganizationModel> GetM_Organisation(string connString)
        {
            var e = SqlHelper.ExecuteProcedureReturnData<List<OrganizationModel>>(connString, "Get_Organization", r => r.TranslateAsOrganizationList());
            return e;
        }
    }
}
