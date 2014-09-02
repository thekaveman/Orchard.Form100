using System;
using System.ComponentModel.DataAnnotations;
using CSM.Form100.Models;
using Newtonsoft.Json;

namespace CSM.Form100.ViewModels
{
    public class ReviewPartViewModel
    {
        [Required]
        public WorkflowStatus State { get; set; }

        [Required,
         RegularExpression(@"\[.+\]", ErrorMessage = "Please add at least one reviewer."),
         Display(Name = "Pending Reviews")]
        public string PendingReviewsData { get; set; }

        [Display(Name = "Review History")]
        public string ReviewHistoryData { get; set; }

        public string AvailableStates { get; private set; }

        public WorkflowStatus DefaultState { get; private set; }

        public ReviewPartViewModel()
        {
            State = WorkflowStatus.New;
            DefaultState = WorkflowStatus.Undefined;
            AvailableStates = JsonConvert.SerializeObject(Enum.GetNames(typeof(WorkflowStatus)));
        }
    }
}