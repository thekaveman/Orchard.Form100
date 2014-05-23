using System.ComponentModel.DataAnnotations;
using CSM.Form100.Models;

namespace CSM.Form100.ViewModels
{
    public class EmployeePartViewModel
    {
        [Required,
         RegularExpression(@"\d+", ErrorMessage = "Employee ID is an integer"),
         Display(Name = "Employee ID")]
        public string EmployeeId { get; set; }

        [Required,
         Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required,
         Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required,
         Display(Name = "Job Class as of Effective Date")]
        public JobStepRecordViewModel CurrentJobStep { get; set; }

        public bool NeedsPriorJobStepEditor { get; set; }

        [Display(Name = "Job Class prior to Effective Date")]
        public JobStepRecordViewModel PriorJobStep { get; set; }

        public EmployeePartViewModel()
        {
            NeedsPriorJobStepEditor = false;
        }
    }
}