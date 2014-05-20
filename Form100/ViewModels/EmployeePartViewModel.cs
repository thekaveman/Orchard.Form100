using System.ComponentModel.DataAnnotations;
using CSM.Form100.Models;

namespace CSM.Form100.ViewModels
{
    public class EmployeePartViewModel
    {
        [Required, Display(Name = "Employee ID")]
        public string EmployeeId { get; set; }

        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public JobStepRecord CurrentJobStep { get; set; }

        public JobStepRecord PriorJobStep { get; set; }
    }
}