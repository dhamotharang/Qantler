using RulersCourt.Models;
using RulersCourt.Models.OfficialTaskCompensation.OfficialTask;
using RulersCourt.Translators;
using System.Collections.Generic;
using Workflow;

namespace RulersCourt.Repository.OfficialTaskCompensation.OfficialTask
{
    public class OfficialTaskGetWorkflow : CommonWorkflow
    {
        public Workflow.WorkflowBO GetOfficialTaskWorkflow(OfficialTaskWorkflowModel responseModel, string conn)
        {
            WorkflowBO bo = new Workflow.WorkflowBO();
            bo.Service = "OfficialTask";
            bo.Status = responseModel.Status;
            bo.ReferenceNumber = responseModel.ReferenceNumber;
            bo.From = GetActor(responseModel.FromID, conn);
            bo.ServiceRequestor = GetActor(responseModel.CreatorID, conn);

            switch (responseModel.Action)
            {
                case "Submit":

                    List<Workflow.Actor> toActor = new List<Workflow.Actor>();
                    foreach (OfficialTaskEmployeeNameModel item in responseModel.EmployeeNameID)
                    {
                        toActor.Add(GetActor(item.OfficialTaskEmployeeID, conn));
                    }

                    bo.To = toActor;
                    bo.WorkflowProcess = Workflow.WorkflowType.SubmissionWorkflow;
                    break;

                case "MarkasComplete":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.CreatorID, conn) };
                    bo.WorkflowProcess = Workflow.WorkflowType.CloseWorkflow;
                    break;
            }

            return bo;
        }

        private static List<OrganizationModel> GetM_Organisation(string connString)
        {
            var e = SqlHelper.ExecuteProcedureReturnData<List<OrganizationModel>>(connString, "Get_Organization", r => r.TranslateAsOrganizationList());
            return e;
        }
    }
}
