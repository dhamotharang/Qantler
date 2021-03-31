using RulersCourt.Models;
using RulersCourt.Models.BabyAddition;
using System;
using System.Collections.Generic;
using Workflow;

namespace RulersCourt.Repository
{
    public class BabyAdditionGetWorkflow : CommonWorkflow
    {
        public Workflow.WorkflowBO GetBabyAdditionWorkflow(BabyAdditionWorkflowModel responseModel, string conn)
        {
            WorkflowBO bo = new Workflow.WorkflowBO();
            bo.Service = "BabyAddition";
            bo.Status = responseModel.Status;
            bo.ReferenceNumber = responseModel.ReferenceNumber;

            bo.From = GetActor(responseModel.FromID, conn);
            bo.ServiceRequestor = GetActor(responseModel.CreatorID, conn);

            switch (responseModel.Action)
            {
                case "Submit":

                    List<Workflow.Actor> toActor = new List<Workflow.Actor>();
                    foreach (UserModel item in responseModel.HRHeadUsedID)
                    {
                        toActor.Add(GetActor(item.UserID, conn));
                    }

                    bo.To = toActor;
                    bo.WorkflowProcess = Workflow.WorkflowType.SubmissionWorkflow;
                    break;

                case "AssignTo":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.AssigneeID, conn) };
                    bo.WorkflowProcess = Workflow.WorkflowType.AssignWorkflow;
                    break;

                case "AssignToMe":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.FromID, conn) };
                    bo.WorkflowProcess = Workflow.WorkflowType.AssignToMeWorkflow;
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