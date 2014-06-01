using Orchard.ContentManagement;

namespace CSM.Form100.Models
{
    public class EmployeePart : ContentPart<EmployeePartRecord>
    {
        public int EmployeeId
        {
            //get { return Record.EmployeeId; }
            //set { Record.EmployeeId = value; }
            get { return Retrieve(e => e.EmployeeId); }
            set { Store(e => e.EmployeeId, value); }
        }

        public string FirstName
        {
            //get { return Record.FirstName; }
            //set { Record.FirstName = value; }
            get { return Retrieve(e => e.FirstName); }
            set { Store(e => e.FirstName, value); }
        }

        public string LastName
        {
            //get { return Record.LastName; }
            //set { Record.LastName = value; }
            get { return Retrieve(e => e.LastName); }
            set { Store(e => e.LastName, value); }
        }

        public JobStepRecord CurrentJobStep
        {
            //get { return Record.CurrentJobStep; }
            //set { Record.CurrentJobStep = value; }
            get { return Retrieve(e => e.CurrentJobStep); }
            set { Store(e => e.CurrentJobStep, value); }
        }

        public JobStepRecord PriorJobStep
        {
            //get { return Record.PriorJobStep; }
            //set { Record.PriorJobStep = value; }
            get { return Retrieve(e => e.PriorJobStep); }
            set { Store(e => e.PriorJobStep, value); }
        }
    }
}