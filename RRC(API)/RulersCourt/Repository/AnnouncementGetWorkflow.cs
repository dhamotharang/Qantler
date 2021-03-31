using RulersCourt.Models;
using RulersCourt.Models.Announcement;
using System.Collections.Generic;
using Workflow;

namespace RulersCourt.Repository
{
    public class AnnouncementGetWorkflow : CommonWorkflow
    {
        public WorkflowBO GetAnnouncementWorkflow(AnnouncementWorkflowModel responseModel, string conn)
        {
            WorkflowBO bo = new WorkflowBO();
            bo.Service = "Announcement";
            bo.Status = responseModel.Status;
            bo.ReferenceNumber = responseModel.ReferenceNumber;

            bo.From = GetActor(responseModel.FromID, conn);
            bo.ServiceRequestor = GetActor(responseModel.CreatorID, conn);

            switch (responseModel.Action)
            {
                case "Submit":
                    List<Actor> toActor = new List<Actor>();
                    foreach (UserModel item in responseModel.HRHeadUsedID)
                    {
                        toActor.Add(GetActor(item.UserID, conn));
                    }

                    bo.To = toActor;
                    bo.WorkflowProcess = WorkflowType.SubmissionWorkflow;
                    break;

                case "AssignTo":
                    bo.To = new List<Actor>() { GetActor(responseModel.AssigneeID, conn) };
                    bo.WorkflowProcess = WorkflowType.AssignWorkflow;
                    break;

                case "AssignToMe":
                    bo.To = new List<Actor>() { GetActor(responseModel.FromID, conn) };
                    bo.WorkflowProcess = WorkflowType.AssignToMeWorkflow;
                    break;

                case "Close":
                    bo.To = new List<Actor>() { GetActor(responseModel.CreatorID, conn) };
                    bo.WorkflowProcess = WorkflowType.CloseWorkflow;
                    break;
            }

            return bo;
        }
    }
}