using Orchard.ContentManagement.Records;
using Orchard.Data.Conventions;

namespace CSM.Form100.Models
{
    public class EmployeePartRecord : ContentPartRecord
    {
        public virtual int EmployeeId { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual JobStepRecord PriorJobStep { get; set; }

        public virtual JobStepRecord CurrentJobStep { get; set; }
    }
}