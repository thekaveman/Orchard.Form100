using System;
using System.ComponentModel.DataAnnotations;
using CSM.Form100.Models;
using Newtonsoft.Json;

namespace CSM.Form100.ViewModels
{
    public class ReviewPartViewModel
    {
        [Required]
        public WorkflowStatus Status { get; set; }

        [Required,
         RegularExpression(@"\[.+\]", ErrorMessage = "Please add at least one reviewer."),
         Display(Name = "Pending Reviews")]
        public string PendingReviewsData { get; set; }

        [Display(Name = "Review History")]
        public string ReviewHistoryData { get; set; }

        public string AvailableStatuses { get; private set; }

        public WorkflowStatus DefaultStatus { get; private set; }

        public ReviewPartViewModel()
        {
            Status = WorkflowStatus.New;
            DefaultStatus = WorkflowStatus.Undefined;
            AvailableStatuses = JsonConvert.SerializeObject(Enum.GetNames(typeof(WorkflowStatus)));
        }
    }
}