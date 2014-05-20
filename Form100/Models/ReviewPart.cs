using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;

namespace CSM.Form100.Models
{
    public class ReviewPart : ContentPart<ReviewPartRecord>
    {
        internal readonly LazyField<Queue<ReviewDecisionRecord>> ApprovalChainField = new LazyField<Queue<ReviewDecisionRecord>>();
        
        public Queue<ReviewDecisionRecord> ApprovalChain
        {
            get { return ApprovalChainField.Value; }
            set { ApprovalChainField.Value = value; }
        }

        public WorkflowStatus Status
        {
            get { return Record.Status; }
            set { Record.Status = value; }
        }
    }
}