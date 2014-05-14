using System.ComponentModel.DataAnnotations;

namespace CSM.Form100.Models
{
    public class JobClassRecord
    {
        public virtual int Id { get; set; }

        [StringLength(1024)]
        public virtual string Title { get; set; }

        [Range(1,5)]
        public virtual int? Step { get; set; }

        public virtual decimal? PayRate { get; set; }

        public virtual int? HoursPerWeek { get; set; }
    }
}