using Workflow.Interface;
using Workflow.Utility;
using Workflow.WorkflowSteps;

namespace Workflow
{
    public class Workflow
    {
        public IWorkflow _workflow;
        public Workflow(WorkflowType wfType)
        {
            InitWorkflowStep(wfType);
        }

        public string ExecuteWorkflow(Settings settings, WorkflowBO wfObject)
        {
            return _workflow.ExecuteWorkflow(settings, wfObject);
        }

        private void InitWorkflowStep(WorkflowType wfType)
        {
            switch (wfType)
            {
                case WorkflowType.DraftWorkflow:
                    _workflow = new DraftWorkflow();
                    break;
                case WorkflowType.SubmissionWorkflow:
                    _workflow = new SubmissionWorkflow();
                    break;
                case WorkflowType.ApprovalWorkflow:
                    _workflow = new ApprovalWorkflow();
                    break;
                case WorkflowType.RejectWorkflow:
                    _workflow = new RejectWorkflow();
                    break;
                case WorkflowType.EscalateWorkflow:
                    _workflow = new EscalateWorkflow();
                    break;
                case WorkflowType.RedirectWorkflow:
                    _workflow = new RedirectWorkflow();
                    break;
                case WorkflowType.ReturnWorkflow:
                    _workflow = new ReturnWorkflow();
                    break;
                case WorkflowType.ShareWorkflow:
                    _workflow = new ShareWorkflow();
                    break;
                case WorkflowType.CloseWorkflow:
                    _workflow = new CloseWorkflow();
                    break;
                case WorkflowType.DutyTaskSubmissionWorkflow:
                    _workflow = new DutyTaskSubmissionWorkflow();
                    break;
                case WorkflowType.DutyTaskCommunicationBoardWorkflow:
                    _workflow = new DutyTaskCommunicationBoardWorkflow();
                    break;
                case WorkflowType.DutyTaskCompleteWorkflow:
                    _workflow = new DutyTaskCompleteWorkflow();
                    break;
                case WorkflowType.DutyTaskDeleteWorkflow:
                    _workflow = new DutyTaskDeleteWorkflow();
                    break;
                case WorkflowType.AssignWorkflow:
                    _workflow = new AssignWorkflow();
                    break;
                case WorkflowType.AssignToMeWorkflow:
                    _workflow = new AssignToMeWorkflow();
                    break;
                case WorkflowType.ReopenWorkflow:
                    _workflow = new ReopenWorkflow();
                    break;
                case WorkflowType.MeetingInvitesWorkflow:
                    _workflow = new MeetingInvitesWorkflow();

                    break;
                case WorkflowType.MeetingRescheduleWorkflow:
                    _workflow = new MeetingRescheduleWorkflow();

                    break;

                case WorkflowType.MeetingCancelWorkflow:
                    _workflow = new MeetingCancelWorkflow();

                    break;
                case WorkflowType.MeetingMomCreatedWorkflow:
                    _workflow = new MeetingMomCreatedWorkflow();

                    break;
                case WorkflowType.MeetingChatWorkflow:
                    _workflow = new MeetingChatWorkflow();

                    break;
				case WorkflowType.TrainingNotificationToManagerWorkflow:
					_workflow = new TrainingNotificationToManagerWorkflow();

					break;

                case WorkflowType.VehicleReleaseWorkflow:
                    _workflow = new VehicleReleaseWorkflow();

                    break;
                case WorkflowType.VehicleReleaseConfirmWorkflow:
                    _workflow = new VehicleReleaseConfirmWorkflow();

                    break;
                case WorkflowType.VehicleReturnWorkflow:
                    _workflow = new VehicleReturnWorkflow();

                    break;

                case WorkflowType.VehicleReturnConfirmWorkflow:
                    _workflow = new VehicleReturnConfirmWorkflow();

                    break;

				case WorkflowType.VehicleRemainderWorkflow:
					_workflow = new VehicleRemainderWorkflow();

					break;

				case WorkflowType.FineSubmissionWorkflow:
					_workflow = new FineSubmissionWorkflow();

					break;

				case WorkflowType.VehicleCancelWorkflow:
					_workflow = new VehicleCancelWorkflow();

					break;

				case WorkflowType.VehicleReleaseConfirmationRejectWorkflow:
					_workflow = new VehicleReleaseConfirmationRejectWorkflow();

					break;

				default:
                    break;
            }
        }
    }
}
