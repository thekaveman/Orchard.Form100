using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;

namespace CSM.Form100.Models
{
    public class ReviewPart : ContentPart<ReviewPartRecord>
    {
        private readonly LazyField<IList<ApprovalRecord>> _approvalsField = new LazyField<IList<ApprovalRecord>>();

        public LazyField<IList<ApprovalRecord>> ApprovalsField
        {
            get { return _approvalsField; }
        }

        public IList<ApprovalRecord> Approvals
        {
            get { return _approvalsField.Value; }
            set { _approvalsField.Value = value; }
        }

        public WorkflowStatus Status
        {
            get { return Retrieve(r => r.Status); }
            set { Store(r => r.Status, value); }
        }
    }
}