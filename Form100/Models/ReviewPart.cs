using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;

namespace CSM.Form100.Models
{
    public class ReviewPart : ContentPart<ReviewPartRecord>
    {
        internal readonly LazyField<Queue<ReviewApprovalRecord>> ApprovalChainField = new LazyField<Queue<ReviewApprovalRecord>>();
        
        public Queue<ReviewApprovalRecord> ApprovalChain
        {
            get { return ApprovalChainField.Value; }
            set { ApprovalChainField.Value = value; }
        }

        public WorkflowStatus Status
        {
            get { return Retrieve(r => r.Status); }
            set { Store(r => r.Status, value); }
        }
    }
}