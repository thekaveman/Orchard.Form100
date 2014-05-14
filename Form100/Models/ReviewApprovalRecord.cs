using System;

namespace CSM.Form100.Models
{
    public class ReviewApprovalRecord
    {
        public virtual int Id { get; set; }

        public virtual bool IsApproved { get; set; }

        public virtual DateTime? DateOfApproval { get; set; }

        public virtual string Approver { get; set; }

        public ReviewApprovalRecord()
        {
            IsApproved = false;
        }
    }
}