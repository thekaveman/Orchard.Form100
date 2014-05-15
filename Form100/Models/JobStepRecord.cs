using Orchard.Data.Conventions;
using System.ComponentModel.DataAnnotations;

namespace CSM.Form100.Models
{
    public class JobStepRecord
    {
        public virtual int Id { get; set; }

        public virtual string Title { get; set; }

        public virtual string DepartmentName { get; set; }

        public virtual string DivisionName { get; set; }

        public virtual int DivisionNumber { get; set; }

        public virtual int StepNumber { get; set; }
        
        public virtual int HoursPerWeek { get; set; }

        public virtual decimal HourlyPay { get; set; }
    }
}