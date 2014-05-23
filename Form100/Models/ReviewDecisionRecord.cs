using System;

namespace CSM.Form100.Models
{
    public class ReviewDecisionRecord
    {
        public virtual int Id { get; set; }

        public virtual int ReviewPartId { get; set; }

        public virtual bool? IsApproved { get; set; }

        public virtual DateTime? ReviewDate { get; set; }

        public virtual string ReviewerName { get; set; }

        public virtual string ReviewerEmail { get; set; }

        public ReviewDecisionRecord()
        {
            IsApproved = null;
            ReviewDate = null;
        }
    }
}