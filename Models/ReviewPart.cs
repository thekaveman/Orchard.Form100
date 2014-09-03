using CSM.Form100.Collections;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;

namespace CSM.Form100.Models
{
    public class ReviewPart : ContentPart<ReviewPartRecord>
    {
        internal readonly LazyField<ReviewChain> PendingReviewsField = new LazyField<ReviewChain>();
        internal readonly LazyField<ReviewChain> ReviewHistoryField = new LazyField<ReviewChain>();

        public ReviewChain PendingReviews
        {
            get { return PendingReviewsField.Value; }
        }

        public ReviewChain ReviewHistory
        {
            get { return ReviewHistoryField.Value; }
        }
        
        public WorkflowStatus State
        {
            get { return Record.State; }
            set { Record.State = value; }
            //get { return Retrieve(r => r.Status); }
            //set { Store(r => r.Status, value); }
        }
    }
}