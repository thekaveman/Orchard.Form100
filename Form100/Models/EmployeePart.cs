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

        public JobStepRecord PriorJobStep
        {
            get { return Retrieve(e => e.PriorJobStep); }
            set { Store(e => e.PriorJobStep, value); }
        }

        public JobStepRecord CurrentJobStep
        {
            get { return Retrieve(e => e.CurrentJobStep); }
            set { Store(e => e.CurrentJobStep, value); }
        }
    }
}