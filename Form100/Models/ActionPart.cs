using System;
using Orchard.ContentManagement;

namespace CSM.Form100.Models
{
    public class ActionPart : ContentPart<ActionPartRecord>
    {
        public DateTime? EffectiveDate
        {
            get { return Retrieve(a => a.EffectiveDate); }
            set { Store(a => a.EffectiveDate, value); }
        }

        public ActionCategory Category
        {
            get { return Retrieve(a => a.Category); }
            set { Store(a => a.Category, value); }
        }

        public string Type
        {
            get { return Retrieve(a => a.Type); }
            set { Store(a => a.Type, value); }
        }

        public string Detail
        {
            get { return Retrieve(a => a.Detail); }
            set { Store(a => a.Detail, value); }
        }
    }
}