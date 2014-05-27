using System;

namespace CSM.Form100.Models
{
    public class ReviewDecisionRecord
    {
        public virtual int Id { get; set; }

        public virtual string ReviewPartIdentifier { get; set; }

        public virtual WorkflowStatus TargetStatus { get; set; }

        public virtual DateTime? ReviewDate { get; set; }

        public virtual string ReviewerName { get; set; }

        public virtual string ReviewerEmail { get; set; }

        public ReviewDecisionRecord()
        {
            TargetStatus = WorkflowStatus.Undefined;
            ReviewDate = null;
        }
    }
}