using Orchard.ContentManagement;
using Orchard.ContentManagement.FieldStorage;

namespace CSM.Form100.Fields
{
    public class WorkflowStatusField : ContentField
    {
        public WorkflowStatus Value
        {
            get { return Storage.Get<WorkflowStatus>(); }
            set { Storage.Set(value); }
        }
    }
}