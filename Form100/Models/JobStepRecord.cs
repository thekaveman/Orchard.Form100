using Orchard.Data.Conventions;
using System.ComponentModel.DataAnnotations;

namespace CSM.Form100.Models
{
    public class JobStepRecord
    {
        public virtual int Id { get; set; }

        [StringLength(RangeProvider.MaxStringLength)]
        public virtual string Title { get; set; }

        [StringLength(RangeProvider.MaxStringLength)]
        public virtual string DepartmentName { get; set; }

        [StringLength(RangeProvider.MaxStringLength)]
        public virtual string DivisionName { get; set; }

        public virtual int DivisionNumber { get; set; }

        [Range(RangeProvider.MinStepNumber, RangeProvider.MaxStepNumber)]
        public virtual int StepNumber { get; set; }
        
        [Range(RangeProvider.MinHoursPerWeek, RangeProvider.MaxHoursPerWeek)]
        public virtual int HoursPerWeek { get; set; }

        [Range(RangeProvider.MinHourlyPayRate, double.MaxValue)]
        public virtual decimal HourlyPayRate { get; set; }
    }
}