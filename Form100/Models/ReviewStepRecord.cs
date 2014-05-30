using System;

namespace CSM.Form100.Models
{
    public class ReviewStepRecord
    {
        public virtual int Id { get; set; }

        public virtual string ReviewPartIdentifier { get; set; }

        public virtual WorkflowStatus ApprovingStatus { get; set; }

        public virtual WorkflowStatus RejectingStatus { get; set; }

        public virtual DateTime? ReviewDate { get; set; }

        public virtual WorkflowStatus ReviewDecision { get; set; }

        public virtual string ReviewerName { get; set; }

        public virtual string ReviewerEmail { get; set; }

        public ReviewStepRecord()
        {
            ApprovingStatus = WorkflowStatus.Undefined;
            RejectingStatus = WorkflowStatus.Undefined;
            ReviewDecision = WorkflowStatus.Undefined;
            ReviewDate = null;
        }
    }
}