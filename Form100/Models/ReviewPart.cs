using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;

namespace CSM.Form100.Models
{
    public class ReviewPart : ContentPart<ReviewPartRecord>
    {
        internal readonly LazyField<Queue<ReviewStepRecord>> PendingReviewsField = new LazyField<Queue<ReviewStepRecord>>();
        internal readonly LazyField<Stack<ReviewStepRecord>> ReviewHistoryField = new LazyField<Stack<ReviewStepRecord>>();

        public Queue<ReviewStepRecord> PendingReviews
        {
            get { return PendingReviewsField.Value; }
            set { PendingReviewsField.Value = value; }
        }

        public Stack<ReviewStepRecord> ReviewHistory
        {
            get { return ReviewHistoryField.Value; }
            set { ReviewHistoryField.Value = value; }
        }

        public WorkflowStatus Status
        {
            get { return Record.Status; }
            set { Record.Status = value; }
        }
    }
}