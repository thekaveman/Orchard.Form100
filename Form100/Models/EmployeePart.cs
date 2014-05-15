using Orchard.ContentManagement;

namespace CSM.Form100.Models
{
    public class EmployeePart : ContentPart<EmployeePartRecord>
    {
        public int? EmployeeId
        {
            get { return Retrieve(e => e.EmployeeId); }
            set { Store(e => e.EmployeeId, value); }
        }

        public string FirstName
        {
            get { return Retrieve(e => e.FirstName); }
            set { Store(e => e.FirstName, value); }
        }

        public string LastName
        {
            get { return Retrieve(e => e.LastName); }
            set { Store(e => e.LastName, value); }
        }

        public JobStepRecord PriorJobClass
        {
            get { return Retrieve(e => e.PriorJobClass); }
            set { Store(e => e.PriorJobClass, value); }
        }

        public JobStepRecord CurrentJobClass
        {
            get { return Retrieve(e => e.CurrentJobClass); }
            set { Store(e => e.CurrentJobClass, value); }
        }
    }
}