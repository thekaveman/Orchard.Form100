using System.ComponentModel;

namespace CSM.Form100.Models
{
    public enum WorkflowStatus
    {
        [Description("Approved")]
        Approved,

        [Description("In Progress")]
        InProgress,

        [Description("New")]
        New,
        
        [Description("Pending")]
        Pending,

        [Description("Rejected")]
        Rejected,

        [Description("Submitted")]
        Submitted,
    }
}