using System;
using Orchard.ContentManagement.Records;
using Orchard.Data.Conventions;

namespace CSM.Form100.Models
{
    public class ActionPartRecord : ContentPartRecord
    {
        public virtual DateTime? EffectiveDate { get; set; }

        public virtual ActionCategory Category { get; set; }

        public virtual string Type { get; set; }

        public virtual string Detail { get; set; }

        public ActionPartRecord()
        {
            Category = ActionCategory.Undefined;
        }
    }
}