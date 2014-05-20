using Orchard.ContentManagement.Records;

namespace CSM.Form100.Models
{
    public class ReviewPartRecord : ContentPartRecord
    {
        public virtual string ApprovalChainIds { get; set; }

        public virtual WorkflowStatus Status { get; set; }
    }
}