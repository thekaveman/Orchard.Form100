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

        [Required]
        public JobStepRecordViewModel CurrentJobStep { get; set; }

        public JobStepRecordViewModel PriorJobStep { get; set; }
    }
}