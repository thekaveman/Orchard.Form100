using System;
using System.ComponentModel.DataAnnotations;

namespace CSM.Form100.Models
{
    public class ReviewApprovalRecord
    {
        public virtual int Id { get; set; }

        public virtual bool IsApproved { get; set; }

        public virtual DateTime? DateOfApproval { get; set; }

        [StringLength(1024)]
        public virtual string ApproverName { get; set; }

        public ReviewApprovalRecord()
        {
            IsApproved = false;
        }
    }
}