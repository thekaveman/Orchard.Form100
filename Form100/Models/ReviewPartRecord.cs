using System.Collections.Generic;
using Orchard.ContentManagement.Records;

namespace CSM.Form100.Models
{
    public class ReviewPartRecord : ContentPartRecord
    {
        public virtual WorkflowStatus Status { get; set; }

        public virtual IList<int> ApprovalIds { get; set; }

        public ReviewPartRecord()
        {
            ApprovalIds = new List<int>();
        }
    }
}