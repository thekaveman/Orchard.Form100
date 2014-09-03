using System.ComponentModel;

namespace CSM.Form100
{
    /// <summary>
    /// An marker for tracking state in a Workflow.
    /// </summary>
    public enum WorkflowStatus
    {
        [Description("Undefined")]
        Undefined,

        [Description("New")]
        New,

        [Description("In Progress")]
        InProgress,

        [Description("Submitted")]
        Submitted,

        [Description("Approved")]
        Approved,

        [Description("Rejected")]
        Rejected
    }
}