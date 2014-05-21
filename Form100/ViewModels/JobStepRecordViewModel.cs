using System.ComponentModel.DataAnnotations;

namespace CSM.Form100.ViewModels
{
    public class JobStepRecordViewModel
    {
        public int Id { get; set; }

        public string Qualifier { get; set; }
        
        [Required]
        public string Title { get; set; }

        [Required,
         Display(Name = "Department")]
        public string DepartmentName { get; set; }

        [Required,
         Display(Name = "Division")]
        public string DivisionName { get; set; }

        [Required,
         RegularExpression(@"\d+", ErrorMessage = "Division Number is an integer"),
         Display(Name = "Division Number")]
        public string DivisionNumber { get; set; }

        [Required,
         RegularExpression(@"\d+", ErrorMessage = "Step is an integer"),
         Display(Name = "Step")]
        public string StepNumber { get; set; }

        [Required,
         RegularExpression(@"\d+", ErrorMessage = "Hours/Week is an integer"),
         Display(Name = "Hours/Week")]
        public string HoursPerWeek { get; set; }

        [Required,
         RegularExpression(@"\d+\.\d{2}", ErrorMessage = "Basic Rate of Pay is a decimal number"),
         Display(Name = "Basic Rate of Pay")]
        public string HourlyRate { get; set; }
    }
}