using System;
using Orchard.ContentManagement;

namespace CSM.Form100.Models
{
    public class ActionPart : ContentPart<ActionPartRecord>
    {
        public DateTime? EffectiveDate
        {
            get { return Record.EffectiveDate; }
            set { Record.EffectiveDate = value; }
        }

        public ActionCategory Category
        {
            get { return Record.Category; }
            set { Record.Category = value; }
        }

        public string Type
        {
            get { return Record.Type; }
            set { Record.Type = value; }
        }

        public string Detail
        {
            get { return Record.Detail; }
            set { Record.Detail = value; }
        }
    }
}