using RulersCourt.Models;
using RulersCourt.Models.Legal;
using System;
using System.Collections.Generic;
using Workflow;

namespace RulersCourt.Repository.Legal
{
    public class LegalGetWorkflow : CommonWorkflow
    {
        public Workflow.WorkflowBO GetLegalWorkflow(LegalWorkflowModel responseModel, string conn)
        {
            WorkflowBO bo = new Workflow.WorkflowBO();
            bo.Service = "Legal";
            bo.Status = responseModel.Status;
            bo.ReferenceNumber = responseModel.ReferenceNumber;

            bo.From = GetActor(responseModel.FromID, conn);
            bo.ServiceRequestor = GetActor(responseModel.CreatorID, conn);

            switch (responseModel.Action)
            {
                case "Submit":

                    List<Workflow.Actor> toActor = new List<Workflow.Actor>();
                    foreach (UserModel item in responseModel.LegalHeadUsedID)
                    {
                        toActor.Add(GetActor(item.UserID, conn));
                    }

                    bo.To = toActor;
                    bo.WorkflowProcess = Workflow.WorkflowType.SubmissionWorkflow;
                    break;

                case "Resubmit":
                    List<Workflow.Actor> toActorID = new List<Workflow.Actor>();
                    foreach (UserModel item in responseModel.LegalHeadUsedID)
                    {
                        toActorID.Add(GetActor(item.UserID, conn));
                    }

                    bo.To = toActorID;
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

                case "ReturnForInfo":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.CreatorID, conn) };

                    bo.WorkflowProcess = Workflow.WorkflowType.ReturnWorkflow;
                    break;

                case "Close":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.CreatorID, conn) };
                    bo.WorkflowProcess = Workflow.WorkflowType.CloseWorkflow;
                    break;

                case "ReOpen":
                    bo.To = new System.Collections.Generic.List<Workflow.Actor>() { GetActor(responseModel.CreatorID, conn) };
                    bo.WorkflowProcess = Workflow.WorkflowType.ReopenWorkflow;
                    break;
            }

            return bo;
        }
    }
}