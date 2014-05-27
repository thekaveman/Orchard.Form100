using Orchard.ContentManagement.Records;
using Orchard.Data.Conventions;

namespace CSM.Form100.Models
{
    public class ReviewPartRecord : ContentPartRecord
    {
        [StringLengthMax]
        public virtual string ApprovalChainIds { get; set; }

        public virtual WorkflowStatus Status { get; set; }
        
        public ReviewPartRecord()
        {
            Status = WorkflowStatus.New;
        }
    }
}