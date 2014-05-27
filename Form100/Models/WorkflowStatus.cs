using System.ComponentModel;

namespace CSM.Form100.Models
{
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

        [Description("Pending")]
        Pending,

        [Description("Approved")]
        Approved,

        [Description("Rejected")]
        Rejected
    }
}