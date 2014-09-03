using System;
using System.Collections.Generic;
using System.Linq;

namespace CSM.Form100.Models
{
    public class ReviewStepRecord
    {
        public virtual int Id { get; set; }

        public virtual string ReviewPartIdentifier { get; set; }

        public virtual string TargetStates { get; set; }
        
        public virtual WorkflowStatus ApprovingState { get; set; }

        public virtual WorkflowStatus RejectingState { get; set; }

        public virtual DateTime? NotificationDate { get; set; }

        public virtual DateTime? ReviewDate { get; set; }

        public virtual WorkflowStatus ReviewDecision { get; set; }

        public virtual string ReviewerName { get; set; }

        public virtual string ReviewerEmail { get; set; }

        public ReviewStepRecord()
        {
            ApprovingState = WorkflowStatus.Undefined;
            RejectingState = WorkflowStatus.Undefined;
            NotificationDate = null;
            ReviewDate = null;
            ReviewDecision = WorkflowStatus.Undefined;
        }

        internal ReviewStepRecord(ReviewStepRecord other)
        {
            ReviewPartIdentifier = other.ReviewPartIdentifier;
            TargetStates = other.TargetStates;
            ApprovingState = other.ApprovingState;
            RejectingState = other.RejectingState;
            NotificationDate = null;
            ReviewDate = null;
            ReviewDecision = WorkflowStatus.Undefined;
            ReviewerName = other.ReviewerName;
            ReviewerEmail = other.ReviewerEmail;
        }
    }
}