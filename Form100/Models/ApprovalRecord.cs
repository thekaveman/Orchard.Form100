using System;

namespace CSM.Form100.Models
{
    public class ApprovalRecord
    {
        public virtual int Id { get; set; }

        public virtual bool IsApproved { get; set; }

        public virtual DateTime? DateOfApproval { get; set; }

        public virtual string Approver { get; set; }

        public ApprovalRecord()
        {
            IsApproved = false;
        }
    }
}