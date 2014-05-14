using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;

namespace CSM.Form100.Models
{
    public class ReviewPart : ContentPart<ReviewPartRecord>
    {
        internal readonly LazyField<IList<ReviewApprovalRecord>> ApprovalsField = new LazyField<IList<ReviewApprovalRecord>>();
        
        public IList<ReviewApprovalRecord> Approvals
        {
            get { return ApprovalsField.Value; }
            set { ApprovalsField.Value = value; }
        }

        public WorkflowStatus Status
        {
            get { return Retrieve(r => r.Status); }
            set { Store(r => r.Status, value); }
        }
    }
}