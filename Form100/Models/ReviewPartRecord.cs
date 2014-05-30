using Orchard.ContentManagement.Records;
using Orchard.Data.Conventions;

namespace CSM.Form100.Models
{
    public class ReviewPartRecord : ContentPartRecord
    {
        public virtual string PendingReviewsIds { get; set; }

        public virtual string ReviewHistoryIds { get; set; }

        public virtual WorkflowStatus Status { get; set; }

        public ReviewPartRecord()
        {
            Status = WorkflowStatus.New;
        }
    }
}