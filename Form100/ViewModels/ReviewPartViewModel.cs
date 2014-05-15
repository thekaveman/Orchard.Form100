using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CSM.Form100.Models;

namespace CSM.Form100.ViewModels
{
    public class ReviewPartViewModel
    {
        [Required]
        public Queue<ReviewDecisionRecord> ApprovalChain { get; set; }

        [Required]
        public WorkflowStatus Status { get; set; }
    }
}