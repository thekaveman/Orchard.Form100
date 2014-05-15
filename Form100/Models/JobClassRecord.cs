using Orchard.Data.Conventions;
using System.ComponentModel.DataAnnotations;

namespace CSM.Form100.Models
{
    public class JobClassRecord
    {
        public virtual int Id { get; set; }

        [StringLength(1024)]
        public virtual string Title { get; set; }

        [StringLength(1024)]
        public virtual string DepartmentName { get; set; }

        [StringLength(1024)]
        public virtual string DivisionName { get; set; }

        [StringLength(1024)]
        public virtual string DivisionNumber { get; set; }

        [Range(1,5)]
        public virtual int Step { get; set; }

        [Range(0.00, double.MaxValue)]
        public virtual decimal PayRate { get; set; }

        public virtual int HoursPerWeek { get; set; }
    }
}