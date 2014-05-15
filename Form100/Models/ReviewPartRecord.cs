using System.Collections.Generic;
using Orchard.ContentManagement.Records;
using System;

namespace CSM.Form100.Models
{
    public class ReviewPartRecord : ContentPartRecord
    {
        public virtual WorkflowStatus Status { get; set; }

        public virtual string ApprovalChainIds { get; set; }
    }
}