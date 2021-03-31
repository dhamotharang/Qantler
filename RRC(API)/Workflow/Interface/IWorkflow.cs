using Workflow.Utility;

namespace Workflow.Interface
{
    public interface IWorkflow
    {
        string ExecuteWorkflow(Settings settings, WorkflowBO wfObject);
    }
}
