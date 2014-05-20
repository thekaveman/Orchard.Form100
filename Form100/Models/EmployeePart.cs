using Orchard.ContentManagement;

namespace CSM.Form100.Models
{
    public class EmployeePart : ContentPart<EmployeePartRecord>
    {
        public int EmployeeId
        {
            get { return Record.EmployeeId; }
            set { Record.EmployeeId = value; }
        }

        public string FirstName
        {
            get { return Record.FirstName; }
            set { Record.FirstName = value; }
        }

        public string LastName
        {
            get { return Record.LastName; }
            set { Record.LastName = value; }
        }

        public JobStepRecord CurrentJobStep
        {
            get { return Record.CurrentJobStep; }
            set { Record.CurrentJobStep = value; }
        }

        public JobStepRecord PriorJobStep
        {
            get { return Record.PriorJobStep; }
            set { Record.PriorJobStep = value; }
        }
    }
}