using Orchard.Data.Conventions;
using System.ComponentModel.DataAnnotations;

namespace CSM.Form100.Models
{
    public class JobStepRecord
    {
        public virtual int Id { get; set; }

        [StringLengthMax]
        public virtual string Title { get; set; }

        [StringLengthMax]
        public virtual string DepartmentName { get; set; }

        [StringLengthMax]
        public virtual string DivisionName { get; set; }

        public virtual int DivisionNumber { get; set; }

        [Range(0, 5)]
        public virtual int StepNumber { get; set; }
        
        [Range(0, 24*7)]
        public virtual int HoursPerWeek { get; set; }

        [Range(0.00, double.MaxValue)]
        public virtual decimal HourlyPay { get; set; }
    }
}