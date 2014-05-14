using System.Collections.Generic;
using Orchard.ContentManagement.Records;

namespace CSM.Form100.Models
{
    public class ReviewPartRecord : ContentPartRecord
    {
        public virtual WorkflowStatus Status { get; set; }

        public virtual IList<ApprovalRecord> Approvals { get; set; }

        public ReviewPartRecord()
        {
            Approvals = new List<ApprovalRecord>();
        }
    }
}