namespace Workflow.Interface
{
    public interface IWorkflowClient
    {
        string StartWorkflow(WorkflowBO wfObj);
    }
}
