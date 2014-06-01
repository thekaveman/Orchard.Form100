using System;
using Orchard.ContentManagement;

namespace CSM.Form100.Models
{
    public class ActionPart : ContentPart<ActionPartRecord>
    {
        public DateTime? EffectiveDate
        {
            //get { return Record.EffectiveDate; }
            //set { Record.EffectiveDate = value; }
            get { return Retrieve(a => a.EffectiveDate); }
            set { Store(a => a.EffectiveDate, value); }
        }

        public ActionCategory Category
        {
            //get { return Record.Category; }
            //set { Record.Category = value; }
            get { return Retrieve(a => a.Category); }
            set { Store(a => a.Category, value); }
        }

        public string Type
        {
            //get { return Record.Type; }
            //set { Record.Type = value; }
            get { return Retrieve(a => a.Type); }
            set { Store(a => a.Type, value); }
        }

        public string Detail
        {
            //get { return Record.Detail; }
            //set { Record.Detail = value; }
            get { return Retrieve(a => a.Detail); }
            set { Store(a => a.Detail, value); }
        }
    }
}