using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CSM.Form100.Models;

namespace CSM.Form100.ViewModels
{
    public class ReviewPartViewModel
    {
        [Required]
        public WorkflowStatus Status { get; set; }

        [Required,
         RegularExpression(@"\[.+\]", ErrorMessage = "Please add at least one reviewer."),
         Display(Name = "Approval Chain")]
        public string ApprovalChainData { get; set; }

        public ReviewPartViewModel()
        {
            Status = WorkflowStatus.New;
        }
    }
}